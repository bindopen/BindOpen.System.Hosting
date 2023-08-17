using BindOpen.System.Data;
using BindOpen.System.Data.Helpers;
using BindOpen.System.Data.Stores;
using BindOpen.System.Hosting.Hosts;
using BindOpen.System.Logging;
using BindOpen.System.Scoping;
using System;
using System.Collections.Generic;

namespace BindOpen.System.Hosting
{
    /// <summary>
    /// This class represents a options options.
    /// </summary>
    public static class BdoHostSettingsExtensions
    {
        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <param key="item">The item to consider.</param>
        /// <param key="specificationAreas">The specification areas to consider.</param>
        /// <param key="updateModes">The update modes to consider.</param>
        /// <returns>The log of the operation.</returns>
        /// <remarks>Put reference sets as null if you do not want to repair this instance.</remarks>
        public static T Update<T>(this T options)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                if (string.IsNullOrEmpty(options.RootFolderPath))
                {
                    options.RootFolderPath = FileHelper.GetAppRootFolderPath();
                }
                else
                {
                    options.RootFolderPath = options.RootFolderPath.GetConcatenatedPath(FileHelper.GetAppRootFolderPath()).EndingWith(@"\").ToPath();
                }

                options.ConfigurationFilePath = options.ConfigurationFilePath.GetConcatenatedPath(options.RootFolderPath).ToPath();

                //Settings?.Update(null, null, log);
                options.ConfigurationWrapper?.WithLibraryFolder(options.ConfigurationWrapper?.LibraryFolderPath.GetConcatenatedPath(options.RootFolderPath).EndingWith(@"\").ToPath());

                options.ExtensionLoadOptions?.AddSource(DatasourceKind.Repository, options.ConfigurationWrapper?.LibraryFolderPath);
            }

            return options;
        }

        // Paths -------------------------------------------

        /// <summary>
        /// Set the root folder.
        /// </summary>
        /// <param key="predicate">The condition that must be satisfied.</param>
        /// <param key="rootFolderPath">The root folder path.</param>
        /// <returns>Returns the options option.</returns>
        public static T SetRootFolder<T>(this T options, Predicate<IBdoHostSettings> predicate, string rootFolderPath)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.RootFolderPathDefinitions ??= new List<(Predicate<IBdoHostSettings> Predicate, string RootFolderPath)>();
                options.RootFolderPathDefinitions.Add((predicate, rootFolderPath));
            }

            return options;
        }

        /// <summary>
        /// Sets the specified root folder path.
        /// </summary>
        /// <param key="path">The path to consider.</param>
        /// <returns>Returns this instance.</returns>
        public static T SetRootFolder<T>(this T options, string path)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.RootFolderPath = path?.EndingWith(@"\").ToPath();
            }

            return options;
        }


        // Settings -------------------------------------------

        /// <summary>
        /// Sets the options settings applying action on it.
        /// </summary>
        /// <param key="optionsSettings">The options settings to consider.</param>
        /// <param key="action">The action to apply on the settings.</param>
        public static T SetConfiguration<T>(this T options, Action<IBdoHostConfigWrapper> action = null)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.ConfigurationWrapper = new BdoHostConfigWrapper();
                action?.Invoke(options.ConfigurationWrapper);
            }

            return options;
        }


        /// <summary>
        /// Set the specified options settings file path.
        /// </summary>
        /// <param key="path">The settings file path.</param>
        /// <param key="isRequired">Indicates whether the options settings file is required.</param>
        /// <returns>Returns the options option.</returns>
        public static T SetConfigurationFile<T>(this T options, string path, bool? isRequired = false)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.ConfigurationFilePath = path?.ToPath();
                options.SetConfigurationFile(isRequired);
            }

            return options;
        }

        /// <summary>
        /// Set the specified options settings file path.
        /// </summary>
        /// <param key="path">The settings file path.</param>
        /// <param key="isRequired">Indicates whether the options settings file is required.</param>
        /// <returns>Returns the options option.</returns>
        public static T SetConfigurationFile<T>(this T options, bool? isRequired)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.IConfigurationFileRequired = isRequired;
            }

            return options;
        }

        // Logs -------------------------------------------

        /// <summary>
        /// Adds the specified logger.
        /// </summary>
        /// <param key="initLogger">The logger initialization to consider.</param>
        /// <returns>Returns the options option.</returns>
        public static T SetLogger<T>(this T options, Func<IBdoHost, IBdoLogger> initLogger)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.LoggerInit = initLogger;
            }

            return options;
        }

        // Trigger actions -------------------------------------------

        /// <summary>  
        /// The action that is executed when the start of this instance succedes.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnStartSuccess<T>(this T options, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnStartSuccess = action;
            }

            return options;
        }

        /// <summary>
        /// The action that is executed when the start of this instance fails.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnStartFailure<T>(this T options, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnStartFailure = action;
            }

            return options;
        }

        /// <summary>
        /// The action that is executed when this instance is successfully completed.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnExecutionSuccess<T>(this T options, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnExecutionSucess = action;
            }

            return options;
        }

        /// <summary>
        /// The action that is executed when this instance execution fails.
        /// </summary>
        /// <param key="action">The action to execute.</param>
        public static T ExecuteOnExecutionFailure<T>(this T options, Action<IBdoHost> action)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnExecutionFailure = action;
            }

            return options;
        }

        /// <summary>
        /// Throws an exception when start fails.
        /// </summary>
        public static T ThrowExceptionOnStartFailure<T>(this T options, bool throwException = false)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnStartFailure = throwException ?
                (_ => throw new BdoHostLoadException("BindOpen options failed while loading"))
                : null;
            }

            return options;
        }

        /// <summary>
        /// Throws an exception when start fails.
        /// </summary>
        public static T ThrowExceptionOnExecutionFailure<T>(this T options, bool throwException = false)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.Action_OnExecutionFailure = throwException ?
                (_ => throw new BdoHostLoadException("BindOpen options failed while loading"))
                : null;
            }

            return options;
        }

        // Depots -------------------------------------------

        /// <summary>
        /// Adds the data store executing the specified action.
        /// </summary>
        /// <param key="action">The action to execute on the created data store.</param>
        public static T AddDepotStore<T>(this T options, Action<IBdoDepotStore> action = null)
            where T : IBdoHostSettings
        {
            if (options != null)
            {
                options.DepotStore ??= BdoData.NewDepotStore();
                action?.Invoke(options.DepotStore);
            }

            return options;
        }
    }
}