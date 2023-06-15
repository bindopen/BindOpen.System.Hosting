using BindOpen.System.Data.Helpers;
using System;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a host.
    /// </summary>
    public static class IBdoHostExtensions
    {
        /// <summary>
        /// Sets the specfied options
        /// </summary>
        /// <param key="options"></param>
        /// <returns>Returns this instance.</returns>
        public static T WithOptions<T>(this T host, IBdoHostOptions options)
            where T : IBdoHost
        {
            if (host != null)
            {
                host.Options = options;
            }

            return host;
        }

        /// <summary>
        /// Runs the specified action.
        /// </summary>
        /// <param key="action">The action to consider.</param>
        /// <returns>Returns this instance.</returns>
        public static T Run<T>(this T host, Action<IBdoHost> action)
            where T : IBdoHost
        {
            if (host != null)
            {
                action?.Invoke(host);
            }

            return host;
        }

        /// <summary>
        /// Returns the path of the application temporary folder.
        /// </summary>
        /// <param key="pathKind">The kind of paths.</param>
        /// <returns>The path of the application temporary folder.</returns>
        public static string GetKnownPath<T>(this T host, BdoHostPathKind pathKind)
            where T : IBdoHost
        {
            string path = null;

            if (host != null)
            {
                switch (pathKind)
                {
                    case BdoHostPathKind.RootFolder:
                        path = host.Options?.RootFolderPath;
                        break;
                    case BdoHostPathKind.LibraryFolder:
                        path = host.Options?.Settings?.LibraryFolderPath;
                        if (string.IsNullOrEmpty(path))
                        {
                            path = host.Options?.Settings?.LibraryFolderPath;
                        }
                        if (string.IsNullOrEmpty(path))
                        {
                            path = host.GetKnownPath(BdoHostPathKind.RootFolder) + BdoDefaultHostPaths.__DefaultLibraryFolderPath;
                        }
                        break;
                    case BdoHostPathKind.HostConfigFile:
                        path = host.Options?.SettingsFilePath;
                        if (string.IsNullOrEmpty(path))
                        {
                            path = host.GetKnownPath(BdoHostPathKind.RootFolder) + BdoDefaultHostPaths.__DefaultHostConfigFileName;
                        }
                        break;
                }
            }

            return (string.IsNullOrEmpty(path) ? null : path).ToPath();
        }
    }
}