using System.Collections.Generic;
using FluentNHibernate.Mapping;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    // NOTE TO MAINTAINERS
    //
    // Most of the tests for many-to-many mapping are still located in the ClassMapXmlCreationTester
    // MY ADVICE: 
    //    - Any time you have to ADD a test for many-to-many, add it HERE not THERE
    //    - Any time you have to MODIFY a test for many-to-many THERE, move it HERE, FIRST.
    // Thanks!  10-NOV-2008 Chad Myers
    
    public class ManyToManyTarget
    {
        public virtual int Id { get; set; }
        public virtual ISet<ChildObject> SetOfChildren { get; set; }
        public virtual IList<ChildObject> BagOfChildren { get; set; }
        public virtual IList<ChildObject> ListOfChildren { get; set; }
        public virtual IDictionary<string, ChildObject> MapOfChildren { get; set; }
        public virtual ChildObject[] ArrayOfChildren { get; set; }
        public virtual IList<string> ListOfSimpleChildren { get; set; }
        public virtual CustomCollection<ChildObject> CustomCollection { get; set; }

        private IList<ChildObject> otherChildren = new List<ChildObject>();
        public virtual IList<ChildObject> GetOtherChildren() { return otherChildren; }
    }

    [TestFixture]
    public class ManyToManyTester
    {
        [Test]
        public void ManyToManyMapping_with_private_backing_field()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(m =>
                    m.HasManyToMany(x => x.GetOtherChildren())
                        .Access.AsCamelCaseField())
                .Element("class/bag")
                .HasAttribute("name", "OtherChildren")
                .HasAttribute("access", "field.camelcase");
        }

        [Test]
        public void ManyToManyMapping_with_foreign_key_name()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(m => m.HasManyToMany(x => x.GetOtherChildren()).WithForeignKeyConstraintNames("FK_Parent", "FK_Child"))
                .Element("class/bag/key")
                .HasAttribute("foreign-key", "FK_Parent")
                .Element("class/bag/many-to-many")
                .HasAttribute("foreign-key", "FK_Child");
        }


        [Test]
        public void Can_use_custom_collection_implicitly()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map => map.HasManyToMany(x => x.CustomCollection))
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly_generic()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                    map.HasManyToMany(x => x.BagOfChildren)
                        .CollectionType<CustomCollection<ChildObject>>()
                )
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                    map.HasManyToMany(x => x.BagOfChildren)
                        .CollectionType(typeof(CustomCollection<ChildObject>))
                )
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly_name()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                    map.HasManyToMany(x => x.BagOfChildren)
                        .CollectionType("name")
                )
                .Element("class/bag").HasAttribute("collection-type", "name");
        }

        [Test]
        public void NotFound_sets_attribute()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                    map.HasManyToMany(x => x.BagOfChildren)
                        .NotFound.Ignore())
                .Element("class/bag/many-to-many").HasAttribute("not-found", "ignore");
        }

        [Test]
        public void ShouldWriteCacheElementWhenAssigned()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                            map.HasManyToMany(x => x.SetOfChildren)
                                .Cache.AsReadWrite())
                .Element("class/set/cache").ShouldNotBeNull();
        }

        [Test]
        public void ShouldNotWriteCacheElementWhenEmpty()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                            map.HasManyToMany(x => x.SetOfChildren))
                .Element("class/set/cache").DoesntExist();
        }

        [Test]
        public void ShouldWriteCacheElementFirst()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                            map.HasManyToMany(x => x.SetOfChildren)
                                .Cache.AsReadWrite())
                .Element("class/set/cache").ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void ShouldWriteBatchSizeAttributeWhenAssigned()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                            map.HasManyToMany(x => x.SetOfChildren)
                                .BatchSize(15))
                .Element("class/set").HasAttribute("batch-size", "15");
        }

        [Test]
        public void ShouldNotWriteBatchSizeAttributeWhenEmpty()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasManyToMany(x => x.SetOfChildren))
                .Element("class/set").DoesntHaveAttribute("batch-size");
        }

        [Test]
        public void ArrayHasIndexElement()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map => map.HasManyToMany(x => x.ArrayOfChildren).AsArray(x => x.Position))
                .Element("class/array/index").Exists();
        }

        [Test]
        public void ShouldWriteIndexWhenAssigned()
        {
           new MappingTester<OneToManyTarget>()
               .ForMapping(map =>
                           map.HasManyToMany(x => x.SetOfChildren)
                           .AsMap("indexColumn"))
               .Element("class/map/index").ShouldNotBeNull();
        }

       [Test]
       public void ShouldWriteIndexAtCorrectPosition()
       {
           new MappingTester<OneToManyTarget>()
               .ForMapping(map =>
                           map.HasManyToMany(x => x.SetOfChildren)
                           .AsMap("indexColumn")
                           .WithParentKeyColumn("ParentID")
                           .WithChildKeyColumn("ChildID")
                           .Cache.AsReadWrite())
               .Element("class/map/index").ShouldBeInParentAtPosition(2);
       }
    }
}
