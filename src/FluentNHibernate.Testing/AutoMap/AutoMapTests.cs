using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.TestFixtures;
using FluentNHibernate.Metadata;
using FluentNHibernate.Testing.AutoMap.ManyToMany;
using NHibernate.Cfg;
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
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());

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
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());

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
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());

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
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());

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
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());

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
            var map = autoMapper.Map<ManyToMany1>(new List<AutoMapType>());

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-many");
            keyElement.AttributeShouldEqual("column", "ManyToMany2_id");
        }

        [Test]
        public void AutoMapManyToMany_ShouldRecognizeSet_BaseOnType()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ManyToMany1>(new List<AutoMapType>());

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//many-to-many");
            keyElement.ParentNode.Name.ShouldEqual("set");
        }

        [Test]
        public void AutoMapOneToMany()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleParentClass>(new List<AutoMapType>());

            Assert.IsNotNull(map);

            var document = map.CreateMapping(new MappingVisitor());

            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//bag");
            keyElement.AttributeShouldEqual("name", "Examples");
        }

        [Test]
        public void AutoMapDoesntSetCacheWithDefaultConvention()
        {
            var autoMapper = new AutoMapper(new Conventions());
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());
            var document = map.CreateMapping(new MappingVisitor());

            var cacheElement = document.DocumentElement.SelectSingleNode("//cache");

            Assert.That(cacheElement, Is.Null);
        }

        [Test]
        public void AutoMapSetsCacheOnClassUsingConvention()
        {
            var conventions = new Conventions();
            var autoMapper = new AutoMapper(conventions);

            conventions.DefaultCache = cache => cache.AsReadOnly();

            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());
            var document = map.CreateMapping(new MappingVisitor(conventions, new Configuration(), new DependencyChain()));
            var cacheElement = document.DocumentElement.SelectSingleNode("//cache");

            Assert.That(cacheElement, Is.Not.Null);
        }
    }
}

