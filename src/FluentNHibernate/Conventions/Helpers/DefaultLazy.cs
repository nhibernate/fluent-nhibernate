using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DefaultLazy
    {
        public static IHibernateMappingConvention Always()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultLazy());
        }

        public static IHibernateMappingConvention Never()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.Not.DefaultLazy());
        }
    }
}