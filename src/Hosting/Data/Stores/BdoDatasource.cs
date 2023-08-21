using BindOpen.System.Data.Meta;

namespace BindOpen.System.Data.Stores
{
    /// <summary>
    /// This class represents a database data field.
    /// </summary>
    public class BdoDatasource : BdoBaseMetaWrapper, IBdoDatasource
    {
        // ------------------------------------------
        // PROPERTIES
        // ------------------------------------------

        #region Properties

        public string Id { get => Detail?.Id; set { (Detail ??= BdoData.NewMetaSet()).WithId(value); } }

        public string Name { get => Detail?.Name; set { (Detail ??= BdoData.NewMetaSet()).WithName(value); } }

        [BdoProperty("datasourceKind")]
        public DatasourceKind DatasourceKind { get; set; }

        [BdoProperty("connectionString")]
        public string ConnectionString { get; set; }

        #endregion

        // ------------------------------------------
        // CONSTRUCTORS
        // ------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoDatasource class.
        /// </summary>
        public BdoDatasource() : base()
        {
        }

        #endregion
    }
}
