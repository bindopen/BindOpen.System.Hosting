using BindOpen.System.Data;
using BindOpen.System.Data.Meta;
using BindOpen.System.IO.Dtos;
using BindOpen.System.Logging;
using BindOpen.System.Processing;
using BindOpen.System.Scoping;
using System;
using System.IO;
using System.Linq;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a host.
    /// </summary>
    public partial class BdoHost : BdoScope, IBdoHost
    {
        // ------------------------------------------
        // CONSTRUCTORS
        // ------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoHost class.
        /// </summary>
        /// <param key="log"></param>
        public BdoHost()
        {
        }

        #endregion

        // ------------------------------------------
        // IBdoHost Implementation
        // ------------------------------------------

        #region IBdoHost

        /// <summary>
        /// The options of this instance.
        /// </summary>
        public IBdoHostSettings Settings { get; set; }

        public IBdoLogger Logger { get; set; }

        public ProcessExecutionState State => _state;

        protected ProcessExecutionState _state;

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <returns>Returns true if this instance is started.</returns>
        public virtual void Start()
        {
            // we start the application instance

            _state = ProcessExecutionState.Pending;

            // we initialize this instance

            var log = BdoLogging.NewLog();

            Initialize(log);

            log?.Sanitize();

            log?.AddEvent(EventKinds.Message, "Host starting...");

            if (_state == ProcessExecutionState.Pending)
            {
                log?.AddEvent(EventKinds.Message, "Host started successfully");
                InitSucceeds();
            }
            else
            {
                log?.AddEvent(EventKinds.Message, "Host loaded with errors");
                Stop();
                InitFails();
            }
        }

        /// <summary>
        /// Indicates the application ends.
        /// </summary>
        public virtual void Stop()
        {
            // we unload the host (syncrhonously for the moment)
            _state = ProcessExecutionState.Ended;
            Clear();

            Logger?.Log(BdoLogging.NewLogEvent(EventKinds.Message, q => q.WithDisplayName("Host ended")));
        }

        // Trigger actions --------------------------------------

        public event EventHandler OnInitSucceeds;

        /// <summary>
        /// Indicates that this instance has successfully started.
        /// </summary>
        private void InitSucceeds()
        {
            InvokeTriggerAction(HostEventKinds.OnInitSuccess);

            OnInitSucceeds?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnInitFails;

        /// <summary>
        /// Indicates that this instance has not successfully started.
        /// </summary>
        private void InitFails()
        {
            InvokeTriggerAction(HostEventKinds.OnInitFailure);

            OnInitFails?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnExecutionSucceeds;

        /// <summary>
        /// Indicates that this instance completes.
        /// </summary>
        private void ExecutionSucceeds()
        {
            InvokeTriggerAction(HostEventKinds.OnExecutionSucess);

            OnExecutionSucceeds?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnExecutionFails;

        /// <summary>
        /// Indicates that this instance fails.
        /// </summary>
        private void ExecutionFails()
        {
            InvokeTriggerAction(HostEventKinds.OnExecutionFailure);

            OnExecutionFails?.Invoke(this, new EventArgs());
        }

        private void InvokeTriggerAction(HostEventKinds eventKind)
        {
            var action = Settings?.EventActions?.FirstOrDefault(q => (eventKind & HostEventKinds.Any) == (q.EventKind & HostEventKinds.Any));
            action?._Action?.Invoke(this);
        }

        // Paths --------------------------------------

        /// <summary>
        /// Initializes information.
        /// </summary>
        /// <returns>Returns the log of the task.</returns>
        protected virtual bool Initialize(IBdoLog log = null)
        {
            var loaded = true;

            // we update options (specially paths)

            Settings.Update();

            // we set the logger

            log?.WithLogger(Settings.LoggerInit?.Invoke(this));

            // we launch the standard initialization of service

            var subLog = log?.InsertChild(EventKinds.Message, "Initializing host...");

            IBdoLog childLog = null;

            DataStore.Add(("$host", this));

            try
            {
                if (_state == ProcessExecutionState.Pending)
                {
                    // we load the host config

                    childLog = subLog?.InsertChild(EventKinds.Message, "Loading host configuration...");

                    if (Settings?.ConfigurationFiles != null)
                    {
                        foreach (var file in Settings.ConfigurationFiles)
                        {
                            if (!File.Exists(file.Path))
                            {
                                subLog?.AddEvent(
                                    file.IsRequired ? EventKinds.Error : EventKinds.Warning,
                                    "Host config file ('" + BdoDefaultHostPaths.__DefaultHostConfigFileName + "') not found");
                            }
                            else
                            {
                                ConfigurationDto configDto = null;
                                var fileExtension = ConfigurationFileExtenions.Any;

                                if (fileExtension == ConfigurationFileExtenions.Any)
                                {
                                    switch (Path.GetExtension(file.Path)?.ToLower())
                                    {
                                        case ".json":
                                            fileExtension = ConfigurationFileExtenions.Json;
                                            break;
                                        case ".xml":
                                        default:
                                            fileExtension = ConfigurationFileExtenions.Xml;
                                            break;
                                    }
                                }

                                switch (fileExtension)
                                {
                                    case ConfigurationFileExtenions.Json:
                                        configDto = JsonHelper.LoadJson<ConfigurationDto>(file.Path);
                                        break;
                                    case ConfigurationFileExtenions.Xml:
                                        configDto = XmlHelper.LoadXml<ConfigurationDto>(file.Path);
                                        break;
                                }

                                var config = configDto.ToPoco();

                                Settings.Configuration = BdoData.NewMetaWrapper<BdoHostConfigWrapper>(this, config);

                                if (childLog?.HasEvent(EventKinds.Error, EventKinds.Exception) != true)
                                {
                                    childLog?.AddEvent(EventKinds.Message, "Host config loaded");
                                }
                            }
                        }
                    }
                }

                if (_state == ProcessExecutionState.Pending)
                {
                    // we load extensions

                    childLog = subLog?.InsertChild(EventKinds.Message, "Loading extensions...");

                    loaded &= this.LoadExtensions(
                        q => q = Settings.ExtensionLoadOptions
                            .AddSource(DatasourceKind.Repository, this.GetKnownPath(BdoHostPathKind.LibraryFolder)),
                        childLog);
                }

                if (_state == ProcessExecutionState.Pending)
                {
                    // we load the data store

                    Clear();

                    DepotStore = Settings?.DepotStore;

                    childLog = subLog?.InsertChild(EventKinds.Message, "Loading data store...");
                    if (DepotStore == null)
                    {
                        childLog?.AddEvent(EventKinds.Message, title: "No data store registered");
                    }
                    else
                    {
                        loaded &= DepotStore.LoadLazy(this, childLog);

                        if (childLog?.HasEvent(EventKinds.Error, EventKinds.Exception) != true)
                        {
                            childLog?.AddEvent(EventKinds.Message, "Data store loaded (" + DepotStore.Depots.Count + " depots added)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                subLog?.AddException(ex);
            }
            finally
            {
            }

            _state = loaded ? ProcessExecutionState.Pending : ProcessExecutionState.Ended;

            return loaded;
        }

        #endregion
    }
}