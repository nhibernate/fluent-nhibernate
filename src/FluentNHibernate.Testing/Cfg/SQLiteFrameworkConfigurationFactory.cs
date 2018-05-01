using FluentNHibernate.Cfg.Db;
using System.Data;

namespace FluentNHibernate.Testing.Cfg
{
    public static class SQLiteFrameworkConfigurationFactory
    {
#if NETFX
        internal static SQLiteConfiguration CreateStandardInMemoryConfiguration()
        {
            return SQLiteConfiguration.Standard.InMemory();
        }
#endif

#if NETCORE

        internal static MsSqliteConfiguration CreateStandardInMemoryConfiguration()
        {
            return MsSqliteConfiguration.Standard.InMemory();
        }

#endif

#if NETFX
        internal static SQLiteConfiguration CreateStandardConfiguration()
        {
            return SQLiteConfiguration.Standard;
        }
#endif

#if NETCORE

        internal static MsSqliteConfiguration CreateStandardConfiguration()
        {
            return MsSqliteConfiguration.Standard;
        }

#endif

#if NETFX
        internal static SQLiteConfiguration CreateStandardConfiguration(IsolationLevel isolationLevel)
        {
            return SQLiteConfiguration.Standard.IsolationLevel(isolationLevel);
        }
#endif

#if NETCORE

        internal static MsSqliteConfiguration CreateStandardConfiguration(IsolationLevel isolationLevel)
        {
            return MsSqliteConfiguration.Standard.IsolationLevel(isolationLevel);
        }

#endif
    }
}