using BindOpen.Kernel.Data.Meta;

namespace BindOpen.Kernel.Hosting.Hosts
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