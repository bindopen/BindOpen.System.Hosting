using BindOpen.Kernel.Hosting.Settings;
using BindOpen.Kernel.Logging;
using BindOpen.Kernel.Logging.Loggers;
using BindOpen.Kernel.Scoping;
using System;

namespace BindOpen.Kernel.Hosting
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