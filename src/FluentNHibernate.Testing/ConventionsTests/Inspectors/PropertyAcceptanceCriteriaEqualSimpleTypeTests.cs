using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.Inspectors
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaEqualSimpleTypeTests
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
            acceptance.Expect(x => x.Insert, Is.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Insert = true }))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Insert = false }))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectEqualShouldValidateToFalseIfUnset()
        {
            acceptance.Expect(x => x.Insert, Is.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Not.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Insert = false }))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Not.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping { Insert = true }))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotEqualShouldValidateToTrueIfUnset()
        {
            acceptance.Expect(x => x.Insert, Is.Not.Equal(true));

            acceptance
                .Matches(new PropertyDsl(new PropertyMapping()))
                .ShouldBeTrue();
        }
    }
}