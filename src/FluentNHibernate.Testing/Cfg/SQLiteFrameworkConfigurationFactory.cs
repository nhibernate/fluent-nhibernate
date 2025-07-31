using FluentNHibernate.Cfg.Db;
using System.Data;

namespace FluentNHibernate.Testing.Cfg;

public static class SQLiteFrameworkConfigurationFactory
{
#if NETFRAMEWORK
    internal static SQLiteConfiguration CreateStandardInMemoryConfiguration()
    {
        return SQLiteConfiguration.Standard.InMemory();
    }
#else
        internal static MsSqliteConfiguration CreateStandardInMemoryConfiguration()
        {
            return MsSqliteConfiguration.Standard.InMemory();
        }
#endif

#if NETFRAMEWORK
    internal static SQLiteConfiguration CreateStandardConfiguration()
    {
        return SQLiteConfiguration.Standard;
    }
#else
        internal static MsSqliteConfiguration CreateStandardConfiguration()
        {
            return MsSqliteConfiguration.Standard;
        }
#endif

#if NETFRAMEWORK
    internal static SQLiteConfiguration CreateStandardConfiguration(IsolationLevel isolationLevel)
    {
        return SQLiteConfiguration.Standard.IsolationLevel(isolationLevel);
    }
#else
        internal static MsSqliteConfiguration CreateStandardConfiguration(IsolationLevel isolationLevel)
        {
            return MsSqliteConfiguration.Standard.IsolationLevel(isolationLevel);
        }
#endif

}
