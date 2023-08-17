using BindOpen.System.Data;
using BindOpen.System.Data.Helpers;
using BindOpen.System.Data.Stores;
using BindOpen.System.Logging;
using BindOpen.System.Scoping;
using System;
using System.Collections.Generic;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a host options.
    /// </summary>
    public class BdoHostSettings : BdoObject, IBdoHostSettings
    {
        // ------------------------------------------
        // PROPERTIES
        // ------------------------------------------

        #region Properties

        // Settings ----------------------

        /// <summary>
        /// The host settings of this instance.
        /// </summary>
        public IBdoHostConfigWrapper ConfigurationWrapper { get; set; } = new BdoHostConfigWrapper();

        /// <summary>
        /// The host config file path.
        /// </summary>
        public string ConfigurationFilePath { get; set; } = (@".\" + BdoDefaultHostPaths.__DefaultHostConfigFileName).ToPath();

        /// <summary>
        /// Indicates whether the host settings file must exist.
        /// </summary>
        /// <remarks>If it does not exist then an exception is thrown.</remarks>
        public bool? IConfigurationFileRequired { get; set; }

        // Paths ----------------------

        /// <summary>
        /// The root folder path.
        /// </summary>
        public string RootFolderPath { get; set; }

        /// <summary>
        /// The root folder path.
        /// </summary>
        public List<(Predicate<IBdoHostSettings> Predicate, string RootFolderPath)> RootFolderPathDefinitions { get; set; }

        // Extensions ----------------------

        /// <summary>
        /// The extension loading options.
        /// </summary>
        public IExtensionLoadOptions ExtensionLoadOptions { get; set; }

        // Loggers -------------------------

        /// <summary>
        /// The logger initialization function of this instance.
        /// </summary>
        public Func<IBdoHost, IBdoLogger> LoggerInit { get; set; }

        // Trigger actions ----------------------

        /// <summary>
        /// The action that the start of this instance completes.
        /// </summary>
        public Action<IBdoHost> Action_OnStartSuccess { get; set; }

        /// <summary>
        /// The action that the start of this instance fails.
        /// </summary>
        public Action<IBdoHost> Action_OnStartFailure { get; set; }

        /// <summary>
        /// The action that this instance completes.
        /// </summary>
        public Action<IBdoHost> Action_OnExecutionSucess { get; set; }

        /// <summary>
        /// The action that is executed when the instance fails.
        /// </summary>
        public Action<IBdoHost> Action_OnExecutionFailure { get; set; }

        // Depot initialization actions ----------------------

        /// <summary>
        /// The data store of this instance.
        /// </summary>
        public IBdoDepotStore DepotStore { get; set; }

        #endregion

        // ------------------------------------------
        // CONSTRUCTORS
        // ------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoHostOptions class.
        /// </summary>
        public BdoHostSettings() : base()
        {
            ExtensionLoadOptions = new ExtensionLoadOptions()
                .AddSource(DatasourceKind.Memory)
                .AddSource(DatasourceKind.Repository, (@".\" + BdoDefaultHostPaths.__DefaultLibraryFolderPath).ToPath());
        }

        #endregion

        // ------------------------------------------
        // IDISPOSABLE METHODS
        // ------------------------------------------

        #region IDisposable_Methods

        private bool _isDisposed = false;

        /// <summary>
        /// Disposes this instance. 
        /// </summary>
        /// <param key="isDisposing">Indicates whether this instance is disposing</param>
        protected override void Dispose(bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            ExtensionLoadOptions?.Dispose();
            DepotStore?.Dispose();

            _isDisposed = true;

            base.Dispose(isDisposing);
        }

        #endregion
    }
}