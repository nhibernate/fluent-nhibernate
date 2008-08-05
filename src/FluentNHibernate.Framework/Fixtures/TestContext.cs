using ShadeTree.Core;

namespace FluentNHibernate.Framework.Fixtures
{
    public static class TestContext
    {
        private static Cache<string, object> _aliases = new Cache<string, object>();

        public static void StoreAlias(string key, object value)
        {
            _aliases.Store(key, value);
        }

        public static T GetValue<T>(string key)
        {
            return (T)_aliases.Get(key);
        }
    }
}