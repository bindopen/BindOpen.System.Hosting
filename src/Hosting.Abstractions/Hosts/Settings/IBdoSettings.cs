using BindOpen.System.Data.Meta;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBdoSettings : IBdoMetaWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        IBdoHostKernelSettings Kernel { get; set; }
    }
}