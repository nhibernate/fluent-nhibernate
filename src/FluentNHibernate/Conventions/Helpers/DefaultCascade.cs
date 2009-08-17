using FluentNHibernate.Conventions.Helpers.Prebuilt;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DefaultCascade
    {
        public static IHibernateMappingConvention All()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultCascade.All());
        }

        public static IHibernateMappingConvention None()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultCascade.None());
        }

        public static IHibernateMappingConvention SaveUpdate()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultCascade.SaveUpdate());
        }

        public static IHibernateMappingConvention Delete()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultCascade.Delete());
        }
    }
}