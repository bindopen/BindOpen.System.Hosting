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
        [BdoProperty(Name = "$kernel", Reference = "/$kernel")]
        IBdoHostKernelSettings Kernel { get; set; }
    }
}