using BindOpen.Data.Meta;

namespace BindOpen.Hosting.Settings
{
    /// <summary>
    /// This class represents a BindOpen host settings.
    /// </summary>
    public class BdoHostSettings : TBdoMetaWrapper<BdoConfiguration>, IBdoSettings
    {        
        // -------------------------------------------------------
        // PROPERTIES
        // -------------------------------------------------------

        #region Properties

        public IBdoHostKernelSettings Kernel { get => KernelSettings; set { KernelSettings = value as BdoHostKernelSettings; } }

        [BdoProperty(Name = "kernel", Reference = "^$kernel/bdo")]
        public BdoHostKernelSettings KernelSettings { get; set; } = new BdoHostKernelSettings();

        #endregion

        // -------------------------------------------------------------
        // CONSTRUCTORS
        // -------------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoHostConfig class.
        /// </summary>
        public BdoHostSettings() : base()
        {
        }

        #endregion
    }
}
