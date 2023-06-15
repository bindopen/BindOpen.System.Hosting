using BindOpen.System.Hosting.Settings;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBdoHostSettings : IBdoSettings
    {
        /// <summary>
        /// 
        /// </summary>
        string LibraryFolderPath { get; set; }
    }
}