using BindOpen.System.Data;
using BindOpen.System.Data.Stores;
using BindOpen.System.Logging;
using BindOpen.System.Scoping;
using System;
using System.Collections.Generic;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// The interface defines the base BDO host options.
    /// </summary>
    public interface IBdoHostOptions : IBdoObject
    {
        // Paths ----------------------

        /// <summary>
        /// The root folder path.
        /// </summary>
        string RootFolderPath { get; set; }

        // -------

        /// <summary>
        /// The root folder path.
        /// </summary>
        List<(Predicate<IBdoHostOptions> Predicate, string RootFolderPath)> RootFolderPathDefinitions { get; set; }

        // Extensions ----------------------

        /// <summary>
        /// The extension load options.
        /// </summary>
        IExtensionLoadOptions ExtensionLoadOptions { get; set; }

        // Depots ----------------------

        /// <summary>
        /// The depot sets of this instance.
        /// </summary>
        IBdoDepotStore DepotStore { get; set; }

        // Settings ----------------------

        /// <summary>
        /// The settings.
        /// </summary>
        IBdoHostSettings Settings { get; set; }

        /// <summary>
        /// The settings file path of this instance.
        /// </summary>
        string SettingsFilePath { get; set; }

        /// <summary>
        /// Indicates whether the host settings file must exist.
        /// </summary>
        /// <remarks>If it does not exist then an exception is thrown.</remarks>
        bool? IsSettingsFileRequired { get; set; }

        // Loggers ----------------------

        /// <summary>
        /// The logger of this instance.
        /// </summary>
        public Func<IBdoHost, IBdoLogger> LoggerInit { get; set; }

        // Trigger actions -------------------------------------------

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
    }
}