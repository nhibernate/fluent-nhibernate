using System.Xml;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Testing;
using FluentNHibernate.Testing.AutoMap.ManyToMany;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoMapTests
    {
        [Test]
        public void AutoMapIdentification()
        {
            var autoMapper = new AutoMapper(new Conventions());
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
            var autoMapper = new AutoMapper(new Conventions());
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
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//property");
            keyElement.AttributeShouldEqual("column", "LineOne");
            keyElement.AttributeShouldEqual("name", "LineOne");
        }

        [Test]
        public void AutoMapIgnoreProperty()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//property");
            keyElement.AttributeShouldEqual("column", "LineOne");
            keyElement.AttributeShouldEqual("name", "LineOne");
        }

        [Test]
        public void AutoMapManyToOne()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-one");
            keyElement.AttributeShouldEqual("column", "Parent_id");
            keyElement.AttributeShouldEqual("name", "Parent");
        }

        [Test]
        public void AutoMapManyToMany()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ManyToMany1>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-many");
            keyElement.AttributeShouldEqual("column", "ManyToMany2_id");
        }

        [Test]
        public void AutoMapManyToMany_ShouldRecognizeSet_BaseOnType()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ManyToMany1>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-many");
            keyElement.ParentNode.Name.ShouldEqual("set");
        }

        [Test]
        public void AutoMapOneToMany()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleParentClass>();

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//bag");
            keyElement.AttributeShouldEqual("name", "Examples");
        }
    }
}

