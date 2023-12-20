using BindOpen.Data;
using BindOpen.Hosting.Settings;
using System;

namespace BindOpen.Hosting
{
    /// <summary>
    /// This class represents a host.
    /// </summary>
    public static class BdoHostExtensions
    {
        /// <summary>
        /// Configures the application host.
        /// </summary>
        /// <param key="setupOptions">The action to setup the application host.</param>
        /// <returns>Returns the application host.</returns>
        public static T Configure<T>(this T host, Action<IBdoHostOptions> setupOptions)
            where T : IBdoHost
        {
            if (host != null)
            {
                host.Options ??= BdoData.New<BdoHostOptions>();
                setupOptions?.Invoke(host.Options);
            }

            return host;
        }
    }
}