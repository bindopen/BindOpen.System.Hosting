using BindOpen.System.Scoping;
using Microsoft.Extensions.Configuration;

namespace BindOpen.System.Data.Stores
{
    /// <summary>
    /// This class represents an data source extensions.
    /// </summary>
    public static class BdoDatasourceExtensions
    {
        /// <summary>
        /// Adds sources from connection strings.
        /// </summary>
        /// <param key="depot">The datasource depot to consider.</param>
        /// <param key="config">The config to consider.</param>
        /// <param key="keyName">The key name to consider.</param>
        public static IBdoSourceDepot AddFromConnectionStrings(
            this IBdoSourceDepot depot,
            IConfiguration config,
            string keyName = "connectionStrings")
        {
            if (depot != null && config != null)
            {
                var sections = config.GetSection(keyName).GetChildren();
                foreach (var section in sections)
                {
                    depot.Add(
                        BdoData.NewDatasource(section.Key, DatasourceKind.Database)
                            .With(
                                BdoData.NewMetaObject()
                                    .WithConnectionString(section.Value)));
                }
            };

            return depot;
        }
    }

}