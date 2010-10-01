using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Builders;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults
{
    [TestFixture]
    public class PropertyColumnDefaultsTester
    {
        [Test]
        public void ShouldHaveDefaultColumnIfNoneSpecified()
        {
            var mapping = new PropertyMapping();                

            new PropertyBuilder(mapping, typeof(PropertyTarget), Prop(x => x.Name));

            mapping.Columns.Defaults.Count().ShouldEqual(1);
            mapping.Columns.UserDefined.Count().ShouldEqual(0);
            mapping.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldHaveNoDefaultsIfUserSpecifiedColumn()
        {
            var mapping = new PropertyMapping();

            new PropertyBuilder(mapping, typeof(PropertyTarget), Prop(x => x.Name))
                .Column("explicit");

            mapping.Columns.Defaults.Count().ShouldEqual(0);
            mapping.Columns.UserDefined.Count().ShouldEqual(1);
            mapping.Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void DefaultColumnShouldInheritColumnAttributes()
        {
            var mapping = new PropertyMapping();

            new PropertyBuilder(mapping, typeof(PropertyTarget), Prop(x => x.Name))
                .Not.Nullable();

            mapping.Columns.Defaults.First().NotNull.ShouldBeTrue();
            mapping.Columns.First().NotNull.ShouldBeTrue();
        }

        private Member Prop(Expression<Func<PropertyTarget, object>> propertyAccessor)
        {
            return ReflectionHelper.GetMember(propertyAccessor);
        }
    }
}
