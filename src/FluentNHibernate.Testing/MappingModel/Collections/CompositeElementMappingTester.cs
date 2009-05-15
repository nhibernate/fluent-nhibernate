using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class CompositeElementMappingTester
    {
        private CompositeElementMapping compositeElementMapping;

        [TestFixtureSetUp]
        public void SetUp()
        {
            compositeElementMapping = new CompositeElementMapping();
        }

        [Test]
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping { Name = "parent" };
            compositeElementMapping.AddReference(reference);

            compositeElementMapping.References.ShouldContain(reference);
        }

        [Test]
        public void CanAddProperty()
        {
            var property = new PropertyMapping { Name = "Property1" };
            compositeElementMapping.AddProperty(property);

            compositeElementMapping.Properties.ShouldContain(property);
        }
    }
}
