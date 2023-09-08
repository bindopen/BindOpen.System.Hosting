namespace BindOpen.Kernel.Data.Stores
{
    /// <summary>
    /// This class represents a data source depot.
    /// </summary>
    public class BdoDatasourceDepot : TBdoDepot<IBdoDatasource>, IBdoDatasourceDepot
    {
        // ------------------------------------------
        // CONSTRUCTORS
        // ------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoDatasourceDepot class.
        /// </summary>
        public BdoDatasourceDepot() : base()
        {
        }

        #endregion
    }
}