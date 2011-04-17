using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaAnyTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void AnyShouldValidateToTrueIfOneMatches()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Name, Layer.Defaults, "Property1");
            acceptance
               .Matches(new PropertyInspector(propertyMapping))
               .ShouldBeTrue();
        }

        [Test]
        public void AnyShouldValidateToTrueIfAllMatch()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Name, Layer.Defaults, "Property1");
            propertyMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(typeof(string)));
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            acceptance
               .Matches(new PropertyInspector(propertyMapping))
               .ShouldBeTrue();
        }

        [Test]
        public void AnyShouldValidateToFalseIfNoneMatch()
        {
            acceptance
                .Any(c => c.Expect(x => x.Name, Is.Set), 
                     c => c.Expect(x => x.Type, Is.Set), 
                     c => c.Expect(x => x.Insert, Is.Set));

            acceptance
               .Matches(new PropertyInspector(new PropertyMapping()))
               .ShouldBeFalse();
        }

        [Test]
        public void CanPassSubCriteriaToAny()
        {
            var subCriteria1 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria1.Expect(x => x.Name, Is.Set);

            var subCriteria2 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria2.Expect(x => x.Type, Is.Set);

            acceptance.Any(subCriteria1, subCriteria2);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Name, Layer.Defaults, "Property1");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void EitherIsAnyWithTwoParameters()
        {
            acceptance
                .Either(c => c.Expect(x => x.Name, Is.Set), 
                        c => c.Expect(x => x.Type, Is.Set));

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Name, Layer.Defaults, "Property1");
            acceptance
               .Matches(new PropertyInspector(propertyMapping))
               .ShouldBeTrue();
        }

        [Test]
        public void CanPassSubCriteriaToEither()
        {
            var subCriteria1 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria1.Expect(x => x.Name, Is.Set);

            var subCriteria2 = new ConcreteAcceptanceCriteria<IPropertyInspector>();
            subCriteria2.Expect(x => x.Type, Is.Set);

            acceptance.Either(subCriteria1, subCriteria2);

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Name, Layer.Defaults, "Property1");
            acceptance
                .Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }
    }
}
