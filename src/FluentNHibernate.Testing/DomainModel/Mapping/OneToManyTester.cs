using System.Collections.Generic;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    using System.Linq;

    public class OneToManyTarget
    {
        public virtual int Id { get; set; }
        public virtual ISet<ChildObject> SetOfChildren { get; set; }
        public virtual IList<ChildObject> BagOfChildren { get; set; }
        public virtual IList<ChildObject> ListOfChildren { get; set; }
        public virtual IDictionary<string, ChildObject> MapOfChildren { get; set; }
        public virtual ChildObject[] ArrayOfChildren { get; set; }
        public virtual IList<string> ListOfSimpleChildren { get; set; }
        public virtual CustomCollection<ChildObject> CustomCollection { get; set; }
        public virtual IDictionary<MapIndex, MapContents> MapOfEnums { get; set; }

        private IList<ChildObject> otherChildren = new List<ChildObject>();
        public virtual IList<ChildObject> GetOtherChildren() { return otherChildren; }

        private IList<ChildObject> listToArrayChild = new List<ChildObject>();
        public virtual ChildObject[] ListToArrayChild { get { return listToArrayChild.ToArray(); } }

    }

    public enum MapIndex
    {
        Index1,
        Index2
    }

    public enum MapContents
    {
        Contents1,
        Contents2
    }

    public class OneToManyComponentTarget
    {
        public virtual ISet<ComponentOfMappedObject> SetOfComponents { get; set; }
        public virtual ComponentOfMappedObject Component { get; set; }
    }

    public class CustomCollection<T> : List<T>
    {}

    [TestFixture]
    public class OneToManyTester
    {
        [Test]
        public void CanSpecifyCollectionOfComponents()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set/composite-element").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsBag()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.BagOfChildren).AsBag())
                .Element("class/bag").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsList()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).AsList())
                .Element("class/list").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsSet()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.SetOfChildren).AsSet())
                .Element("class/set").Exists()
                .OutputToConsole();
                
        }

        [Test]
        public void CanSpecifyCollectionTypeAsMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map").Exists();
        }


        [Test]
        public void CanSpecifyCollectionTypeAsMapOfEnums()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfEnums)
                                       .AsMap<MapIndex>(
                                       index => index.WithColumn("IndexColumn").WithType<MapIndex>(),
                                       element => element.WithColumn("ElementColumn").WithType<MapContents>()))
                .Element("class/map/index")
                .HasAttribute("column", "IndexColumn")
                .HasAttribute("type", typeof (MapIndex).AssemblyQualifiedName)
                .Element("class/map/element")
                .HasAttribute("column", "ElementColumn")
                .HasAttribute("type", typeof (MapContents).AssemblyQualifiedName);                                
        }



        [Test]
        public void CanSpecifyCollectionTypeAsArray()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ArrayOfChildren).AsArray(x => x.Name))
                .Element("class/array").Exists();
        }

        [Test]
        public void CanSpecifyForeignKeyColumnAsString()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.BagOfChildren).WithKeyColumn("ParentID"))
                .Element("class/bag/key")
                .HasAttribute("column", "ParentID");
        }

        [Test]
        public void CanSpecifyMultipleForeignKeyColumnsTogether()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .WithKeyColumns("ID1", "ID2")
                )
                .Element("class/bag/key/column").Exists()
                .Element("class/bag/key/column[@name='ID1']").Exists()
                .Element("class/bag/key/column[@name='ID2']").Exists();
        }

        [Test]
        public void CanStackForeignKeyColumns()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .WithKeyColumn("ID1")
                        .WithKeyColumn("ID2")
                )
                .Element("class/bag/key/column").Exists()
                .Element("class/bag/key/column[@name='ID1']").Exists()
                .Element("class/bag/key/column[@name='ID2']").Exists();
        }
        [Test]
        public void CanSpecifyForeignKeyName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).WithForeignKeyConstraintName("FK_TEST"))                    
                .Element("class/bag/key")
                .HasAttribute("foreign-key", "FK_TEST");
        }

        [Test]
        public void CanSpecifyIndexColumnAndTypeForList()
        {
            new MappingTester<OneToManyTarget>()
            .ForMapping(map => map.HasMany(x => x.ListOfChildren)
                .AsList(index => index
                                    .WithColumn("ListIndex")
                                    .WithType<int>()
                ))
                .Element("class/list/index")
                .HasAttribute("column", "ListIndex")
                .HasAttribute("type", typeof(int).AssemblyQualifiedName);
        }

        [Test]
        public void CanSetTableName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).WithTableName("MyTableName"))
                .Element("class/bag").HasAttribute("table", "MyTableName");
        }

        [Test]
        public void TableNameAttributeOnlyAddedWhenSet()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren))
                .Element("class/bag").DoesntHaveAttribute("table");
        }

        [Test]
        public void cascade_attribute_is_noneexistant_if_not_specified()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(c => c.HasMany(x => x.BagOfChildren))
                .Element("class/bag").DoesntHaveAttribute("cascade");
        }

        [Test]
        public void ListHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).AsList())
                .Element("class/list/index").Exists();
        }

        [Test]
        public void MapHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map/index").HasAttribute("type", typeof(string).AssemblyQualifiedName)
                .Element("class/map/index").HasAttribute("column", "Name");
        }

        [Test]
        public void ArrayHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ArrayOfChildren).AsArray(x => x.Position))
                .Element("class/array/index").Exists();
        }

        [Test]
        public virtual void CanOverrideMapIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(
                    x => x.Name,
                    index => index.WithColumn("ChildObjectName")))
                .Element("class/map/index").HasAttribute("column", "ChildObjectName");
        }

        [Test]
        public void OneToManyElementIsExcludedForComponents()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set/one-to-many").DoesntExist();
        }

        [Test]
        public void ShouldMapElementsOfCompositeElement()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set/composite-element/property[@name = 'Name']").Exists();
        }

        [Test]
        public void CanSetTableNameForCompositeElements()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                     .Component(c => c.Map(x => x.Name))
                                     .WithTableName("MyTableName"))
                .Element("class/set").HasAttribute("table", "MyTableName");
        }

        [Test]
        public void setting_the_cascade_to_something_other_than_none_updates_the_cascade_attribute()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(c => c.HasMany(x => x.BagOfChildren).Cascade.AllDeleteOrphan())
                .Element("class/bag")
                .HasAttribute("cascade", "all-delete-orphan");
        }

        [Test]
        public void SetsLazyLoadingAsDefault()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set").DoesntHaveAttribute("lazy");
        }

        [Test]
        public void SetsLazyLoadingOnThroughConvention()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OneToManyConvention = p => p.LazyLoad();

            new MappingTester<OneToManyComponentTarget>()
                .UsingVisitor(visitor)
                .ForMapping(m => m.HasMany(x => x.SetOfComponents).Component(c => c.Map(x => x.Name)))
                .Element("class/set").HasAttribute("lazy", "true");
        }


        [Test]
        public void SetsCascadeOffAsDefault()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set").DoesntHaveAttribute("cascade");
        }

        [Test]
        public void SetsCascadeOnThroughConvention()
        {
            var visitor = new MappingVisitor();
            visitor.Conventions.OneToManyConvention = p => p.Cascade.All();

            new MappingTester<OneToManyComponentTarget>()
                .UsingVisitor(visitor)
                .ForMapping(m => m.HasMany(x => x.SetOfComponents).Component(c => c.Map(x => x.Name)))
                .Element("class/set").HasAttribute("cascade", "all");
        }

        [Test] 
        public void CanSetAsElement() 
        { 
            new MappingTester<OneToManyTarget>() 
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).AsElement("columnName")) 
                .Element("class/bag/element").Exists(); 
        } 
 
        [Test] 
        public void ElementHasCorrectType() 
        { 
            new MappingTester<OneToManyTarget>() 
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).AsElement("columnName")) 
                .Element("class/bag/element").HasAttribute("type", typeof(string).AssemblyQualifiedName); 
        } 
 
        [Test] 
        public void ElementHasCorrectColumnName() 
        { 
            string columnName = "columnName"; 
 
            new MappingTester<OneToManyTarget>() 
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).AsElement(columnName)) 
                .Element("class/bag/element").HasAttribute("column", columnName); 
        }

        [Test]
        public void OneToManyMapping_with_private_backing_field()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                {
                    m.DefaultAccess.AsCamelCaseField();
                    m.HasMany(x => x.GetOtherChildren());
                })
                .HasAttribute("default-access", "field.camelcase")
                .Element("class/bag")
                .HasAttribute("name", "OtherChildren");
        }

        private class StaticExample
        {
            public static string SomeValue = "SomeValue";
        }

        [Test]
        public void Can_specify_where_fluently_with_static_class_member_reference()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Name == StaticExample.SomeValue)
                )
               .Element("class/bag").HasAttribute("where", "Name = 'SomeValue'");
        }

        const string someValue = "SomeValue";

        [Test]
        public void Can_specify_where_fluently_with_const()
        {

            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Name == someValue)
                )
               .Element("class/bag").HasAttribute("where", "Name = 'SomeValue'");
        }

        [Test]
        public void Can_specify_where_fluently_with_local_variable()
        {
            var local = "someValue";

            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Name == local)
                )
               .Element("class/bag").HasAttribute("where", "Name = 'someValue'");
        }

        [Test]
        public void Can_specify_where_fluently_int_equals_int()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Position == 1)
                )
               .Element("class/bag").HasAttribute("where", "Position = 1");
        }

        [Test]
        public void Can_specify_where_fluently_string_equals_string()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Name == "1")
                )
                .Element("class/bag").HasAttribute("where", "Name = '1'");
        }

        [Test]
        public void Can_specify_where_fluently_NOT_equals()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Name != "1")
                )
                .Element("class/bag").HasAttribute("where", "Name != '1'");
        }

        [Test]
        public void Can_specify_where_fluently_greater()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Position > 1)
                )
                .Element("class/bag").HasAttribute("where", "Position > 1");
        }

        [Test]
        public void Can_specify_where_fluently_less()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Position < 1)
                )
                .Element("class/bag").HasAttribute("where", "Position < 1");
        }

        public void Can_specify_where_fluently_greater_or_equal()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Position >= 1)
                )
                .Element("class/bag").HasAttribute("where", "Position >= 1");
        }

        [Test]
        public void Can_specify_where_fluently_less_or_equal()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where(x => x.Position <= 1)
                )
                .Element("class/bag").HasAttribute("where", "Position <= 1");
        }

        [Test]
        public void Can_specify_where_as_string()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.ListOfChildren)
                        .Where("some where clause")
                )
                .Element("class/bag").HasAttribute("where", "some where clause");
        }

        [Test]
        public void Can_use_custom_collection_implicitly()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.CustomCollection))
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly_generic()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .CollectionType<CustomCollection<ChildObject>>()
                )
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .CollectionType(typeof(CustomCollection<ChildObject>))
                )
                .Element("class/bag").HasAttribute("collection-type", typeof(CustomCollection<ChildObject>).AssemblyQualifiedName);
        }

        [Test]
        public void Can_use_custom_collection_explicitly_name()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .CollectionType("name")
                )
                .Element("class/bag").HasAttribute("collection-type", "name");
        }

        [Test]
        public void ExplicitIEnumerableCreatesBag()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany<ChildObject>(x => x.BagOfChildren))
                .Element("class/bag").Exists();
        }

        [Test]
        public void ExplicitIDictionaryCreatesMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany<string, ChildObject>(x => x.MapOfChildren)
                        .AsMap(x => x.Name))
                .Element("class/map").Exists();
        }

        [Test]
        public void ArraysShouldntHaveCollectionTypeDefined()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ArrayOfChildren))
                .Element("class/bag").DoesntHaveAttribute("collection-type");
        }

        [Test]
        public void CanMapToInternalListExposedAsArray()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListToArrayChild)
                                       .Access.AsCamelCaseField().AsList())
                .Element("class/list")
                .DoesntHaveAttribute("collection-type");
        }
    }
}
