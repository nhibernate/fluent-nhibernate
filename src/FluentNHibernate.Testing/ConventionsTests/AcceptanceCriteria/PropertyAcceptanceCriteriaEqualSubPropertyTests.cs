using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

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

            var propertyMapping = new PropertyMapping
            {
                Member = ReflectionHelper.GetMember<Record>(x => x.Age),
            };
            propertyMapping.Set(x => x.Type, Layer.Defaults, new TypeReference(typeof(Record)));
            acceptance.Matches(new PropertyInspector(propertyMapping))
                .ShouldBeTrue();
        }
    }
}