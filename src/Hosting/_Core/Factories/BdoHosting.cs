using BindOpen.System.Data;
using BindOpen.System.Hosting.Hosts;
using BindOpen.System.Logging;
using System;

namespace BindOpen.System.Hosting
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
            IBdoLog log = null)
        {
            BdoHost host = NewHost(null, log);
            return host;
        }

        /// <summary>
        /// Adds a BindOpen host.
        /// </summary>
        /// <param key="setupAction">The setup action to consider.</param>
        /// <returns></returns>
        public static BdoHost NewHost(
            Action<IBdoHostOptions> setupAction,
            IBdoLog log = null)
        {
            var host = BdoData.New<BdoHost>().WithLog(log);
            host.Configure(setupAction);
            host.Start();
            return host;
        }
    }
}