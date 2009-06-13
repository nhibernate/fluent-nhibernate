using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Is=FluentNHibernate.Conventions.AcceptanceCriteria.Is;

namespace FluentNHibernate.Testing.ConventionsTests.AcceptanceCriteria
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
        public void ExpectShouldEvaluateSubPropertyWithEvaluation()
        {
            acceptance.Expect(x =>
                x.Type.Name == typeof(Record).Name);

            acceptance.Matches(new PropertyInspector(new PropertyMapping
            {
                PropertyInfo = ReflectionHelper.GetProperty<Record>(x => x.Age),
                Type = new TypeReference(typeof(Record))
            }))
                .ShouldBeTrue();
        }
    }
}