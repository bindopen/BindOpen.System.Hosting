using BindOpen.System.Data.Meta;
using BindOpen.System.Scoping;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a BindOpen host settings.
    /// </summary>
    public class BdoHostConfigWrapper : BdoMetaWrapper, IBdoHostConfigWrapper
    {
        // -------------------------------------------------------
        // PROPERTIES
        // -------------------------------------------------------

        #region Properties

        [BdoProperty(Name = "$kernel")]

        public IBdoHostKernelSection Kernel { get; set; } = new BdoHostKernelSection();

        #endregion

        // -------------------------------------------------------------
        // CONSTRUCTORS
        // -------------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoHostConfig class.
        /// </summary>
        public BdoHostConfigWrapper()
            : base()
        {
        }

        /// <summary>
        /// Instantiates a new instance of the BdoHostConfig class.
        /// </summary>
        /// <param key="scope">The scope to consider.</param>
        /// <param key="config">The config to consider.</param>
        public BdoHostConfigWrapper(
            IBdoScope scope,
            IBdoConfiguration config)
            : base(scope, config)
        {
        }

        #endregion
    }
}
