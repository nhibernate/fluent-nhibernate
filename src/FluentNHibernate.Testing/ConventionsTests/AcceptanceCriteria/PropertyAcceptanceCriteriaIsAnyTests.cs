using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
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
                
            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.Field.ToString()}))
                .ShouldBeTrue();

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.Property.ToString() }))
                .ShouldBeTrue();
        }

        [Test]
        public void IsAnyFailsIfNoValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsAny(Access.Property, Access.Field));

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.CamelCaseField().ToString() }))
                .ShouldBeFalse();
        }

        [Test]
        public void IsNotAnyFailsIfAnyOfTheSuppliedValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsNotAny(Access.Property, Access.Field));

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.Field.ToString() }))
                .ShouldBeFalse();

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.Property.ToString() }))
                .ShouldBeFalse();
        }

        [Test]
        public void IsNotAnySucceedsIfNoValuesMatch()
        {
            acceptance.Expect(x => x.Access.IsNotAny(Access.Property, Access.Field));

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping() { Access = Access.CamelCaseField().ToString() }))
                .ShouldBeTrue();
        }
    }
}
