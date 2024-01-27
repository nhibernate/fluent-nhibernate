using System;
using System.Collections.Generic;
using NHibernate;
using Serilog;
using Serilog.Events;

namespace FluentNHibernate;

/// <summary>
/// Serilog logger for NHibernate
/// </summary>
public class SerilogNHibernateLogger : INHibernateLogger
{
    private readonly ILogger _contextLogger;

    public SerilogNHibernateLogger(ILogger serilogLogger)
    {
        _contextLogger = serilogLogger ?? throw new ArgumentNullException(nameof(serilogLogger));
    }

    /// <summary>
    /// Mapping between NHibernate log levels and Serilog
    /// In Serilog, non does not exists
    /// </summary>
    private static readonly Dictionary<NHibernateLogLevel, LogEventLevel> MapLevels =
        new Dictionary<NHibernateLogLevel, LogEventLevel>
        {
            { NHibernateLogLevel.Trace, LogEventLevel.Verbose },
            { NHibernateLogLevel.Info, LogEventLevel.Information },
            { NHibernateLogLevel.Debug, LogEventLevel.Debug },
            { NHibernateLogLevel.Warn, LogEventLevel.Warning },
            { NHibernateLogLevel.Error, LogEventLevel.Error },
            { NHibernateLogLevel.Fatal, LogEventLevel.Fatal }
        };

    /// <summary>
    /// Is the logging level enabled?
    /// </summary>
    /// <param name="logLevel">NHibernate log level</param>
    /// <returns>true/false</returns>
    public bool IsEnabled(NHibernateLogLevel logLevel)
    {
        // special case because for Serilog there's no none level
        return logLevel != NHibernateLogLevel.None || _contextLogger.IsEnabled(MapLevels[logLevel]);
    }

    /// <summary>
    /// Log a new entry in serilog
    /// </summary>
    /// <param name="logLevel">NHibernate log level</param>
    /// <param name="state">Log state</param>
    /// <param name="exception">Exception if any</param>
    public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
    {
        _contextLogger.Write(MapLevels[logLevel], exception, state.Format, state.Args);
    }
}
