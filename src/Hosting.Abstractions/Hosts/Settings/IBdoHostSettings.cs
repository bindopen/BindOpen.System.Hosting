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
    public interface IBdoHostSettings : IBdoObject
    {
        void Update();

        // Root ----------------------

        string RootFolderPath { get; }

        /// <summary>
        /// The root folder path.
        /// </summary>
        List<(Predicate<IBdoHostSettings> Predicate, string Path)> RootFolderPathAssignments { get; set; }

        // Library ----------------------

        string LibraryFolderPath { get; set; }

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

        // Configuration ----------------------

        /// <summary>
        /// The settings.
        /// </summary>
        IBdoHostConfigWrapper Configuration { get; set; }

        /// <summary>
        /// The settings file path of this instance.
        /// </summary>
        List<(string Path, bool IsRequired, ConfigurationFileExtenions FileKind)> ConfigurationFiles { get; set; }

        IBdoHostSettings SetConfiguration(Action<IBdoHostConfigWrapper> action = null);

        IBdoHostSettings SetConfiguration<T>(Action<T> action = null) where T : class, IBdoHostConfigWrapper, new();

        // Loggers ----------------------

        /// <summary>
        /// The logger of this instance.
        /// </summary>
        public Func<IBdoHost, IBdoLogger> LoggerInit { get; set; }

        // Trigger actions ----------------------

        public List<(HostEventKinds EventKind, Action<IBdoHost> _Action)> EventActions { get; set; }
    }
}