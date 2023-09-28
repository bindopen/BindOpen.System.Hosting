using BindOpen.Kernel.Data;
using BindOpen.Kernel.Data.Helpers;
using BindOpen.Kernel.Data.Meta;
using BindOpen.Kernel.Hosting.Settings;
using BindOpen.Kernel.Logging;
using BindOpen.Kernel.Logging.Loggers;
using BindOpen.Kernel.Scoping;
using System;
using System.IO;
using System.Linq;

namespace BindOpen.Kernel.Hosting
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
        public IBdoHostOptions Options { get; set; }

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

            Initialize();

            //log?.Sanitize();

            var log = Logger?.NewRootLog();
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

            Logger?.Log(log);
        }

        /// <summary>
        /// Indicates the application ends.
        /// </summary>
        public virtual void Stop()
        {
            // we unload the host (syncrhonously for the moment)
            _state = ProcessExecutionState.Ended;
            Clear();

            var log = Logger?.NewRootLog();
            log?.AddEvent(EventKinds.Message, q => q.WithTitle("Host ended"));
            Logger?.Log(log);
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
            var action = Options?.EventActions?.FirstOrDefault(q => (eventKind & HostEventKinds.Any) == (q.EventKind & HostEventKinds.Any));
            action?._Action?.Invoke(this);
        }

        // Paths --------------------------------------

        /// <summary>
        /// Initializes information.
        /// </summary>
        /// <returns>Returns the log of the task.</returns>
        protected virtual bool Initialize()
        {
            var loaded = true;

            // we update options (specially paths)

            Options.Update();

            // we set the logger

            Logger = Options.LoggerInit?.Invoke(this);

            var log = Logger?.NewRootLog();

            // we launch the standard initialization of service

            var subLog = log?.InsertChild(EventKinds.Message, "Initializing host...");

            IBdoLog childLog = null;

            DataStore.Add(("$host", this));

            try
            {
                // we load the host config

                childLog = subLog?.InsertChild(EventKinds.Message, "Loading host configuration...");

                Options.Settings ??= BdoData.NewMetaWrapper<BdoHostSettings>(this);

                if (Options?.ConfigurationFiles != null)
                {
                    foreach (var file in Options.ConfigurationFiles)
                    {
                        if (loaded)
                        {
                            var path = file.Path.GetConcatenatedPath(this.GetKnownPath(BdoHostPathKind.RootFolder));

                            if (!File.Exists(path))
                            {
                                loaded = false;
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
                                    fileExtension = (Path.GetExtension(path)?.ToLower()) switch
                                    {
                                        ".json" => ConfigurationFileExtenions.Json,
                                        _ => ConfigurationFileExtenions.Xml,
                                    };
                                }

                                switch (fileExtension)
                                {
                                    case ConfigurationFileExtenions.Json:
                                        configDto = JsonHelper.LoadJson<ConfigurationDto>(path, log);
                                        break;
                                    case ConfigurationFileExtenions.Xml:
                                        configDto = XmlHelper.LoadXml<ConfigurationDto>(path, log);
                                        break;
                                }

                                var config = configDto.ToPoco();

                                Options.Settings.UpdateDetail(config);
                                Options.Settings.UpdateProperties();

                                if (childLog?.HasEvent(EventKinds.Error, EventKinds.Exception) != true)
                                {
                                    childLog?.AddEvent(EventKinds.Message, "Host config loaded");
                                }
                            }
                        }
                    }
                }

                if (loaded)
                {
                    // we load extensions

                    childLog = subLog?.InsertChild(EventKinds.Message, "Loading extensions...");

                    loaded &= this.LoadExtensions(
                        q => q = Options.ExtensionLoadOptions
                            .AddSource(DatasourceKind.Repository, this.GetKnownPath(BdoHostPathKind.LibraryFolder)),
                        childLog);
                }

                if (_state == ProcessExecutionState.Pending)
                {
                    // we load the data store

                    Clear();

                    DepotStore = Options?.DepotStore;

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

            Logger?.Log(log);

            _state = loaded ? ProcessExecutionState.Pending : ProcessExecutionState.Ended;

            return loaded;
        }

        #endregion
    }
}