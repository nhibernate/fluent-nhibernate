using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.Inspectors
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
            acceptance.Expect(x => x.Access, Is.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Access = "field" }))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access, Is.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Access = "property" }))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfUnset()
        {
            acceptance.Expect(x => x.Access, Is.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access, Is.Not.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Access = "property" }))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Access, Is.Not.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Access = "field" }))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfUnset()
        {
            acceptance.Expect(x => x.Access, Is.Not.Equal(Access.AsField()));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping()))
                .ShouldBeTrue();
        }
    }
}