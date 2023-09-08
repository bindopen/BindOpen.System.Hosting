using BindOpen.Kernel.Data.Helpers;
using BindOpen.Kernel.Processing;
using NUnit.Framework;
using System.IO;

namespace BindOpen.Kernel.Hosting.Settings
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class PathTests
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
        public void RootFolderTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder("rootFolder")
                    .AddRootFolder(s => false, "rootFolderA")
                    .AddRootFolder(s => true, "rootFolderB")
                    .WithLibraryFolder(@".\bdo\lib"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            var rootFolderPath = appHost.GetKnownPath(BdoHostPathKind.RootFolder);

            Assert.That(rootFolderPath == @"rootFolderB\", "Bad library folder path");

            appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder("rootFolder")
                    .AddRootFolder(s => false, "rootFolderA")
                    .WithLibraryFolder(@".\bdo\lib"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            rootFolderPath = appHost.GetKnownPath(BdoHostPathKind.RootFolder);

            Assert.That(rootFolderPath == @"rootFolder\", "Bad library folder path");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void DefaultRootFolderTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder(@".\default")
                    .AddRootFolder(s => false, "rootFolderA")
                    .WithLibraryFolder(@".\bdo\lib"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            var rootFolderPath = appHost.GetKnownPath(BdoHostPathKind.RootFolder);

            var path = Path.Combine(FileHelper.GetAppRootFolderPath(), @"default".ToPath()).EndingWith(@"\");

            Assert.That(rootFolderPath == path, "Bad library folder path");

            // No root folder set initially

            appHost = BdoHosting.NewHost(
                options => options
                    .AddRootFolder(s => false, "rootFolderA")
                    .WithLibraryFolder(@".\bdo\lib"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            rootFolderPath = appHost.GetKnownPath(BdoHostPathKind.RootFolder);

            path = FileHelper.GetAppRootFolderPath().EndingWith(@"\");

            Assert.That(rootFolderPath == path, "Bad library folder path");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void LibraryFolderTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .SetRootFolder("./")
                    .WithLibraryFolder(@".\bdo\lib"));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Bad library folder path");

            var libraryPath = appHost.GetKnownPath(BdoHostPathKind.LibraryFolder);

            var path = Path.Combine(appHost.GetKnownPath(BdoHostPathKind.RootFolder), @"bdo\lib".ToPath()).EndingWith(@"\");

            Assert.That(libraryPath == path, "Bad library folder path");
        }
    }
}
