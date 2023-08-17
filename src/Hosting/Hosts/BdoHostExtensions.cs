using BindOpen.System.Data;
using System;

namespace BindOpen.System.Hosting.Hosts
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
        public static T Configure<T>(this T host, Action<IBdoHostSettings> setupOptions)
            where T : IBdoHost
        {
            if (host != null)
            {
                host.Settings ??= BdoData.New<BdoHostSettings>();
                setupOptions?.Invoke(host.Settings);
            }

            return host;
        }
    }
}