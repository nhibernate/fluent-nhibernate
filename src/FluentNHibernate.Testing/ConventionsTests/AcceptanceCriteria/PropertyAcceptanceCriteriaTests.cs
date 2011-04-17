using System.Linq;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Is = FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ShouldAllowExpectOnProperty()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
        }

        [Test]
        public void ExpectOnPropertyShouldRegister()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance.Expectations.Count().ShouldEqual(1);
        }

        [Test]
        public void ExpectOnPropertyShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectOnPropertyShouldValidateToFalseIfNotGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectsShouldValidateToTrueIfGivenMatchingModel()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            propertyMapping.Set(x => x.Update, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void MultipleExpectsShouldValidateToFalseIfOnlyOneMatches()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeFalse();
        }

        [Test]
        public void MultipleExpectsShouldValidateToFalseIfNoneMatch()
        {
            acceptance.Expect(x => x.Insert, Is.Set);
            acceptance.Expect(x => x.Update, Is.Set);

            acceptance
                .Matches(new PropertyInspector(new PropertyMapping()))
                .ShouldBeFalse();
        }
    }
}