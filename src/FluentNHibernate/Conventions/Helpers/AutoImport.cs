using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class AutoImport
    {
        public static IHibernateMappingConvention Always()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.AutoImport());
        }

        public static IHibernateMappingConvention Never()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.Not.AutoImport());
        }
    }
}