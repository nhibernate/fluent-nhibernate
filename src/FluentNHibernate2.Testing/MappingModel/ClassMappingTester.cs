using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class ClassMappingTester
    {
        private ClassMapping _classMapping;

        [SetUp]
        public void SetUp()
        {
            _classMapping = new ClassMapping();
        }
        
        [Test]
        public void CanConstructValidInstance()
        {
            _classMapping.Name = "class1";
            _classMapping.Id = MappingMother.CreateNativeIDMapping();
            _classMapping.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void CanSetIdToBeStandardIdMapping()
        {
            var idMapping = MappingMother.CreateNativeIDMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
            _classMapping.Hbm.Id.ShouldEqual(idMapping.Hbm);
        }

        [Test]
        public void CanSetIdToBeCompositeIdMapping()
        {
            var idMapping = new CompositeIdMapping();
            _classMapping.Id = idMapping;

            _classMapping.Id.ShouldEqual(idMapping);
            _classMapping.Hbm.CompositeId.ShouldEqual(idMapping.Hbm);
        }

        [Test]
        public void CanAddProperty()
        {
            var property = new PropertyMapping("Property1");
            _classMapping.AddProperty(property);

            _classMapping.Properties.ShouldContain(property);
            _classMapping.Hbm.Items.ShouldContain(property.Hbm);

        }

        [Test]
        public void CanAddBag()
        {
            var bag = new BagMapping("bag1", new KeyMapping(), new OneToManyMapping("class1"));
            _classMapping.AddCollection(bag);

            _classMapping.Collections.ShouldContain(bag);
            _classMapping.Hbm.Items.ShouldContain(bag.Hbm);
        }

        [Test]
        public void CanAddReference()
        {
            var reference = new ManyToOneMapping("parent");
            _classMapping.AddReference(reference);

            _classMapping.References.ShouldContain(reference);
            _classMapping.Hbm.Items.ShouldContain(reference.Hbm);
        }
    }
}