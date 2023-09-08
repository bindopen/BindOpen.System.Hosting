using BindOpen.Kernel.Data.Helpers;
using BindOpen.Kernel.Hosting.Settings;
using Microsoft.Extensions.Configuration;
using System;

namespace BindOpen.Kernel.Hosting.Tests
{
    public static class GlobalVariables
    {
        static string _workingFolder = null;
        static IBdoHost _appHost = null;
        static IConfiguration _netCoreConfiguration;

        public static string WorkingFolder
        {
            get
            {
                string workingFolder = GlobalVariables._workingFolder;
                if (workingFolder == null)
                    GlobalVariables._workingFolder = workingFolder =
                        ((_appHost?.GetKnownPath(BdoHostPathKind.RootFolder) ?? AppDomain.CurrentDomain.BaseDirectory)
                        .EndingWith(@"\") + @"bdo\temp\").ToPath();

                return workingFolder;
            }
        }

        public static IBdoHost AppHost
        {
            get
            {
                _appHost ??= BdoHosting.NewHost(
                    options => options
                        .ThrowExceptionOnInitFailure());

                _appHost?.Start();

                return _appHost;
            }
        }

        public static IConfiguration NetCoreConfiguration
        {
            get
            {
                if (_netCoreConfiguration != null)
                {
                    return _netCoreConfiguration;
                }

                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppHost?.GetKnownPath(BdoHostPathKind.RootFolder))
                    .AddJsonFile(@"bdo\config\appsettings.json".ToPath(), optional: true, reloadOnChange: true);

                return _netCoreConfiguration = builder.Build();
            }
        }
    }

}
