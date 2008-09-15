using System.Collections.Generic;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    public class OneToManyTarget
    {
        public int Id { get; set; }        
        public ISet<ChildObject> SetOfChildren { get; set; }
        public IList<ChildObject> BagOfChildren { get; set; }
        public IList<ChildObject> ListOfChildren { get; set; }
        public IDictionary<string, ChildObject> MapOfChildren { get; set; }
        public ChildObject[] ArrayOfChildren { get; set; }
    }

    public class OneToManyComponentTarget
    {
        public virtual ISet<ComponentOfMappedObject> SetOfComponents { get; set; }
        public virtual ComponentOfMappedObject Component { get; set; }
    }

    [TestFixture]
    public class OneToManyTester
    {
        [Test]
        public void CanSpecifyCollectionOfComponents()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/bag/composite-element").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsBag()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.BagOfChildren).AsBag())
                .Element("class/bag").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsList()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.ListOfChildren).AsList())
                .Element("class/list").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsSet()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.SetOfChildren).AsSet())
                .Element("class/set").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsArray()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.ArrayOfChildren).AsArray(x => x.Name))
                .Element("class/array").Exists();
        }

        [Test]
        public void CanSpecifyForeignKeyColumnAsString()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.BagOfChildren).WithKeyColumn("ParentID"))
                .Element("class/bag/key")
                .HasAttribute("column", "ParentID");
        }

        [Test]
        public void CanSpecifyIndexColumnAndTypeForList()
        {
            new MappingTester<OneToManyTarget>()
            .ForMapping(map => map.HasMany<ChildObject>(x => x.ListOfChildren)
                .AsList(index => index
                                    .WithColumn("ListIndex")
                                    .WithType<int>()
                ))
                .Element("class/list/index")
                .HasAttribute("column", "ListIndex")
                .HasAttribute("type", typeof(int).AssemblyQualifiedName);
        }

        [Test]
        public void cascade_attribute_is_noneexistant_if_not_specified()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(c => c.HasMany<ChildObject>(x => x.BagOfChildren))
                .Element("class/bag").DoesntHaveAttribute("cascade");
        }

        [Test]
        public void ListHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.ListOfChildren).AsList())
                .Element("class/list/index").Exists();
        }

        [Test]
        public void MapHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map/index").HasAttribute("type", typeof(string).AssemblyQualifiedName)
                .Element("class/map/index").HasAttribute("column", "Name");
        }

        [Test]
        public void ArrayHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.ArrayOfChildren).AsArray(x => x.Position))
                .Element("class/array/index").Exists();
        }

        [Test]
        public virtual void CanOverrideMapIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany<ChildObject>(x => x.MapOfChildren).AsMap(
                    x => x.Name,
                    index => index.WithColumn("ChildObjectName")))
                .Element("class/map/index").HasAttribute("column", "ChildObjectName");
        }

        [Test]
        public void OneToManyElementIsExcludedForComponents()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/bag/one-to-many").DoesntExist();
        }

        [Test]
        public void ShouldMapElementsOfCompositeElement()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/bag/composite-element/property[@name = 'Name']").Exists();
        }

        [Test]
        public void setting_the_cascade_to_something_other_than_none_updates_the_cascade_attribute()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(c => c.HasMany<ChildObject>(x => x.BagOfChildren).Cascade.AllDeleteOrphan())
                .Element("class/bag")
                .HasAttribute("cascade", "all-delete-orphan");
        }

        [Test]
        public void SetsLazyLoadingAsDefault()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/bag").DoesntHaveAttribute("lazy");
        }

        [Test]
        public void SetsLazyLoadingOnThroughConvention()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OneToManyConvention = p => p.SetAttribute("lazy", "true");

            new MappingTester<OneToManyComponentTarget>()
                .UsingVisitor(visitor)
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents).Component(c => c.Map(x => x.Name)))
                .Element("class/bag").HasAttribute("lazy", "true");
        }


        [Test]
        public void SetsCascadeOffAsDefault()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/bag").DoesntHaveAttribute("cascade");
        }

        [Test]
        public void SetsCascadeOnThroughConvention()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OneToManyConvention = p => p.SetAttribute("cascade", "all");

            new MappingTester<OneToManyComponentTarget>()
                .UsingVisitor(visitor)
                .ForMapping(m => m.HasMany<ComponentOfMappedObject>(x => x.SetOfComponents).Component(c => c.Map(x => x.Name)))
                .Element("class/bag").HasAttribute("cascade", "all");
        }

    }
}