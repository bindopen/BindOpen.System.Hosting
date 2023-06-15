using BindOpen.System.Logging;
using BindOpen.System.Processing;
using BindOpen.System.Scoping;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// The interface defines the base bot.
    /// </summary>
    public interface IBdoHost : IBdoScope, IBdoLogged
    {
        ProcessExecutionState State { get; set; }

        IBdoHostOptions Options { get; set; }

        void Start();

        void Stop();
    }
}