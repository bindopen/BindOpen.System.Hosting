using BindOpen.System.Data;
using BindOpen.System.Data.Meta;
using BindOpen.System.Data.Stores;
using BindOpen.System.Hosting;
using BindOpen.System.Processing;
using NUnit.Framework;

namespace BindOpen.System.Tests.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class DatasourcesTests
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
        public void CreateDatasourceTest()
        {
            var detail = BdoData.NewMetaSet()
                .With(
                    (IBdoDatasource.__ConnectionString_Token, "test_connectionString"),
                    (IBdoDatasource.__ConnectionString_DatasourceKind, DatasourceKind.EmailServer));

            var datasource = GlobalVariables.AppHost.NewMetaWrapper<BdoDatasource>(detail);

            Assert.That(datasource.ConnectionString == "test_connectionString", "Bad data source loading");
            Assert.That(datasource.DatasourceKind == DatasourceKind.EmailServer, "Bad data source loading");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void AddFromConnectionStringsTest()
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
            Assert.That(datasourceA?.DatasourceKind == DatasourceKind.Database, "Bad data source loading");

            Assert.That(datasourceDepot?.Get("db.testA")?.ConnectionString != null,
                "Bad data source loading");

            var datasourceB = datasourceDepot?["db.testB"];
            Assert.That(datasourceB?.Name == "db.testB", "Bad data source loading from .NET Core config");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void AddFromSettingsTest()
        {
            var appHost = BdoHosting.NewHost(
                options => options
                    .AddConfigurationFile(@".\bdo\config\appconfig.xml")
                    .AddDepotStore(store => store
                        .RegisterDatasources(m => m
                            .AddFromSettings(options.Settings))));
            appHost.Start();

            Assert.That(appHost.State == ProcessExecutionState.Pending, "Application host not load failed");

            var datasourceDepot = appHost.GetDatasourceDepot();

            var datasourceA = datasourceDepot?["db.testA"];
            Assert.That(datasourceA?.Name == "db.testA", "Bad data source loading");
            Assert.That(datasourceA?.DatasourceKind == DatasourceKind.EmailServer, "Bad data source loading");

            Assert.That(datasourceDepot?.Get("db.testA")?.ConnectionString != null,
                "Bad data source loading");

            var datasourceB = datasourceDepot?["db.testB"];
            Assert.That(datasourceB?.Name == "db.testB", "Bad data source loading from .NET Core config");
        }
    }
}
