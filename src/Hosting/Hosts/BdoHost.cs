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

        public ProcessExecutionState State { get; set; }

        public IBdoLog Log { get; set; }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <returns>Returns true if this instance is started.</returns>
        public virtual void Start()
        {
            // we start the application instance

            State = ProcessExecutionState.Pending;

            // we initialize this instance
            Initialize();

            Log?.Sanitize();

            Log?.AddEvent(EventKinds.Message, "Host starting...");

            if (State == ProcessExecutionState.Pending)
            {
                Log?.AddEvent(EventKinds.Message, "Host started successfully");
                StartSucceeds();
            }
            else
            {
                Log?.AddEvent(EventKinds.Message, "Host loaded with errors");
                Stop();
                StartFails();
            }
        }

        /// <summary>
        /// Indicates the application ends.
        /// </summary>
        public virtual void Stop()
        {
            // we unload the host (syncrhonously for the moment)
            State = ProcessExecutionState.Ended;
            Clear();

            Log?.AddEvent(EventKinds.Message, "Host ended");
        }

        // Trigger actions --------------------------------------

        /// <summary>
        /// Indicates that this instance has successfully started.
        /// </summary>
        private void StartSucceeds()
        {
            Settings?.Action_OnStartSuccess?.Invoke(this);
        }

        /// <summary>
        /// Indicates that this instance has not successfully started.
        /// </summary>
        private void StartFails()
        {
            Settings?.Action_OnStartFailure?.Invoke(this);
        }

        /// <summary>
        /// Indicates that this instance completes.
        /// </summary>
        private void ExecutionSucceeds()
        {
            Settings?.Action_OnExecutionSucess?.Invoke(this);
        }

        /// <summary>
        /// Indicates that this instance fails.
        /// </summary>
        private void ExecutionFails()
        {
            Settings?.Action_OnExecutionFailure?.Invoke(this);
        }

        // Paths --------------------------------------

        /// <summary>
        /// Initializes information.
        /// </summary>
        /// <returns>Returns the log of the task.</returns>
        protected virtual bool Initialize()
        {
            var loaded = true;

            // we determine the root folder path

            var rootFolderPathDefinition = Settings?.RootFolderPathDefinitions?.FirstOrDefault(p => p.Predicate(Settings) == true);
            if (rootFolderPathDefinition != null)
            {
                Settings?.SetRootFolder(rootFolderPathDefinition?.RootFolderPath);
            }

            // we update options (specially paths)

            Settings.Update();

            // we set the logger

            Log.WithLogger(Settings.LoggerInit?.Invoke(this));

            // we launch the standard initialization of service

            var log = Log?.InsertChild(EventKinds.Message, "Initializing host...");

            IBdoLog childLog = null;

            DataStore.Add(("$host", this));

            // if no errors was found

            if (State == ProcessExecutionState.Pending)
            {
                try
                {
                    // we load the host config

                    string hostConfigFilePath = this.GetKnownPath(BdoHostPathKind.HostConfigFile);

                    if (!File.Exists(hostConfigFilePath))
                    {
                        var message = "Host config file ('" + BdoDefaultHostPaths.__DefaultHostConfigFileName + "') not found";
                        if (Settings.IConfigurationFileRequired == true)
                        {
                            log?.AddEvent(EventKinds.Error, message);
                            State = ProcessExecutionState.Ended;
                        }
                        else if (Settings.IConfigurationFileRequired == false)
                        {
                            log?.AddEvent(EventKinds.Warning, message);
                        }
                    }
                    else
                    {
                        childLog = log?.InsertChild(EventKinds.Message, "Loading host configuration...");

                        var config = XmlHelper.LoadXml<ConfigurationDto>(hostConfigFilePath).ToPoco();
                        Settings.ConfigurationWrapper = BdoData.NewMetaWrapper<BdoHostConfigWrapper>(this, config);

                        if (childLog?.HasEvent(EventKinds.Error, EventKinds.Exception) != true)
                        {
                            childLog?.AddEvent(EventKinds.Message, "Host config loaded");
                        }
                    }

                    //Options.Update().AddEventsTo(childLog);

                    // we load extensions

                    childLog = log?.InsertChild(EventKinds.Message, "Loading extensions...");

                    loaded &= this.LoadExtensions(
                        q => q = Settings.ExtensionLoadOptions
                            .AddSource(DatasourceKind.Repository, this.GetKnownPath(BdoHostPathKind.LibraryFolder)),
                        childLog);

                    if (State == ProcessExecutionState.Pending)
                    {
                        // we load the data store

                        Clear();

                        DepotStore = Settings?.DepotStore;

                        childLog = log?.InsertChild(EventKinds.Message, "Loading data store...");
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
                    log?.AddException(ex);
                }
                finally
                {
                }
            }

            State = loaded ? ProcessExecutionState.Pending : ProcessExecutionState.Ended;

            return loaded;
        }

        #endregion
    }
}