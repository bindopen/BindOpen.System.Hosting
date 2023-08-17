using BindOpen.System.Data;
using BindOpen.System.Data.Stores;
using BindOpen.System.Hosting;
using BindOpen.System.Hosting.Hosts;
using BindOpen.System.Processing;
using NUnit.Framework;

namespace BindOpen.System.Tests.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class AppHostTest
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
        public void TestAppHost()
        {
            Assert.That(GlobalVariables.AppHost.State == ProcessExecutionState.Pending, "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestAppHostWithNoOptions()
        {
            var appHost = BdoHosting.NewHost();
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestAppHostWithDatasources()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .AddDepotStore(store => store
                        .RegisterDatasources(m => m
                            .AddFromConnectionStrings(GlobalVariables.NetCoreConfiguration))));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            var datasourceDepot = appHost.GetDatasourceDepot();

            var datasourceA = datasourceDepot?["db.testA"];
            Assert.That(datasourceA?.Name == "db.testA", "Bad data source loading");
            Assert.That(datasourceA?.Kind == DatasourceKind.Database, "Bad data source loading");

            Assert.That(datasourceDepot?.Get("db.testA")?.ConnectionString != null,
                "Bad data source loading");

            var datasourceB = datasourceDepot?["db.testB"];
            Assert.That(datasourceB?.Name == "db.testB", "Bad data source loading from .NET Core config");
        }
    }
}
