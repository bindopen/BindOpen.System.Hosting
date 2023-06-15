using NUnit.Framework;

namespace BindOpen.System.Tests.Hosting
{
    /// <summary>
    /// This class set the global settings up.
    /// </summary>
    [SetUpFixture]
    public class GlobalSetUp
    {
        [OneTimeSetUp]
        public void Setup()
        {
            // Setup variables for the first time
            _ = GlobalVariables.AppHost;
        }
    }
}