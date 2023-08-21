using BindOpen.System.Data;
using BindOpen.System.Data.Meta;
using BindOpen.System.Hosting.Hosts;

namespace BindOpen.System.Tests.Hosting
{
    /// <summary>
    /// This class represents a website application settings.
    /// </summary>
    public class TestSettings : BdoHostSettings
    {
        // -------------------------------------------------------
        // PROPERTIES
        // -------------------------------------------------------

        #region Properties

        /// <summary>
        /// The ReCaptcha public key of this instance.
        /// </summary>
        [BdoProperty("folderPath")]
        public string FolderPath { get; set; }

        /// <summary>
        /// The ReCaptcha secret key of this instance.
        /// </summary>
        [BdoProperty("secretKey")]
        public string SecretKey { get; set; }

        /// <summary>
        /// The News URIs of this instance.
        /// </summary>
        [BdoProperty("uris")]
        public TBdoDictionary<string> NewsUris { get; set; }

        #endregion

        // -------------------------------------------------------------
        // CONSTRUCTORS
        // -------------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Instantiates a new instance of the WebsiteAppSettings class.
        /// </summary>
        public TestSettings() : base()
        {
        }

        #endregion
    }
}