using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaIsSetTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ExpectSetShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectSetShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectSetsShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance
                .Expect(x => x.Insert, Is.Set)
                .Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            propertyMapping.Set(x => x.Update, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void MultipleExpectSetsShouldValidateToFalseIfOnlyOneMatches()
        {
            acceptance
                .Expect(x => x.Insert, Is.Set)
                .Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectSetsShouldValidateToFalseIfNoneMatch()
        {
            acceptance
                .Expect(x => x.Insert, Is.Set)
                .Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void ExpectNotSetShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Not.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectNotSetShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Not.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectNotSetsShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Not.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeTrue();
        }

        [Test]
        public void MultipleExpectNotSetsShouldValidateToFalseIfOnlyOneMatches()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Not.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectNotSetsShouldValidateToFalseIfNoneMatch()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Not.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            propertyMapping.Set(x => x.Update, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void CombinationOfSetAndNotSetShouldValidateToTrueWhenGivenMatchingModel()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Update, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void CombinationOfSetAndNotSetShouldValidateToFalseWhenOnlyOneMatches()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void CombinationOfSetAndNotSetShouldValidateToFalseWhenNoneMatch()
        {
            acceptance
                .Expect(x => x.Insert, Is.Not.Set)
                .Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }
    }
}