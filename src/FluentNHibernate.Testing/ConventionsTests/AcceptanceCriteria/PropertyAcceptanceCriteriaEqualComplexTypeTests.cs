using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaEqualComplexTypeTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ExpectEqualShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access == Access.Field);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, "field");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access == Access.Field);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, "property");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfUnset()
        {
            acceptance.Expect(x => x.Access == Access.Field);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access != Access.Field);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, "property");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access != Access.Field);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Access, Layer.Defaults, "field");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfUnset()
        {
            acceptance.Expect(x => x.Access != Access.Field);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeTrue();
        }
    }
}