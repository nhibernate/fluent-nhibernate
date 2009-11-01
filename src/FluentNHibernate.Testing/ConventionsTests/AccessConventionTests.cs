using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel.Access;
using FluentNHibernate.Testing.DomainModel.Access.Mappings;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class AccessConventionTests
    {
        //todo: need to look at timestamp
        //todo: need to look at any mappings
        //todo: need to look at index-many-to-many
        //todo: need to look at entity map
        //todo: need to look at ternary association
        //todo: need to look at dynamic components

        private string expectedAccess = "backfield";

        private ClassMapping compositeId;
        private ClassMapping manyToMany;
        private ClassMapping manyToOne;
        private ClassMapping oneToOne;
        private ClassMapping parent;


        [SetUp]
        public void SetUp()
        {
            PersistenceModel model = new PersistenceModel();

            model.Add(new CompositeIdModelMapping());
            model.Add(new ManyToManyModelMapping());
            model.Add(new ManyToOneModelMapping());
            model.Add(new OneToOneModelMapping());
            model.Add(new ParentModelMapping());

            var classMappings = model.BuildMappings().SelectMany(x => x.Classes).ToDictionary(x => x.Type);
            compositeId = classMappings[typeof(CompositeIdModel)];
            manyToMany = classMappings[typeof(ManyToManyModel)];
            manyToOne = classMappings[typeof(ManyToOneModel)];
            oneToOne = classMappings[typeof(OneToOneModel)];
            parent = classMappings[typeof(ParentModel)];
        }

        [Test]
        public void id_is_set()
        {
            Assert.AreEqual(expectedAccess, ((IdMapping)parent.Id).Access);
        }

        [Test]
        public void composite_id_is_set()
        {
            CompositeIdMapping id;

            id = ((CompositeIdMapping)compositeId.Id);
            Assert.AreEqual(id.Access, id.Access);
            Assert.AreEqual(expectedAccess, id.KeyProperties.First(x => x.Name.Equals("IdA")).Access);
            Assert.AreEqual(expectedAccess, id.KeyProperties.First(x => x.Name.Equals("IdB")).Access);

            id = ((CompositeIdMapping)oneToOne.Id);
            Assert.AreEqual(id.Access, id.Access);
            Assert.AreEqual(expectedAccess, id.KeyManyToOnes.First(x => x.Name.Equals("Parent")).Access);
        }

        [Test]
        public void version_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Version.Access);
        }

        [Test]
        public void property_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Properties.First(x => x.Name.Equals("Property")).Access);
        }

        [Test]
        public void joined_property_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Joins.SelectMany(x => x.Properties).First(x => x.Name.Equals("JoinedProperty")).Access);
        }

        [Test]
        public void component_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Components.First(x => x.Name.Equals("Component")).Access);
        }

        [Test]
        public void one_to_one_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.OneToOnes.First(x => x.Name.Equals("One")).Access);
        }

        [Test]
        public void one_to_many_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("MapOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("SetOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("ListOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("BagOne")).Access);
        }

        [Test]
        public void many_to_many_is_set()
        {
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("MapMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("SetMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("ListMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("BagMany")).Access);
        }

        [Test]
        public void many_to_one_is_set()
        {
            Assert.AreEqual(expectedAccess, manyToOne.References.First(x => x.Name.Equals("Parent")).Access);
        }
    }
}
