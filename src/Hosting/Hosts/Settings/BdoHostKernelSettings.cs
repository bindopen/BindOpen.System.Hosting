using BindOpen.System.Data;
using BindOpen.System.Data.Helpers;
using BindOpen.System.Data.Meta;

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
        [BdoProperty(Name = "applicationInstanceName")]
        public string ApplicationInstanceName { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        [BdoProperty(Name = "library.folderPath")]
        public string LibraryFolderPath { get; set; } = (@".\" + BdoDefaultHostPaths.__DefaultLibraryFolderPath).ToPath();

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        [BdoProperty(Name = "/$logging/folderPath")]
        public string LoggingFolderPath { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        [BdoProperty(Name = "/$logging/fileName")]
        public string LoggingFileName { get; set; }

        /// <summary>
        /// The library folder path of this instance.
        /// </summary>
        [BdoProperty(Name = "/$logging/expirationDayNumber")]
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
