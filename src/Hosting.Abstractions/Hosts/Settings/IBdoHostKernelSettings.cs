using BindOpen.System.Data.Meta;

namespace BindOpen.System.Hosting.Hosts
{
    public interface IBdoHostKernelSettings
    {
        [BdoProperty(Name = "applicationInstanceName")]
        string ApplicationInstanceName { get; set; }

        [BdoProperty(Name = "library.folderPath")]
        string LibraryFolderPath { get; set; }

        [BdoProperty(Name = "/$logging/folderPath")]
        int LoggingExpirationDayNumber { get; set; }

        [BdoProperty(Name = "/$logging/fileName")]
        string LoggingFileName { get; set; }

        [BdoProperty(Name = "/$logging/expirationDayNumber")]
        string LoggingFolderPath { get; set; }
    }
}