using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DefaultLazy
    {
        public static IClassConvention AlwaysTrue()
        {
            return new BuiltClassConvention(
                map => true,
                map => map.HibernateMapping.DefaultLazy());
        }

        public static IClassConvention AlwaysFalse()
        {
            return new BuiltClassConvention(
                map => true,
                map => map.HibernateMapping.Not.DefaultLazy());
        }
    }
}
