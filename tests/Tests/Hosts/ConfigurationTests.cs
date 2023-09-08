using BindOpen.Kernel.Hosting.Tests;
using BindOpen.Kernel.Processing;
using NUnit.Framework;

namespace BindOpen.Kernel.Hosting
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
                options => options
                    .SetSettings(q => q.Kernel.ApplicationInstanceName = "host-test"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            Assert.That(appHost.Options?.Settings?.Kernel?.ApplicationInstanceName == "host-test", "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CastingConfigTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder(@".\")
                    .SetSettings<TestSettings>());
            appHost.Start();

            Assert.That(appHost.Options.Settings is TestSettings, "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void KernelConfigFileTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder(@".\")
                    .SetSettings<TestSettings>()
                    .AddConfigurationFile());
            appHost.Start();

            var settings = appHost.Options.GetSettings<TestSettings>();

            Assert.That(settings.Kernel.ApplicationInstanceName == "host-test", "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SeveralFilesTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder(@".\")
                    .SetSettings<TestSettings>()
                    .AddConfigurationFile(@".\bdo\config\appconfig.xml", true)
                    .AddConfigurationFile(@".\bdo\config\appconfig2.xml", true));
            appHost.Start();

            var settings = appHost.Options.GetSettings<TestSettings>();

            Assert.That(settings.FolderPath == "_folderPath", "Application host not load failed");
            Assert.That(settings.NewsUris.Count == 2, "Application host not load failed");
        }
    }
}
