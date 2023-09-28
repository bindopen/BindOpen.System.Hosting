using BindOpen.Kernel.Logging;
using BindOpen.Kernel.Logging.Loggers;
using BindOpen.Kernel.Processing;
using NUnit.Framework;

namespace BindOpen.Kernel.Hosting.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class LoggersTests
    {
        /// <summary>
        /// 
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void DefaultConfigurationTest()
        {
            var bdoHost = BdoHosting.NewHost(
                options => options
                    .SetLogger(q => BdoLogging.NewLogger<BdoTraceLogger>()));
            bdoHost.Start();

            Assert.That(bdoHost.State == ProcessExecutionState.Pending, "Application host not load failed");
        }
    }
}
