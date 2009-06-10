using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.DslImplementation;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.Alterations
{
    [TestFixture, Category("Alteration DSL")]
    public class PropertyAlterationMapsToPropertyMapping
    {
        private PropertyMapping mapping;
        private IPropertyAlteration alteration;

        [SetUp]
        public void CreateDsl()
        {
            mapping = new PropertyMapping();
            alteration = new PropertyAlteration(mapping);
        }

        [Test]
        public void AccessMapped()
        {
            alteration.Access.AsField();
            mapping.Access.ShouldEqual("field");
        }
    }
}