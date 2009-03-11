using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DynamicInsert
    {
        public static IClassConvention AlwaysTrue()
        {
            return new BuiltClassConvention(
                map => !map.Attributes.Has("dynamic-insert"),
                map => map.DynamicInsert());
        }

        public static IClassConvention AlwaysFalse()
        {
            return new BuiltClassConvention(
                map => !map.Attributes.Has("dynamic-insert"),
                map => map.Not.DynamicInsert());
        }
    }
}