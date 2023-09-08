using BindOpen.Kernel.Data.Meta;

namespace BindOpen.Kernel.Data.Stores
{
    /// <summary>
    /// This interface defines a data source.
    /// </summary>
    public interface IBdoDatasource : IBdoMetaWrapper, INamed, IReferenced
    {
        public static string __ConnectionString_DatasourceKind = "datasourceKind";
        public static string __ConnectionString_Token = "connectionString";

        /// <summary>
        /// 
        /// </summary>
        DatasourceKind DatasourceKind { get; set; }


        string ConnectionString { get; set; }
    }
}