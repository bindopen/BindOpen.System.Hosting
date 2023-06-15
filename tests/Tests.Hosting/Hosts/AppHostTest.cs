using BindOpen.System.Data;
using BindOpen.System.Data.Meta;
using BindOpen.System.Data.Stores;
using BindOpen.System.Hosting;
using BindOpen.System.Hosting.Hosts;
using BindOpen.System.Processing;
using BindOpen.System.Scoping.Connectors;
using NUnit.Framework;
using System.Linq;

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
                    .AddDataStore(store => store
                        .RegisterDatasources(m => m
                            .AddFromConnectionStrings(GlobalVariables.NetCoreConfiguration))));

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            var datasourceDepot = appHost.GetDatasourceDepot();

            var datasourceA = datasourceDepot?["db.testA"];
            Assert.That(datasourceA?.Name == "db.testA", "Bad data source loading");

            Assert.That(datasourceDepot?.Get("db.testA").FirstOrDefault()
                .GetConnectionString() != null,
                "Bad data source loading");

            Assert.That(datasourceDepot?.Get("db.testA").Get("database.mssqlserver$client").GetConnectionString() != null,
                "Bad data source loading");

            var datasourceB = datasourceDepot?["db.testB"];
            Assert.That(datasourceB?.Name == "db.testB", "Bad data source loading from .NET Core config");

            Assert.That(datasourceDepot.Descendant<IBdoConfiguration>("db.testB", 0).GetConnectionString() != null,
                "Bad data source loading from .NET Core config");

            Assert.That(datasourceDepot.Get()?.Name == "db.testA", "Bad data source loading");
        }
    }
}
