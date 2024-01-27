using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using NHibernate;
using Serilog;

namespace FluentNHibernate;

public static partial class FluentConfigurationExtensions
{
    /// <summary>
    /// Configure NHibernateLogger.
    /// <see cref="NHibernate.NHibernateLogger.SetLoggersFactory(INHibernateLoggerFactory)"/>
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="loggerFactory"></param>
    /// <returns></returns>
    public static FluentConfiguration UsingLoggerFactory(
        this FluentConfiguration cfg,
        INHibernateLoggerFactory loggerFactory
    )
    {
        NHibernateLogger.SetLoggersFactory(loggerFactory);
        return cfg;
    }

    /// <summary>
    /// Configure Serilog For NHibernateLogger.
    /// <see cref="NHibernate.NHibernateLogger.SetLoggersFactory(INHibernateLoggerFactory)"/>
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static FluentConfiguration UsingSerilog(this FluentConfiguration cfg, ILogger logger)
    {
        NHibernateLogger.SetLoggersFactory(new SerilogNHibernateLoggerFactory(logger));
        return cfg;
    }

    /// <summary>
    /// Configure The globally-shared Serilog For NHibernateLogger.
    /// <see cref="Serilog.Log.Logger"/>
    /// </summary>
    /// <param name="cfg"></param>
    /// <returns></returns>
    public static FluentConfiguration UsingSerilog(this FluentConfiguration cfg)
    {
        return UsingSerilog(cfg, Log.Logger);
    }
}
