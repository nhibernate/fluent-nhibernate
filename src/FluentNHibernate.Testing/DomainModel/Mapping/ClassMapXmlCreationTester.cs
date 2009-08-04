using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapXmlCreationTester
    {
        [Test]
        public void BasicManyToManyMapping()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.HasManyToMany(x => x.Children))
                .Element("class/bag")
                    .HasAttribute("name", "Children")
                    .DoesntHaveAttribute("cascade")
                .Element("class/bag/key/column").HasAttribute("name", "MappedObject_id")
                .Element("class/bag/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                    .DoesntHaveAttribute("fetch")
                .Element("class/bag/many-to-many/column").HasAttribute("name", "ChildObject_id");
        }
        
        [Test]
        public void ManyToManyAsSet()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet())
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/set/key/column").HasAttribute("name", "MappedObject_id")
                .Element("class/set/many-to-many").HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                .Element("class/set/many-to-many/column").HasAttribute("name", "ChildObject_id");
		}

		[Test]
		public void ManyToManyAsBag()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsBag())
                .Element("class/bag")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/bag/key/column").HasAttribute("name", "MappedObject_id")
                .Element("class/bag/many-to-many").HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                .Element("class/bag/many-to-many/column").HasAttribute("name", "ChildObject_id");
		}
		
		[Test]
		public void ManyToManyAsSetWithChildForeignKey()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().ChildKeyColumn("TheKids_ID"))
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                .Element("class/set/key/column").HasAttribute("name", "MappedObject_id")
                .Element("class/set/many-to-many").HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                .Element("class/set/many-to-many/column").HasAttribute("name", "TheKids_ID");
		}

		[Test]
		public void ManyToManyAsSetWithParentForeignKey()
		{
			new MappingTester<MappedObject>()
				.ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().ParentKeyColumn("TheParentID"))
				.Element("class/set")
					.HasAttribute("name", "Children")
					.HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
				.Element("class/set/key/column").HasAttribute("name", "TheParentID")
				.Element("class/set/many-to-many").HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
				.Element("class/set/many-to-many/column").HasAttribute("name", "ChildObject_id");
		}

		[Test]
		public void ManyToManyAsSetWithJoinFetchMode()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).AsSet().FetchType.Join())
                .Element("class/set")
                    .HasAttribute("name", "Children")
                    .HasAttribute("table", typeof(ChildObject).Name + "To" + typeof(MappedObject).Name)
                    .HasAttribute("fetch", "join")
                .Element("class/set/key/column").HasAttribute("name", "MappedObject_id")
                .Element("class/set/many-to-many")
                    .HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName)
                .Element("class/set/many-to-many/column").HasAttribute("name", "ChildObject_id");
		}

		[Test]
        public void BasicOneToManyMapping()
        {
			new MappingTester<MappedObject>()
				.ForMapping(map => map.HasMany(x => x.Children))
				.Element("class/bag")
					.HasAttribute("name", "Children")
				.Element("class/bag/key/column")
					.HasAttribute("name", "MappedObject_id")
				.Element("class/bag/one-to-many")
					.HasAttribute("class", typeof (ChildObject).AssemblyQualifiedName);
        }

        [Test]
        public void AdvancedOneToManyMapping()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasMany(x => x.Children).LazyLoad().Inverse())
                .Element("class/bag[@name='Children']")
                    .HasAttribute("lazy", "true")
                    .HasAttribute("inverse", "true");
        }

        [Test]
        public void AdvancedOneToManyMapping_NotLazy_NotInverse()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasMany(x => x.Children).Not.LazyLoad().Not.Inverse())
                .Element("class/bag[@name='Children']")
                    .HasAttribute("lazy", "false")
                    .HasAttribute("inverse", "false");
        }

        [Test]
        public void AdvancedManyToManyMapping()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).LazyLoad().Inverse())
                .Element("class/bag[@name='Children']")
                    .HasAttribute("lazy", "true")
                    .HasAttribute("inverse", "true");
        }

        [Test]
        public void AdvancedManyToManyMapping_NotLazy_NotInverse()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).Not.LazyLoad().Not.Inverse())
                .Element("class/bag[@name='Children']")
                    .HasAttribute("lazy", "false")
                    .HasAttribute("inverse", "false");
        }

        [Test]
        public void CascadeAll_with_many_to_many()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.HasManyToMany(x => x.Children).Cascade.All())
                .Element("class/bag[@name='Children']")
                    .HasAttribute("cascade", "all");
        }

        [Test]
        public void CascadeAll_with_one_to_many()
        {
        	new MappingTester<MappedObject>()
        		.ForMapping(map => map.HasMany(x => x.Children).Cascade.All())
        		.Element("class/bag[@name='Children']").HasAttribute("cascade", "all");
        }

        [Test]
        public void Create_a_component_mapping()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Component(x => x.Component, c =>
                 {
                     c.Map(x => x.Name);
                     c.Map(x => x.Age);
                 }))
                .Element("class/component")
                    .HasAttribute("name", "Component")
                    .HasAttribute("insert", "true")
                    .HasAttribute("update", "true")
                .Element("class/component/property[@name='Name']").Exists()
                .Element("class/component/property[@name='Age']").Exists();
        }

        [Test]
        public void DetermineTheTableName()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => { })
                .Element("class").HasAttribute("table", "`MappedObject`");
        }

        [Test]
        public void CanSetTableName()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Table("myTableName"))
                .Element("class").HasAttribute("table", "myTableName");
        }

        [Test]
        public void CanOverrideLazyLoad()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.LazyLoad())
                .Element("class").HasAttribute("lazy", "true");
        }

        [Test]
        public void CanOverrideNotLazyLoad()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.LazyLoad())
                .Element("class").HasAttribute("lazy", "false");
        }

        [Test] // stupid, I know but somebody'll try it
        public void DoubleNotWorksCorrectly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.Not.LazyLoad())
                .Element("class").HasAttribute("lazy", "true");
        }

        [Test]
        public void Map_an_enumeration()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.Color))
                .Element("class/property[@name='Color']")
                    .Exists()
                    .HasAttribute("type", typeof(GenericEnumMapper<ColorEnum>).AssemblyQualifiedName)
                .Element("class/property[@name='Color']/column")
                    .Exists();
        }

        [Test]
		public void MapANullableEnumeration()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Map(x => x.NullableColor))
                .Element("class/property[@name='NullableColor']")
                    .Exists()
                    .HasAttribute("type", typeof(GenericEnumMapper<ColorEnum>).AssemblyQualifiedName)
                .Element("class/property[@name='NullableColor']/column")
                    .Exists()
                    .HasAttribute("not-null", "false");
		}

        [Test]
        public void MapASimplePropertyWithNoOverrides()
        {
            new MappingTester<MappedObject>()
                .ForMapping(map => map.Map(x => x.Name))
                .Element("//property[@name='Name']")
                    .Exists()
                    .HasAttribute("type", typeof(string).AssemblyQualifiedName)
                .Element("//property[@name='Name']/column")
                    .Exists()
                    .HasAttribute("name", "Name");
        }

        [Test]
        public void WriteTheClassNode()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => { })
                .Element("class")
                    .HasAttribute("name", typeof(MappedObject).AssemblyQualifiedName)
                    .HasAttribute("table", "`MappedObject`");
        }

        [Test]
		public void WriteTheClassNodeForGenerics()
		{
            new MappingTester<MappedGenericObject<MappedObject>>()
                .ForMapping(m => { })
                .Element("class")
                    .HasAttribute("name", typeof(MappedGenericObject<MappedObject>).AssemblyQualifiedName)
                    .HasAttribute("table", "`MappedGenericObject_MappedObject`");
		}

		[Test]
		public void DomainClassMapWithId()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Id(x => x.Id, "id"))
                .Element("class/id")
                    .Exists()
                    .HasAttribute("name", "Id")
                    .HasAttribute("type", typeof(Int64).AssemblyQualifiedName)
                .Element("class/id/generator")
                    .Exists()
                    .HasAttribute("class", "identity")
                .Element("class/id/column")
                    .Exists()
                    .HasAttribute("name", "id");
		}

		[Test]
		public void DomainClassMapWithIdNoColumn()
		{
		    new MappingTester<MappedObject>()
		        .ForMapping(m => m.Id(x => x.Id))
		        .Element("class/id/column").HasAttribute("name", "Id");
		}

        [Test]
		public void DomainClassMapWithIdNoColumnAndGenerator()
		{
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Id(x => x.Id).GeneratedBy.Native())
                .Element("class/id/generator")
                    .HasAttribute("class", "native");
		}

   		[Test]
		public void CreatingAManyToOneReferenceWithColumnSpecified()
		{
   		    new MappingTester<MappedObject>()
   		        .ForMapping(m => m.References(x => x.Parent, "MyParentId"))
   		        .Element("class/many-to-one/column")
                    .HasAttribute("name", "MyParentId");
		}

        [Test]
        public void CreatingAManyToOneReferenceWithColumnSpecifiedThroughColumnNameMethod()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.References(x => x.Parent).Column("MyParentId"))
                .Element("class/many-to-one/column")
                    .HasAttribute("name", "MyParentId");
        }

		[Test]
		public void CreatingAManyToOneReferenceUsingSpecifiedForeignKey()
		{
		    new MappingTester<MappedObject>()
		        .ForMapping(m => m.References(x => x.Parent).ForeignKey("FK_MyForeignKey"))
		        .Element("class/many-to-one").HasAttribute("foreign-key", "FK_MyForeignKey");
		}

		[Test]
		public void CreatingAManyToOneReferenceWithCascadeSpecifiedAsNone()
		{
		    new MappingTester<MappedObject>()
		        .ForMapping(m => m.References(x => x.Parent).Cascade.None())
		        .Element("class/many-to-one").HasAttribute("cascade", "none");
		}

		[Test]
		public void CreatingAManyToOneReferenceWithFetchtypeSet()
		{
			new MappingTester<MappedObject>()
				.ForMapping(m => m.References(x => x.Parent).Fetch.Select())
				.Element("class/many-to-one").HasAttribute("fetch", "select");
		}

        [Test]
        public void CanSetSchema()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Schema("test"))
                .Element("class").HasAttribute("schema", "test");
        }

        [Test]
        public void SpanningClassAcrossTwoTables()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Join("tableTwo", t => t.Map(x => x.Name)))
                .Element("class/join").Exists();
        }

        [Test]
        public void CanSetReadonly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.ReadOnly())
                .Element("class").HasAttribute("mutable", "false");
        }

        [Test]
        public void CanSetNonReadonly()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Not.ReadOnly())
                .Element("class").HasAttribute("mutable", "true");
        }

        [Test]
        public void ShouldWriteBatchSizeAttributeWhenAssigned()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.BatchSize(15))
                .Element("class").HasAttribute("batch-size", "15");
        }

        [Test]
        public void ShouldNotWriteBatchSizeAttributeWhenEmpty()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => { })
                .Element("class").DoesntHaveAttribute("batch-size");
        }

        [Test]
        public void ShouldAddCacheElementBeforeId()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => { x.Id(y => y.Id); x.Cache.ReadWrite(); })
                .Element("class/cache")
                    .ShouldBeInParentAtPosition(0)
                .Element("class/id")
                    .ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void ShouldAddCacheElementBeforeCompositeId()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => { x.CompositeId().KeyProperty(y => y.Id).KeyProperty(y => y.Name); x.Cache.ReadWrite(); })
                .Element("class/cache")
                    .ShouldBeInParentAtPosition(0)
                .Element("class/composite-id")
                    .ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void ShouldAddCompositeIdBeforeDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => { x.DiscriminateSubClassesOnColumn("test"); x.CompositeId().KeyProperty(y => y.Id).KeyProperty(y => y.Name); })
                .Element("class/composite-id")
                    .ShouldBeInParentAtPosition(0)
                .Element("class/discriminator")
                    .ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void ShouldAddIdBeforeDiscriminator()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => { x.DiscriminateSubClassesOnColumn("test"); x.Id(y => y.Id); })
                .Element("class/id")
                    .ShouldBeInParentAtPosition(0)
                .Element("class/discriminator")
                    .ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void ShouldAddDiscriminatorBeforeVersion()
        {
            new MappingTester<MappedObject>()
                .ForMapping(x => { x.Version(y => y.Version); x.DiscriminateSubClassesOnColumn("test"); })
                .Element("class/discriminator")
                    .ShouldBeInParentAtPosition(0)
                .Element("class/version")
                    .ShouldBeInParentAtPosition(1);
        }
    }

    public class SecondMappedObject
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public int IdPart1 { get; set; }
        public int IdPart2 { get; set; }
        public int IdPart3 { get; set; }
    }

    public class ComponentOfMappedObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public enum ColorEnum
    {
        Blue,
        Green,
        Red
    }

    public class MappedObject
    {
        public ColorEnum Color { get; set; }

        public ColorEnum? NullableColor { get; set; }

        public ComponentOfMappedObject Component { get; set; }

        public SecondMappedObject Parent { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public IList<ChildObject> Children { get; set; }
        public IDictionary Dictionary { get; set; }

        public long Id { get; set; }

        public int Version { get; set; }
    }

    public class MappedObjectSubclass : MappedObject
    {
        public int SubclassProperty { get; set; }

        public ChildObject Child { get; set; }

        public IList<ChildObject> Children { get; set; }
    }

    public class MappedObjectSubSubClass : MappedObjectSubclass
    {
        public int SubSubclassProperty { get; set; }

        public ChildObject SubSubclassChild { get; set; }

        public IList<ChildObject> SubSubclassChildren { get; set; }
    }

    public class ChildObject
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Position { get; set; }
    }

    public class MappedGenericObject<T>
	{
		public T Owner { get; set; }
	}
}
