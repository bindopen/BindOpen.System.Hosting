using BindOpen.System.Data.Meta;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBdoHostConfigWrapper : IBdoMetaWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        string LibraryFolderPath { get; set; }
    }
}