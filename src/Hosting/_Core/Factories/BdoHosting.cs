using BindOpen.Kernel.Data;
using BindOpen.Kernel.Hosting.Settings;
using System;

namespace BindOpen.Kernel.Hosting
{
    /// <summary>
    /// This static class is a factory for BindOpen hosts.
    /// </summary>
    public static class BdoHosting
    {
        /// <summary>
        /// Adds a BindOpen host.
        /// </summary>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns></returns>
        public static BdoHost NewHost(
            Action<IBdoHostOptions> setupAction = null)
        {
            var host = BdoData.New<BdoHost>();
            host.Configure(setupAction);

            return host;
        }
    }
}