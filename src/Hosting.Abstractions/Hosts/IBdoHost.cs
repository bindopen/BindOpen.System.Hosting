using BindOpen.Kernel.Logging;
using BindOpen.Kernel.Processing;
using BindOpen.Kernel.Scoping;
using System;

namespace BindOpen.Kernel.Hosting.Hosts
{
    /// <summary>
    /// The interface defines the base bot.
    /// </summary>
    public interface IBdoHost : IBdoScope, IBdoLoggerTracked
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