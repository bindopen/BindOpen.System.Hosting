using BindOpen.System.Logging;
using BindOpen.System.Processing;
using BindOpen.System.Scoping;
using System;

namespace BindOpen.System.Hosting.Hosts
{
    /// <summary>
    /// The interface defines the base bot.
    /// </summary>
    public interface IBdoHost : IBdoScope, IBdoLogTracked
    {
        ProcessExecutionState State { get; }

        IBdoHostOptions Options { get; set; }

        void Start();

        void Stop();

        // Trigger actions --------------------------------------

        event EventHandler OnInitSucceeds;

        event EventHandler OnInitFails;

        event EventHandler OnExecutionSucceeds;

        event EventHandler OnExecutionFails;
    }
}