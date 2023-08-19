namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a options options.
    /// </summary>
    public static class IBdoHostSettingsExtensions
    {
        /// <summary>
        /// Set the root folder.
        /// </summary>
        /// <param key="predicate">The condition that must be satisfied.</param>
        /// <param key="rootFolderPath">The root folder path.</param>
        /// <returns>Returns the options option.</returns>
        public static T WithLibraryFolder<T>(this T settings, string path)
            where T : IBdoHostSettings
        {
            if (settings != null)
            {
                settings.LibraryFolderPath = path;
            }

            return settings;
        }
    }
}