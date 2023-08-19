using BindOpen.System.Data;
using BindOpen.System.Data.Helpers;
using BindOpen.System.Data.Stores;
using BindOpen.System.Hosting.Hosts;
using BindOpen.System.Logging;
using System;
using System.Collections.Generic;

namespace BindOpen.System.Hosting
{
    /// <summary>
    /// This class represents a settings settings.
    /// </summary>
    public static class BdoHostSettingsExtensions
    {
        // Paths -------------------------------------------

        /// <summary>
        /// Set the root folder.
        /// </summary>
        /// <param key="predicate">The condition that must be satisfied.</param>
        /// <param key="rootFolderPath">The root folder path.</param>
        /// <returns>Returns the settings option.</returns>
        public static T AddRootFolder<T>(this T settings, Predicate<IBdoHostSettings> predicate, string rootFolderPath)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.RootFolderPathAssignments ??= new List<(Predicate<IBdoHostSettings> Predicate, string RootFolderPath)>();
                settings.RootFolderPathAssignments.Add((predicate, rootFolderPath));
            }

            return settings;
        }

        /// <summary>
        /// Sets the specified root folder path.
        /// </summary>
        /// <param key="path">The path to consider.</param>
        /// <returns>Returns this instance.</returns>
        public static T SetRootFolder<T>(this T settings, string path)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.RootFolderPathAssignments ??= new List<(Predicate<IBdoHostSettings> Predicate, string RootFolderPath)>();
                settings.RootFolderPathAssignments.Add((_ => true, path?.EndingWith(@"\").ToPath()));
            }

            return settings;
        }


        // Settings -------------------------------------------

        /// <summary>
        /// Set the specified settings settings file path.
        /// </summary>
        /// <param key="path">The settings file path.</param>
        /// <param key="isRequired">Indicates whether the settings settings file is required.</param>
        /// <returns>Returns the settings option.</returns>
        public static T AddConfigurationFile<T>(
            this T settings,
            string path,
            bool isRequired = false,
            ConfigurationFileExtenions extension = ConfigurationFileExtenions.Any)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.ConfigurationFiles ??= new();
                settings.ConfigurationFiles.Add((path, isRequired, extension));
            }

            return settings;
        }

        // Logs -------------------------------------------

        /// <summary>
        /// Adds the specified logger.
        /// </summary>
        /// <param key="initLogger">The logger initialization to consider.</param>
        /// <returns>Returns the settings option.</returns>
        public static T SetLogger<T>(this T settings, Func<IBdoHost, IBdoLogger> initLogger)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.LoggerInit = initLogger;
            }

            return settings;
        }

        // Trigger actions -------------------------------------------

        /// <summary>  
        /// The action that is executed when the start of this instance succedes.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnInitSuccess<T>(this T settings, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.EventActions ??= new();
                settings.EventActions.Add((HostEventKinds.OnInitSuccess, action));
            }

            return settings;
        }

        /// <summary>
        /// The action that is executed when the start of this instance fails.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnInitFailure<T>(this T settings, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.EventActions ??= new();
                settings.EventActions.Add((HostEventKinds.OnInitFailure, action));
            }

            return settings;
        }

        /// <summary>
        /// The action that is executed when this instance is successfully completed.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnExecutionSuccess<T>(this T settings, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.EventActions ??= new();
                settings.EventActions.Add((HostEventKinds.OnExecutionSucess, action));
            }

            return settings;
        }

        /// <summary>
        /// The action that is executed when this instance execution fails.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnExecutionFailure<T>(this T settings, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.EventActions ??= new();
                settings.EventActions.Add((HostEventKinds.OnExecutionFailure, action));
            }

            return settings;
        }

        /// <summary>
        /// Throws an exception when start fails.
        /// </summary>
        public static T ThrowExceptionOnInitFailure<T>(this T settings, bool throwException = false)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.ExecuteOnInitFailure(_ => throw new BdoHostLoadException("BindOpen settings failed while loading"));
            }

            return settings;
        }

        /// <summary>
        /// Throws an exception when start fails.
        /// </summary>
        public static T ThrowExceptionOnExecutionFailure<T>(this T settings, bool throwException = false)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.ExecuteOnExecutionFailure(_ => throw new BdoHostLoadException("BindOpen settings failed while loading"));
            }

            return settings;
        }

        // Depots -------------------------------------------

        /// <summary>
        /// Adds the data store executing the specified action.
        /// </summary>
        /// <param key="action">The action to execute on the created data store.</param>
        public static T AddDepotStore<T>(this T settings, Action<IBdoDepotStore> action = null)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.DepotStore ??= BdoData.NewDepotStore();
                action?.Invoke(settings.DepotStore);
            }

            return settings;
        }
    }
}