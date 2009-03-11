namespace FluentNHibernate.Conventions.Helpers
{
    public static class DynamicUpdate
    {
        public static IClassConvention AlwaysTrue()
        {
            return new BuiltClassConvention(
                map => !map.Attributes.Has("dynamic-update"),
                map => map.DynamicUpdate());
        }

        public static IClassConvention AlwaysFalse()
        {
            return new BuiltClassConvention(
                map => !map.Attributes.Has("dynamic-update"),
                map => map.Not.DynamicUpdate());
        }
    }
}