using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class RunnableConventionsTests
    {
        [Test]
        public void ShouldApplyIArrayConvention()
        {
            TestConvention(new ArrayConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Array)
                    .AsArray(x => x.Id);

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIBagConvention()
        {
            TestConvention(new BagConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Bag)
                    .AsBag();

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIClassConvention()
        {
            TestConvention(new ClassConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);

                return map;
            })
                .BatchSize.ShouldEqual(10);
        }

        [Test]
        public void ShouldApplyICollectionConventionToHasManys()
        {
            TestConvention(new CollectionConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Bag);

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyICollectionConventionToHasManyToManys()
        {
            TestConvention(new CollectionConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasManyToMany(x => x.Bag);

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIColumnConvention()
        {
            TestConvention(new ColumnConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Map(x => x.Property);

                return map;
            })
                .Properties.First()
                .Columns.First()
                .Length.ShouldEqual(100);
        }

        [Test]
        public void ShouldApplyIComponentConvention()
        {
            TestConvention(new ComponentConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Component(x => x.Component, c => { });

                return map;
            })
                .Components.First()
                .Unique.ShouldBeTrue();
        }

        [Test]
        public void ShouldApplyIDynamicComponentConvention()
        {
            TestConvention(new DynamicComponentConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.DynamicComponent(x => x.DynamicComponent, c => { });

                return map;
            })
                .Components.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyGenericIDynamicComponentConvention()
        {
            TestConvention(new DynamicComponentConvention(), () =>
                {
                    var map = new ClassMap<Target>();

                    map.Id(x => x.Id);
                    map.DynamicComponent(x => x.GenericDynamicComponent, c => { });

                    return map;
                })
                .Components.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIHasManyConvention()
        {
            TestConvention(new HasManyConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Bag);

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIHasManyToManyConvention()
        {
            TestConvention(new HasManyToManyConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasManyToMany(x => x.Bag);

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIHasOneConvention()
        {
            TestConvention(new HasOneConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasOne(x => x.Other);

                return map;
            })
                .OneToOnes.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIIdConvention()
        {
            var @class = TestConvention(new IdConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);

                return map;
            });

            ((IdMapping)@class.Id)
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIIndexConvention()
        {
            var collection = TestConvention(new IndexConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Array)
                    .AsArray(x => x.Id);

                return map;
            })
                .Collections.First();

            collection.Index
                .Columns.First().Name.ShouldEqual("test");
        }

        [Test]
        public void ShouldApplyIIndexManyToManyConvention()
        {
            var collection = TestConvention(new IndexManyToManyConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasManyToMany(x => x.DictionaryBag)
                    .AsMap("index")
                    .AsTernaryAssociation("index", "value");

                return map;
            })
                .Collections.First();

            ((IndexManyToManyMapping)collection.Index).ForeignKey.ShouldEqual("fk");
        }

        [Test]
        public void ShouldApplyIJoinConvention()
        {
            TestConvention(new JoinConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Join("second", m => { });

                return map;
            })
                .Joins.First()
                .Schema.ShouldEqual("dto");
        }

// ignoring warning for JoinedSubClass
#pragma warning disable 612,618

        [Test]
        public void ShouldApplyIJoinedSubclassConvention()
        {
            var subclass = TestConvention(new JoinedSubclassConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.JoinedSubClass<TargetSubclass>("key", m => { });

                return map;
            })
                .Subclasses.First();

            subclass
                .TableName.ShouldEqual("tbl");
        }

#pragma warning restore 612,618

        [Test]
        public void ShouldApplyIListConvention()
        {
            TestConvention(new ListConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Array)
                    .AsList();

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIMapConvention()
        {
            TestConvention(new MapConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.DictionaryBag)
                    .AsMap("index");

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIPropertyConvention()
        {
            TestConvention(new PropertyConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Map(x => x.Property);

                return map;
            })
                .Properties.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIReferenceConvention()
        {
            TestConvention(new ReferenceConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.References(x => x.Other);

                return map;
            })
                .References.First()
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyISetConvention()
        {
            TestConvention(new SetConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.HasMany(x => x.Array)
                    .AsSet();

                return map;
            })
                .Collections.First()
                .Access.ShouldEqual("field");
        }

// ignoring warning for obsolete SubClass
#pragma warning disable 612,618

        [Test]
        public void ShouldApplyISubclassConvention()
        {
            var subclass = TestConvention(new SubclassConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.DiscriminateSubClassesOnColumn("column")
                    .SubClass<TargetSubclass>(m => { });

                return map;
            })
                .Subclasses.First();

            ((SubclassMapping)subclass)
                .DiscriminatorValue.ShouldEqual("xxx");
        }

#pragma warning restore 612,618

        [Test]
        public void ShouldApplyIUserTypeConvention()
        {
            TestConvention(new OtherObjectUserTypeConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Map(x => x.Other);

                return map;
            })
                .Properties.First()
                .Type.GetUnderlyingSystemType().ShouldEqual(typeof(OtherObjectUserType));
        }

        [Test]
        public void ShouldApplyIVersionConvention()
        {
            TestConvention(new VersionConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);
                map.Version(x => x.Property);

                return map;
            })
                .Version
                .Access.ShouldEqual("field");
        }

        [Test]
        public void ShouldApplyIHibernateMappingConvention()
        {
            TestConvention(new HibernateMappingConvention(), () =>
            {
                var map = new ClassMap<Target>();

                map.Id(x => x.Id);

                return map;
            })
                .DefaultLazy.ShouldBeFalse();
        }

        #region conventions

#pragma warning disable 612,618
        private class ArrayConvention : IArrayConvention
        {
            public void Apply(IArrayInstance instance)
            {
                instance.Access.Field();
            }
        }

        private class BagConvention : IBagConvention
        {
            public void Apply(IBagInstance instance)
            {
                instance.Access.Field();
            }
        }

        private class ClassConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.BatchSize(10);
            }
        }

        private class CollectionConvention : ICollectionConvention
        {
            public void Apply(ICollectionInstance instance)
            {
                instance.Access.Field();
            }
        }

        private class ColumnConvention : IColumnConvention
        {
            public void Apply(IColumnInstance instance)
            {
                instance.Length(100);
            }
        }

        private class ComponentConvention : IComponentConvention
        {
            public void Apply(IComponentInstance instance)
            {
                instance.Unique();
            }
        }

        private class DynamicComponentConvention : IDynamicComponentConvention
        {
            public void Apply(IDynamicComponentInstance instance)
            {
                instance.Access.Field();
            }
        }

        private class HasManyConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Access.Field();
            }
        }

        private class HasManyToManyConvention : IHasManyToManyConvention
        {
            public void Apply(IManyToManyCollectionInstance instance)
            {
                instance.Access.Field();
            }
        }

        class HasOneConvention : IHasOneConvention
        {
            public void Apply(IOneToOneInstance instance)
            {
                instance.Access.Field();
            }
        }

        class IdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Access.Field();
            }
        }

        class IndexConvention : IIndexConvention
        {
            public void Apply(IIndexInstance instance)
            {
                instance.Column("test");
            }
        }

        class IndexManyToManyConvention : IIndexManyToManyConvention
        {
            public void Apply(IIndexManyToManyInstance instance)
            {
                instance.ForeignKey("fk");
            }
        }

        class JoinConvention : IJoinConvention
        {
            public void Apply(IJoinInstance instance)
            {
                instance.Schema("dto");
            }
        }

        class JoinedSubclassConvention : IJoinedSubclassConvention
        {
            public void Apply(IJoinedSubclassInstance instance)
            {
                instance.Table("tbl");
            }
        }

        class ListConvention : IListConvention
        {
            public void Apply(IListInstance instance)
            {
                instance.Access.Field();
            }
        }

        class MapConvention : IMapConvention
        {
            public void Apply(IMapInstance instance)
            {
                instance.Access.Field();
            }
        }

        class PropertyConvention : IPropertyConvention
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.Access.Field();
            }
        }

        class ReferenceConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Access.Field();
            }
        }

        class SetConvention : ISetConvention
        {
            public void Apply(ISetInstance instance)
            {
                instance.Access.Field();
            }
        }

        class SubclassConvention : ISubclassConvention
        {
            public void Apply(ISubclassInstance instance)
            {
                instance.DiscriminatorValue("xxx");
            }
        }

        class OtherObjectUserTypeConvention : UserTypeConvention<OtherObjectUserType>
        {

        }

        class VersionConvention : IVersionConvention
        {
            public void Apply(IVersionInstance instance)
            {
                instance.Access.Field();
            }
        }

        class HibernateMappingConvention : IHibernateMappingConvention
        {
            public void Apply(IHibernateMappingInstance instance)
            {
                instance.Not.DefaultLazy();
            }
        }

#pragma warning restore 612,618

        private class OtherObjectUserType : IUserType
        {
            public OtherObjectUserType()
            {
            }

            public new bool Equals(object x, object y)
            {
                return false;
            }

            public int GetHashCode(object x)
            {
                return 0;
            }

            public object NullSafeGet(IDataReader rs, string[] names, object owner)
            {
                return null;
            }

            public void NullSafeSet(IDbCommand cmd, object value, int index)
            {}

            public object DeepCopy(object value)
            {
                return value;
            }

            public object Replace(object original, object target, object owner)
            {
                return original;
            }

            public object Assemble(object cached, object owner)
            {
                return cached;
            }

            public object Disassemble(object value)
            {
                return value;
            }

            public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
            {
                throw new NotImplementedException();
            }

            public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
            {
                throw new NotImplementedException();
            }

            public SqlType[] SqlTypes
            {
                get { return null; }
            }
            public Type ReturnedType
            {
                get { return typeof(OtherObject); }
            }
            public bool IsMutable
            {
                get { return false; }
            }
        }

        #endregion

        private ClassMapping TestConvention<T>(T convention, Func<IMappingProvider> getMapping) where T : IConvention
        {
            var model = new PersistenceModel();

            model.Conventions.Add(convention);
            model.Add(getMapping());

            return model.BuildMappings()
                .First()
                .Classes.First();
        }

        private HibernateMapping TestConvention(HibernateMappingConvention convention, Func<IMappingProvider> getMapping)
        {
            var model = new PersistenceModel();

            model.Conventions.Add(convention);
            model.Add(getMapping());

            return model.BuildMappings()
                .First();
        }

        private class Target
        {
            public OtherObject[] Array { get; set; }
            public IList<OtherObject> Bag { get; set; }
            public string Property { get; set; }
            public OtherObject Component { get; set; }
            public IDictionary DynamicComponent { get; set; }
            public IDictionary<string, object> GenericDynamicComponent { get; set; }
            public OtherObject Other { get; set; }
            public int Id { get; set; }
            public IDictionary<string, OtherObject> DictionaryBag { get; set; }
        }

        private class TargetSubclass : Target
        {}

        private class OtherObject
        {
            public int Id { get; set; }
        }
    }
}