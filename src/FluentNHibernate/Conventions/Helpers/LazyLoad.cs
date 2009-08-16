using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class LazyLoad
    {
        public static IClassConvention Always()
        {
            return new BuiltClassConvention(
                criteria => {},
                map => map.LazyLoad());
        }

        public static IClassConvention Never()
        {
            return new BuiltClassConvention(
                criteria => { },
                map => map.Not.LazyLoad());
        }
    }
}
