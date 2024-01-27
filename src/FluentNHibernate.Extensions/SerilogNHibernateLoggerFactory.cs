using NHibernate;
using Serilog;
using Serilog.Core;

namespace FluentNHibernate;

/// <summary>
/// Factory to create SerilogNHibernateLogger
/// </summary>
public class SerilogNHibernateLoggerFactory : INHibernateLoggerFactory
{
    public ILogger Logger { get; protected set; }

    public SerilogNHibernateLoggerFactory(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        Logger = logger;
    }

    public SerilogNHibernateLoggerFactory()
        : this(Log.Logger) { }

    public INHibernateLogger LoggerFor(string keyName)
    {
        ILogger contextLogger = Logger.ForContext(Constants.SourceContextPropertyName, keyName);
        return new SerilogNHibernateLogger(contextLogger);
    }

    public INHibernateLogger LoggerFor(Type type)
    {
        ILogger contextLogger = Logger.ForContext(type);
        return new SerilogNHibernateLogger(contextLogger);
    }
}
