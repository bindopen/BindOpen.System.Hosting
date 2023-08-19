using BindOpen.System.Hosting;
using BindOpen.System.Processing;
using NUnit.Framework;

namespace BindOpen.System.Tests.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class ConfigurationTests
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
            var appHost = BdoHosting.NewHost(
                settings => settings
                    .SetConfiguration(q => q.Kernel.ApplicationInstanceName = "host-test"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            Assert.That(appHost.Settings?.Configuration?.Kernel?.ApplicationInstanceName == "host-test", "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void OneFileTest()
        {
            var appHost = BdoHosting.NewHost(
                settings => settings
                    .SetRootFolder("./")
                    .SetConfiguration<TestConifguration>()
                    .AddConfigurationFile(null, true));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SeveralFilesTest()
        {
            var appHost = BdoHosting.NewHost(
                settings => settings
                    .SetRootFolder("./")
                    .SetConfiguration<TestConifguration>()
                    .AddConfigurationFile("./bdo/bincopen.xml", true)
                    .AddConfigurationFile("./bdo/appconfig.xml", true));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");
        }
    }
}
