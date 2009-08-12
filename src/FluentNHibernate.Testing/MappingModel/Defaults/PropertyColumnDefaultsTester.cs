using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class PropertyColumnDefaultsTester
    {
        [Test]
        public void ShouldHaveDefaultColumnIfNoneSpecified()
        {
            var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget)))
                .GetPropertyMapping();

            mapping.Columns.Defaults.Count().ShouldEqual(1);
            mapping.Columns.UserDefined.Count().ShouldEqual(0);
            mapping.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldHaveNoDefaultsIfUserSpecifiedColumn()
        {
            var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget))
                .Column("explicit"))
                .GetPropertyMapping();

            mapping.Columns.Defaults.Count().ShouldEqual(0);
            mapping.Columns.UserDefined.Count().ShouldEqual(1);
            mapping.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void DefaultColumnShouldInheritColumnAttributes()
        {
            var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget))
                .Not.Nullable())
                .GetPropertyMapping();

            mapping.Columns.Defaults.First().NotNull.ShouldBeTrue();
            mapping.Columns.First().NotNull.ShouldBeTrue();
        }

        private PropertyInfo Prop(Expression<Func<PropertyTarget, object>> propertyAccessor)
        {
            return ReflectionHelper.GetProperty(propertyAccessor);
        }
    }
}
