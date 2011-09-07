using System;

namespace FluentNHibernate.Diagnostics
{
    /// <summary>
    /// Diagnostic logging configuration
    /// </summary>
    public class DiagnosticsConfiguration
    {
        readonly IDiagnosticMessageDispatcher dispatcher;
        readonly Action<IDiagnosticLogger> setLogger;

        public DiagnosticsConfiguration(IDiagnosticMessageDispatcher dispatcher, Action<IDiagnosticLogger> setLogger)
        {
            this.dispatcher = dispatcher;
            this.setLogger = setLogger;
        }

        /// <summary>
        /// Conditionally enable logging
        /// </summary>
        /// <param name="enable">Enable logging</param>
        public DiagnosticsConfiguration Enable(bool enable)
        {
            if (enable)
                Enable();
            else
                Disable();

            return this;
        }

        /// <summary>
        /// Enable logging
        /// </summary>
        public DiagnosticsConfiguration Enable()
        {
            setLogger(new DefaultDiagnosticLogger(dispatcher));
            return this;
        }

        /// <summary>
        /// Disable logging
        /// </summary>
        public DiagnosticsConfiguration Disable()
        {
            setLogger(new NullDiagnosticsLogger());
            return this;
        }

        /// <summary>
        /// Register a logging listener
        /// </summary>
        /// <param name="listener">Listener</param>
        public DiagnosticsConfiguration RegisterListener(IDiagnosticListener listener)
        {
            dispatcher.RegisterListener(listener);
            return this;
        }

        /// <summary>
        /// Register a default Console.Write listener
        /// </summary>
        /// <returns></returns>
        public DiagnosticsConfiguration OutputToConsole()
        {
            RegisterListener(new ConsoleOutputListener());
            return this;
        }

        /// <summary>
        /// Register a Console.Write listener with a custom result formatter
        /// </summary>
        /// <param name="formatter">Result formatter</param>
        public DiagnosticsConfiguration OutputToConsole(IDiagnosticResultsFormatter formatter)
        {
            RegisterListener(new ConsoleOutputListener(formatter));
            return this;
        }

        /// <summary>
        /// Register a default file output listener
        /// </summary>
        /// <param name="path">Output path</param>
        public DiagnosticsConfiguration OutputToFile(string path)
        {
            RegisterListener(new FileOutputListener(path));
            return this;
        }

        /// <summary>
        /// Register a file output listener with a custom result formatter
        /// </summary>
        /// <param name="formatter">Result formatter</param>
        /// <param name="path">Output path</param>
        public DiagnosticsConfiguration OutputToFile(IDiagnosticResultsFormatter formatter, string path)
        {
            RegisterListener(new FileOutputListener(formatter, path));
            return this;
        }
    }
}