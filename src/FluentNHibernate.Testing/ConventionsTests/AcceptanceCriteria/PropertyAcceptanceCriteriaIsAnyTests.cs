using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaIsAnyTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void IsAnyMatchesAnyOfTheSuppliedValues()
        {
            acceptance.Expect(x => x.Access.IsAny(Access.Property, Access.Field));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, Access.Field.ToString());

            var propertyMapping2 = new PropertyMapping();
            propertyMapping2.Set(x => x.Access, Layer.Defaults, Access.Property.ToString());

            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();

            acceptance
                .Matches(new PropertyInspector(propertyMapping2))
                .ShouldBeTrue();
        }

        [Test]
        public void IsAnyFailsIfNoValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsAny(Access.Property, Access.Field));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, Access.CamelCaseField().ToString());
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void IsNotAnyFailsIfAnyOfTheSuppliedValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsNotAny(Access.Property, Access.Field));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, Access.Field.ToString());

            var propertyMapping2 = new PropertyMapping();
            propertyMapping2.Set(x => x.Access, Layer.Defaults, Access.Property.ToString());

            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();

            acceptance
                .Matches(new PropertyInspector(propertyMapping2))
                .ShouldBeFalse();
        }

        [Test]
        public void IsNotAnySucceedsIfNoValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsNotAny(Access.Property, Access.Field));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, Access.CamelCaseField().ToString());
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }
    }
}
