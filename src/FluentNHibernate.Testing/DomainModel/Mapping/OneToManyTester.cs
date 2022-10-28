﻿using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    using System.Linq;

    public class SomeEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class SomeOtherEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class IndexTarget {}

    public class FieldOneToManyTarget
    {
        public IList<ChildObject> BagOfChildren;
    }

    public class OneToManyTarget
    {
        public virtual int Id { get; set; }
        public virtual ISet<ChildObject> SetOfChildren { get; set; }
        public virtual HashSet<ChildObject> HashSetOfChildren { get; set; }
        public virtual IList<ChildObject> BagOfChildren { get; set; }
        public virtual IList<ChildObject> ListOfChildren { get; set; }
        public virtual IDictionary<string, ChildObject> MapOfChildren { get; set; }
        public virtual ChildObject[] ArrayOfChildren { get; set; }
        public virtual IList<string> ListOfSimpleChildren { get; set; }
        public virtual CustomCollection<ChildObject> CustomCollection { get; set; }
        public virtual IDictionary<MapIndex, MapContents> MapOfEnums { get; set; }
        public virtual IDictionary<SomeEntity, ChildObject> EntityMapOfChildren { get; set; }
        public virtual IDictionary<SomeEntity, ValueObject> EntityMapOfComplexValues { get; set; }
        public virtual IDictionary<SomeEntity, string> EntityMapOfValues { get; set; }

        private IList<ChildObject> otherChildren = new List<ChildObject>();
        public virtual IList<ChildObject> GetOtherChildren() { return otherChildren; }

        private IList<ChildObject> listToArrayChild = new List<ChildObject>();
        public virtual ChildObject[] ListToArrayChild { get { return listToArrayChild.ToArray(); } }

    }

    public class ValueObject
    {
        public virtual string Name { get; set; }
        public virtual int Position { get; set; }
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

    public class SortComparer : IComparer<ChildObject>
    {
        public int Compare(ChildObject x, ChildObject y)
        {
            return 0;
        }
    }

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
                .Element("class/set").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsNaturallySortedSet()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.SetOfChildren).AsSet(SortType.Natural))
                .Element("class/set").Exists().HasAttribute("sort", "natural");
        }

        [Test]
        public void CanSpecifyCollectionTypeAsComparerSortedSet()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.SetOfChildren).AsSet<SortComparer>())
                .Element("class/set").Exists().HasAttribute("sort", typeof(SortComparer).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCollectionTypeAsComparerSortedSet_CfgVerified()
        {
            var cfg = new Configuration();
            var model = new PersistenceModel();
            var classMap = new ClassMap<OneToManyTarget>();

            CreateStandardInMemoryConfiguration()
                .ConfigureProperties(cfg);

            classMap.Id(x => x.Id);
            classMap.HasMany(x => x.SetOfChildren).AsSet<SortComparer>();

            model.Add(classMap);
            model.Configure(cfg);
        }

        [Test]
        public void CanSpecifyCollectionTypeAsMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map").Exists();
        }

        [Test]
        public void CanSpecifyCollectionTypeAsMapWithStringColumnName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.MapOfChildren)
                        .AsMap("Name")
                        .KeyColumns.Add("ParentId"))
                .Element("class/map/key/column").HasAttribute("name", "ParentId")
                .Element("class/map/index/column").HasAttribute("name", "Name")
                .Element("class/map/one-to-many").HasAttribute("class", typeof(ChildObject).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCollectionTypeAsMapOfEnums()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfEnums)
                                       .AsMap<MapIndex>(
                                           index => index.Column("IndexColumn").Type<MapIndex>(),
                                           element => element.Column("ElementColumn").Type<MapContents>()))
                .Element("class/map/index").HasAttribute("type", typeof(MapIndex).AssemblyQualifiedName)
                .Element("class/map/index/column").HasAttribute("name", "IndexColumn")
                .Element("class/map/element").HasAttribute("type", typeof(MapContents).AssemblyQualifiedName)
                .Element("class/map/element/column").HasAttribute("name", "ElementColumn");
        }

        [Test]
        public void CanSpecifyCollectionTypeAsNaturallySortedMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap("Name", SortType.Natural))
                .Element("class/map").Exists().HasAttribute("sort", "natural");
        }

        [Test]
        public void CanSpecifyCollectionTypeAsNaturallySortedMapAlsoWhenUsingIndexSelector()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(x => x.Name, SortType.Natural))
                .Element("class/map").Exists().HasAttribute("sort", "natural");
        }

        [Test]
        public void CanSpecifyCollectionTypeAsComparerSortedMap()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap<MapIndex, SortComparer>("Name"))
                .Element("class/map").Exists().HasAttribute("sort", typeof(SortComparer).AssemblyQualifiedName);
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
                .ForMapping(map => map.HasMany(x => x.BagOfChildren).KeyColumns.Add("ParentID"))
                .Element("class/bag/key/column").HasAttribute("name", "ParentID");
        }

        [Test]
        public void CanSpecifyMultipleForeignKeyColumnsTogether()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .KeyColumns.Add("ID1", "ID2")
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
                        .KeyColumns.Add("ID1")
                        .KeyColumns.Add("ID2")
                )
                .Element("class/bag/key/column").Exists()
                .Element("class/bag/key/column[@name='ID1']").Exists()
                .Element("class/bag/key/column[@name='ID2']").Exists();
        }
        [Test]
        public void CanSpecifyForeignKeyName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).ForeignKeyConstraintName("FK_TEST"))
                .Element("class/bag/key")
                .HasAttribute("foreign-key", "FK_TEST");
        }

        [Test]
        public void CanSpecifyIndexColumnAndTypeForList()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren)
                    .AsList(index =>
                    {
                        index.Column("ListIndex");
                        index.Type<int>();
                    }))
                .Element("class/list/index").HasAttribute("type", typeof(int).AssemblyQualifiedName)
                .Element("class/list/index/column").HasAttribute("name", "ListIndex");
        }

        [Test]
        public void CanSpecifyIndexBaseOffsetAndColumnForList()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren)
                    .AsList(index =>
                    {
                        index.Offset(1);
                        index.Column("ListIndex");
                    }))
                .Element("class/list/list-index").HasAttribute("base", "1")
                .Element("class/list/list-index/column").HasAttribute("name", "ListIndex");
        }

        [Test]
        public void SpecifyingIndexBaseOffsetAndTypeForListThrowsNotSupportedException()
        {
            Assert.That(() =>
                new MappingTester<OneToManyTarget>()
                    .ForMapping(map => map.HasMany(x => x.ListOfChildren)
                        .AsList(index =>
                        {
                            index.Offset(1);
                            index.Type("ListIndex");
                        })),
                Throws.TypeOf<NotSupportedException>());
        }

        [Test]
        public void CanSetTableName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.ListOfChildren).Table("MyTableName"))
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
        public void MapHasIndexElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfChildren).AsMap(x => x.Name))
                .Element("class/map/index").HasAttribute("type", typeof(string).AssemblyQualifiedName)
                .Element("class/map/index/column").HasAttribute("name", "Name");
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
                    index => index.Column("ChildObjectName")))
                .Element("class/map/index/column").HasAttribute("name", "ChildObjectName");
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
                                     .Table("MyTableName"))
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
        public void SetsCascadeOffAsDefault()
        {
            new MappingTester<OneToManyComponentTarget>()
                .ForMapping(m => m.HasMany(x => x.SetOfComponents)
                                    .Component(c => c.Map(x => x.Name)))
                .Element("class/set").DoesntHaveAttribute("cascade");
        }

        [Test]
        public void CanSetAsElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).Element("columnName"))
                .Element("class/bag/element").Exists();
        }

        [Test]
        public void ElementHasCorrectType()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).Element("columnName"))
                .Element("class/bag/element").HasAttribute("type", typeof(string).AssemblyQualifiedName);
        }

        [Test]
        public void ElementHasCorrectColumnName()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).Element("columnName"))
                .Element("class/bag/element/column").HasAttribute("name", "columnName");
        }

        [Test]
        public void ElementCallsCustomMappingHandler()
        {
            var called = false;
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.ListOfSimpleChildren).Element("columnName", elementPart => called = true));
            called.ShouldBeTrue();
        }

        [Test]
        public void OneToManyMapping_with_private_backing_field()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m =>
                    m.HasMany(x => x.GetOtherChildren())
                        .Access.CamelCaseField())
                .Element("class/bag")
                .HasAttribute("name", "otherChildren")
                .HasAttribute("access", "field.camelcase");
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
                                       .Access.CamelCaseField().AsList())
                .Element("class/list")
                .DoesntHaveAttribute("collection-type");
        }

        [Test]
        public void NotFound_sets_attribute()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                    map.HasMany(x => x.BagOfChildren)
                        .NotFound.Ignore())
                .Element("class/bag/one-to-many").HasAttribute("not-found", "ignore");
        }

        [Test]
        public void ShouldWriteCacheElementWhenAssigned()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.SetOfChildren)
                                .Cache.ReadWrite())
                .Element("class/set/cache").ShouldNotBeNull();
        }

        [Test]
        public void ShouldNotWriteCacheElementWhenEmpty()
        {
            new MappingTester<ManyToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.SetOfChildren))
                .Element("class/set/cache").DoesntExist();
        }

        [Test]
        public void ShouldWriteCacheElementFirst()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.SetOfChildren)
                                .Cache.ReadWrite())
                .Element("class/set/cache").ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void ShouldWriteBatchSizeAttributeWhenAssigned()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.SetOfChildren)
                                .BatchSize(15))
                .Element("class/set").HasAttribute("batch-size", "15");
        }

        [Test]
        public void ShouldNotWriteBatchSizeAttributeWhenEmpty()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.SetOfChildren))
                .Element("class/set").DoesntHaveAttribute("batch-size");
        }

        [Test]
        public void CanSetForeignKeyToCascadeDeletes()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.BagOfChildren)
                                .ForeignKeyCascadeOnDelete())
                .Element("class/bag/key").HasAttribute("on-delete", "cascade");
        }

        [Test]
        public void IndexTypeDefaultsToInt32()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map =>
                            map.HasMany(x => x.MapOfChildren)
                                .AsMap("indexCol"))
                .Element("class/map/index").HasAttribute("type", typeof(Int32).AssemblyQualifiedName);
        }

        [Test]
        public void EntityMapOfEntitiesShouldWriteKeyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfChildren)
                    .AsEntityMap())
                .Element("class/map/key").ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void EntityMapOfEntitiesShouldWriteIndexManyToManyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfChildren)
                    .AsEntityMap())
                .Element("class/map/index-many-to-many").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void EntityMapOfEntitiesShouldWriteOneToManyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfChildren)
                    .AsEntityMap())
                .Element("class/map/one-to-many").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void EntityMapOfEntitiesShouldWriteCorrectColumnNames()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfChildren)
                    .AsEntityMap())
                .Element("class/map/key/column").HasAttribute("name", "OneToManyTarget_id")
                .Element("class/map/index-many-to-many/column").HasAttribute("name", "SomeEntity_id");
        }

        [Test]
        public void EntityMapOfComplexValuesShouldWriteKeyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfComplexValues)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Position);
                    })
                    .AsEntityMap())
                .Element("class/map/key").ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void EntityMapOfComplexValuesShouldWriteIndexManyToManyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfComplexValues)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Position);
                    })
                    .AsEntityMap())
                .Element("class/map/index-many-to-many").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void EntityMapOfComplexValuesShouldWriteCompositeElementElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfComplexValues)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Position);
                    })
                    .AsEntityMap())
                .Element("class/map/composite-element").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void EntityMapOfComplexValuesShouldWriteCorrectColumnNames()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfComplexValues)
                    .Component(c =>
                    {
                        c.Map(x => x.Name);
                        c.Map(x => x.Position);
                    })
                    .AsEntityMap())
                .Element("class/map/key/column").HasAttribute("name", "OneToManyTarget_id")
                .Element("class/map/index-many-to-many/column").HasAttribute("name", "SomeEntity_id");
        }

        [Test]
        public void EntityMapOfValuesShouldWriteKeyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfValues)
                    .Element("StringValue")
                    .AsEntityMap())
                .Element("class/map/key").ShouldBeInParentAtPosition(0);
        }

        [Test]
        public void EntityMapOfValuesShouldWriteIndexManyToManyElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfValues)
                    .Element("StringValue")
                    .AsEntityMap())
                .Element("class/map/index-many-to-many").ShouldBeInParentAtPosition(1);
        }

        [Test]
        public void EntityMapOfValuesShouldWriteCompositeElementElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfValues)
                    .Element("StringValue")
                    .AsEntityMap())
                .Element("class/map/element").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void EntityMapOfValuesShouldWriteCorrectColumnNames()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfValues)
                    .Element("StringValue")
                    .AsEntityMap())
                .Element("class/map/key/column").HasAttribute("name", "OneToManyTarget_id")
                .Element("class/map/index-many-to-many/column").HasAttribute("name", "SomeEntity_id");
        }

        class Container
        {
            public virtual Containee Containee { get; set; }
        }

        class Containee
        {
            public virtual IDictionary<ChildObject, ValueObject> EntityMapOfValueObjects { get; set; }
        }

        [Test]
        public void WhenEntityMapIsDefinedInValueObjectWeUsedToHaveBugBecauseXmlClasslikeNodeSorterDidntSortChildrenOfComponentButItsFixedNow()
        {
            new MappingTester<Container>()
                .ForMapping(map => map
                    .Component(x => x.Containee, containee => containee
                        .HasMany(y => y.EntityMapOfValueObjects)
                        .Component(child =>
                        {
                            child.Map(x => x.Name);
                            child.Map(x => x.Position);
                        })
                        .AsEntityMap()
                    )
                )
                .Element("class/component/map").ShouldBeInParentAtPosition(0)
                .Element("class/component/map/key").ShouldBeInParentAtPosition(0)
                .Element("class/component/map/index-many-to-many").ShouldBeInParentAtPosition(1)
                .Element("class/component/map/composite-element").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void WhenEntityMapIsDefinedInEntityEverythingWorks()
        {
            new MappingTester<Containee>()
                .ForMapping(map => map
                    .HasMany(x => x.EntityMapOfValueObjects)
                    .Component(child =>
                    {
                        child.Map(x => x.Name);
                        child.Map(x => x.Position);
                    })
                    .AsEntityMap()
                )
                .Element("class/map").ShouldBeInParentAtPosition(0)
                .Element("class/map/key").ShouldBeInParentAtPosition(0)
                .Element("class/map/index-many-to-many").ShouldBeInParentAtPosition(1)
                .Element("class/map/composite-element").ShouldBeInParentAtPosition(2);
        }

        [Test]
        public void CanSpecifyOrderByClause()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.BagOfChildren).OrderBy("foo"))
                .Element("class/bag").HasAttribute("order-by", "foo");
        }

        [Test]
        public void CanSpecifyOrderByClauseExpression()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.BagOfChildren).OrderBy(c => c.Name))
                .Element("class/bag").HasAttribute("order-by", "Name");
        }

        [Test]
        public void OrderByClauseIgnoredForUnorderableCollections()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.MapOfChildren).AsMap("indexCol"))
                .Element("class/map").DoesntHaveAttribute("order-by");
        }

        [Test]
        public void CanSpecifyUniqueKey()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.MapOfChildren).KeyColumns.Add("key_col", c => c.Unique()))
                .Element("class/bag/key/column").HasAttribute("unique", "true");
        }


        [Test]
        public void CanSpecifySqlTypeOnElementColumn()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(map => map.HasMany(x => x.MapOfEnums)
                                       .AsMap<MapIndex>(
                                           index => index.Column("IndexColumn"),
                                           element => element.Columns.Add("elementColumn", c => c.SqlType("ntext"))))
                .Element("class/map/element/column").HasAttribute("sql-type", "ntext");
        }

        [Test]
        public void CanSpecifyAdditionalColumnWithSqlTypeOnElement()
        {
            new MappingTester<OneToManyTarget>()
                .ForMapping(m => m.HasMany(x => x.MapOfChildren)
                    .Element("colName", e => e.Columns.Add("additionalColumn", c => c.SqlType("ntext"))))
                .Element("class/bag/element/column[@name='colName']").DoesntHaveAttribute("sql-type")
                .Element("class/bag/element/column[@name='additionalColumn']").HasAttribute("sql-type", "ntext");
        }

    }
}
