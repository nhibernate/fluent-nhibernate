using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentNHibernate.Mapping;
using Iesi.Collections;
using Iesi.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class OneToManyTester
    {
        private ClassMap<OneToManyTarget> _classMap;

        [SetUp]
        public void SetUp()
        {
            _classMap = new ClassMap<OneToManyTarget>();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsSet()
        {
            _classMap.HasMany<ChildObject>(x => x.SetOfChildren)
                .AsSet();

            var document = _classMap.CreateMapping(new MappingVisitor());
            var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");
            classElement.ShouldHaveChild("set");                        
        }

        [Test]
        public void CanSpecifyCollectionTypeAsBag()
        {
            _classMap.HasMany<ChildObject>(x => x.BagOfChildren)
                .AsBag();

            var document = _classMap.CreateMapping(new MappingVisitor());
            var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");
            classElement.ShouldHaveChild("bag");
        }

        [Test]
        public void CanSpecifyCollectionTypeAsList()
        {
            _classMap.HasMany<ChildObject>(x => x.ListOfChildren)
                .AsList();

            var document = _classMap.CreateMapping(new MappingVisitor());
            var classElement = (XmlElement)document.DocumentElement.SelectSingleNode("class");
            classElement.ShouldHaveChild("list");        
        }

        [Test]
        public void ListHasIndexElement()
        {
            _classMap.HasMany<ChildObject>(x => x.ListOfChildren)
                .AsList();

            var document = _classMap.CreateMapping(new MappingVisitor());
            var listElement = (XmlElement)document.DocumentElement.SelectSingleNode("class/list");            
            listElement.ShouldHaveChild("index");            
        }

        [Test]
        public void CanSpecifyForeignKeyColumnAsString()
        {
            _classMap.HasMany<ChildObject>(x => x.BagOfChildren)
                .WithKeyColumn("ParentID");

            var document = _classMap.CreateMapping(new MappingVisitor());
            var keyElement = (XmlElement)document.DocumentElement.SelectSingleNode("//key");
            keyElement.AttributeShouldEqual("column", "ParentID");
        }

        [Test]
        public void CanSpecifyIndexColumnAndTypeForList()
        {
            _classMap.HasMany<ChildObject>(x => x.ListOfChildren)
                .AsList(index => index
                    .WithColumn("ListIndex")
                    .WithType<int>()
                    );

            var document = _classMap.CreateMapping(new MappingVisitor());
            var indexElement = (XmlElement)document.DocumentElement.SelectSingleNode("class/list/index");
            indexElement.AttributeShouldEqual("column", "ListIndex");
            indexElement.AttributeShouldEqual("type", typeof(int).AssemblyQualifiedName);
        }

        public class OneToManyTarget
        {            
            public ISet<ChildObject> SetOfChildren { get; set; }
            public IList<ChildObject> BagOfChildren { get; set; }
            public IList<ChildObject> ListOfChildren { get; set; }            
        }
    }
}
