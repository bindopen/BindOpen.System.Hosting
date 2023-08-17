using BindOpen.System.Data.Meta;

namespace BindOpen.System.Data.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBdoDatasource : IBdoMetaWrap, INamed, IReferenced
    {
        /// <summary>
        /// 
        /// </summary>
        DatasourceKind Kind { get; set; }


        string ConnectionString { get; set; }
    }
}