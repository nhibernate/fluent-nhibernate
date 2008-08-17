using System.Xml;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoMapTests
    {
        [Test]
        public void AutoMapIdentification()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//id");
            keyElement.AttributeShouldEqual("column", "Id");
            keyElement.AttributeShouldEqual("name", "Id");
        }

        [Test]
        public void AutoMapVersion()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//version");
            keyElement.AttributeShouldEqual("column", "Timestamp");
            keyElement.AttributeShouldEqual("name", "Timestamp");
        }

        [Test]
        public void AutoMapProperty()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//property");
            keyElement.AttributeShouldEqual("column", "LineOne");
            keyElement.AttributeShouldEqual("name", "LineOne");
        }

        [Test]
        public void AutoMapDoesntIncludeCustomProperty()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleCustomColumn>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//property");
            Assert.IsNull(keyElement);
        }

        [Test]
        public void AutoMapManyToOne()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-one");
            keyElement.AttributeShouldEqual("column", "Parent_id");
            keyElement.AttributeShouldEqual("name", "Parent");
        }

        [Test]
        public void AutoMapOneToMany()
        {
            var autoMapper = new AutoMapper();
            var map = autoMapper.Map<ExampleParentClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//bag");
            keyElement.AttributeShouldEqual("name", "Examples");
        }
    }
}

