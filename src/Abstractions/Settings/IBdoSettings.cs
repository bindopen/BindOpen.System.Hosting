using BindOpen.Kernel.Data.Meta;

namespace BindOpen.Kernel.Hosting.Settings
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