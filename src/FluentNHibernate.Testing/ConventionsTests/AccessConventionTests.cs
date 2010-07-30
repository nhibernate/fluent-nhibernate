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
            model.Conventions.Add(new BackfieldAccessConvention());

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
        public void IdIsSet()
        {
            Assert.AreEqual(expectedAccess, ((IdMapping)parent.Id).Access);
        }

        [Test]
        public void CompositeIdIsSet()
        {
            CompositeIdMapping id;

            id = ((CompositeIdMapping)compositeId.Id);
            Assert.AreEqual(expectedAccess, id.Access);
            Assert.AreEqual(expectedAccess, id.Keys.First(x => x.Name.Equals("IdA")).Access);
            Assert.AreEqual(expectedAccess, id.Keys.First(x => x.Name.Equals("IdB")).Access);

            id = ((CompositeIdMapping)oneToOne.Id);
            Assert.AreEqual(expectedAccess, id.Access);
            Assert.AreEqual(expectedAccess, id.Keys.First(x => x.Name.Equals("Parent")).Access);
        }

        [Test]
        public void VersionIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Version.Access);
        }

        [Test]
        public void PropertyIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Properties.First(x => x.Name.Equals("Property")).Access);
        }

        [Test]
        public void JoinedPropertyIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Joins.SelectMany(x => x.Properties).First(x => x.Name.Equals("JoinedProperty")).Access);
        }

        [Test]
        public void ComponentIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Components.First(x => x.Name.Equals("Component")).Access);
        }

        [Test]
        public void DynamicComponentIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Components.First(x => x.Name.Equals("Dynamic")).Access);
        }

        [Test]
        public void OneToOneIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.OneToOnes.First(x => x.Name.Equals("One")).Access);
        }

        [Test]
        public void OneToManyIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("MapOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("SetOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("ListOne")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("BagOne")).Access);
        }

        [Test]
        public void ManyToManyIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("MapMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("SetMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("ListMany")).Access);
            Assert.AreEqual(expectedAccess, parent.Collections.First(x => x.Name.Equals("BagMany")).Access);
        }

        [Test]
        public void ManyToOneIsSet()
        {
            Assert.AreEqual(expectedAccess, manyToOne.References.First(x => x.Name.Equals("Parent")).Access);
        }

        [Test]
        public void AnyIsSet()
        {
            Assert.AreEqual(expectedAccess, parent.Anys.First(x => x.Name.Equals("Any")).Access);
        }

        private class BackfieldAccessConvention : AccessConvention
        {
            protected override void Apply(Type owner, string name, FluentNHibernate.Conventions.Instances.IAccessInstance access)
            {
                access.BackField();
            }
        }
    }
}
