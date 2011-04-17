using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaInheritanceTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ShouldInheritCriteriaFromAnotherConvention()
        {
            acceptance
                .SameAs<AnotherConvention>();

            acceptance.Expectations.Count()
                .ShouldEqual(2);
        }

        [Test]
        public void ShouldUseInheritedCriteria()
        {
            acceptance
                .SameAs<AnotherConvention>();

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, false);
            propertyMapping.Set(x => x.Update, Layer.Defaults, false);
            acceptance.Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        [Test]
        public void ShouldInheritCriteriaFromAnotherConventionOpposite()
        {
            acceptance
                .OppositeOf<AnotherConvention>();

            acceptance.Expectations.Count()
                .ShouldEqual(2);
        }

        [Test]
        public void ShouldUseInheritedOppositeCriteria()
        {
            acceptance
                .OppositeOf<AnotherConvention>();

            var propertyMapping = new PropertyMapping();
            propertyMapping.Set(x => x.Insert, Layer.Defaults, true);
            propertyMapping.Set(x => x.Update, Layer.Defaults, true);
            acceptance.Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }

        private class AnotherConvention : IPropertyConvention, IConventionAcceptance<IPropertyInspector>
        {
            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria
                    .Expect(x => x.Insert == false)
                    .Expect(x => x.Update == false);
            }

            public void Apply(IPropertyInstance instance)
            { }
        }
    }
}