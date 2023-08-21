using BindOpen.System.Data;
using BindOpen.System.Data.Helpers;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// This class represents a BindOpen host settings.
    /// </summary>
    public class BdoHostKernelSettings : BdoObject, IBdoHostKernelSettings
    {
        // -------------------------------------------------------
        // PROPERTIES
        // -------------------------------------------------------

        #region Properties

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        public string ApplicationInstanceName { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        public string LibraryFolderPath { get; set; } = (@".\" + BdoDefaultHostPaths.__DefaultLibraryFolderPath).ToPath();

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        public string LoggingFolderPath { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        public string LoggingFileName { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        public int LoggingExpirationDayNumber { get; set; }

        #endregion

        // -------------------------------------------------------------
        // CONSTRUCTORS
        // -------------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the BdoHostConfigSection class.
        /// </summary>
        public BdoHostKernelSettings()
            : base()
        {
        }

        #endregion
    }
}
