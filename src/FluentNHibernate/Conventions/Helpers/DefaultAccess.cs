using System;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.Properties;
using Prefix=FluentNHibernate.Mapping.Prefix;

namespace FluentNHibernate.Conventions.Helpers
{
    public static class DefaultAccess
    {
        public static IHibernateMappingConvention Field()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.Field());
        }

        public static IHibernateMappingConvention BackField()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.BackField());
        }

        public static IHibernateMappingConvention Property()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.Property());
        }

        public static IHibernateMappingConvention ReadOnlyProperty()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyProperty());
        }

        public static IHibernateMappingConvention NoOp()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.NoOp());
        }

        public static IHibernateMappingConvention None()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.None());
        }

        public static IHibernateMappingConvention CamelCaseField()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.CamelCaseField());
        }

        public static IHibernateMappingConvention CamelCaseField(CamelCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.CamelCaseField(prefix));
        }

        public static IHibernateMappingConvention LowerCaseField()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.LowerCaseField());
        }

        public static IHibernateMappingConvention LowerCaseField(LowerCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.LowerCaseField(prefix));
        }

        public static IHibernateMappingConvention PascalCaseField(PascalCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.PascalCaseField(prefix));
        }

        public static IHibernateMappingConvention ReadOnlyPropertyThroughCamelCaseField()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyPropertyThroughCamelCaseField());
        }

        public static IHibernateMappingConvention ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyPropertyThroughCamelCaseField(prefix));
        }

        public static IHibernateMappingConvention ReadOnlyPropertyThroughLowerCaseField()
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyPropertyThroughLowerCaseField());
        }

        public static IHibernateMappingConvention ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyPropertyThroughLowerCaseField(prefix));
        }

        public static IHibernateMappingConvention ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.ReadOnlyPropertyThroughPascalCaseField(prefix));
        }

        public static IHibernateMappingConvention Using(string value)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.Using(value));
        }

        public static IHibernateMappingConvention Using(Type access)
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.Using(access));
        }

        public static IHibernateMappingConvention Using<T>() where T : IPropertyAccessor
        {
            return new BuiltHibernateMappingConvention(
                criteria => { },
                instance => instance.DefaultAccess.Using<T>());
        }
    }
}