using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.Inspectors
{
    [TestFixture]
    public class PropertyAcceptanceCriteriaEqualSubPropertyTests
    {
        private IAcceptanceCriteria<IPropertyInspector> acceptance;

        [SetUp]
        public void CreateAcceptanceCriteria()
        {
            acceptance = new ConcreteAcceptanceCriteria<IPropertyInspector>();
        }

        [Test]
        public void ExpectShouldEvaluateSubProperty()
        {
            acceptance.Expect(x => x.PropertyType.Name, Is.Equal(typeof(int).Name));

            acceptance.Matches(new PropertyDsl(new PropertyMapping {PropertyInfo = ReflectionHelper.GetProperty<Record>(x => x.Age)}))
                .ShouldBeTrue();
        }

        [Test]
        public void ExpectShouldEvaluateSubPropertyWithEvaluation()
        {
            acceptance.Expect(x => x.PropertyType.Name == typeof(int).Name);

            acceptance.Matches(new PropertyDsl(new PropertyMapping {PropertyInfo = ReflectionHelper.GetProperty<Record>(x => x.Age)}))
                .ShouldBeTrue();
        }
    }
}