using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentNHibernate;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Test;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class AutoMapTests
    {
        [Test]
        public void AutoMapAssembly()
        {

/*
 *          Need to think of way to test this.
 *          
            var autoModel = new AutoPersistenceModel(Assembly.GetAssembly(typeof(AutoMapTests)));
            autoModel.AddEntityAssembly(Assembly.GetAssembly(typeof (AutoMapTests)),
                                        t => t.Namespace == "FluentNHibernate.AutoMap.Test");
 * */
        }

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

namespace FluentNHibernate.AutoMap.Test
{
    public class ExampleCustomColumn
    {
        public int CustomColumn
        {
            get
            {
                return 12;
            }
        }
    }

    public class ExampleInheritedClass : ExampleClass
    {
        public string ExampleProperty { get; set; }
    }

    public class ExampleClass
    {
        public virtual int Id { get; set; }
        public virtual string LineOne { get; set; }
        public DateTime Timestamp { get; set; }
        public ExampleParentClass Parent { get; set; }
    }

    public class ExampleParentClass
    {
        public virtual int Id { get; set; }
        public virtual IList<ExampleClass> Examples {get; set;}
    }
}