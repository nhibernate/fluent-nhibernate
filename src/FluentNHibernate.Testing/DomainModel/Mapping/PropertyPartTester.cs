﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FluentNHibernate.Mapping;
using NHibernate.Engine;
using NHibernate.Properties;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class PropertyPartTester
    {
        [Test]
        public void MapWithoutColumnNameUsesPropertyNameForPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name);
                })
                .Element("class/property[@name='Name']/column")
                    .HasAttribute("name", "Name");
        }

        [Test]
        public void MapWithoutColumnNameUsesPropertyNameForColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name);
                })
                .Element("class/property/column").HasAttribute("name", "Name");
        }

        [Test]
        public void MapWithColumnNameUsesColumnNameForPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name, "column_name");
                })
                .Element("class/property[@name='Name']/column")
                    .HasAttribute("name", "column_name");
        }

        [Test]
        public void MapWithColumnNameUsesColumnNameForColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name, "column_name");
                })
                .Element("class/property/column").HasAttribute("name", "column_name");
        }

        [Test]
        public void MapWithFluentColumnNameUsesColumnNameForColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Column("column_name");
                })
                .Element("class/property/column").HasAttribute("name", "column_name");
        }

        static MappingTester<T> Model<T>(Action<ClassMap<T>> mapping)
        {
            return new MappingTester<T>()
                .ForMapping(mapping);
        }

        [Test]
        public void ShouldAddOnlyOneColumnWhenNotSpecified()
        {
            Model<PropertyTarget>(m =>
            {
                m.Id(x => x.Id);
                m.Map(x => x.Name);
            })
                .Element("class/property[@name='Name']").HasThisManyChildNodes(1);
        }

        [Test]
        public void ShouldAddAllColumns()
        {
            Model<PropertyTarget>(m =>
            {
                m.Id(x => x.Id);
                m.Map(x => x.Name).Columns.Add("one", "two", "three");
            })
                .Element("class/property[@name='Name']").HasThisManyChildNodes(3)
                .Element("class/property[@name='Name']/column[@name='one']").Exists()
                .Element("class/property[@name='Name']/column[@name='two']").Exists()
                .Element("class/property[@name='Name']/column[@name='three']").Exists();
        }

        [Test]
        public void ColumnNameIsPropertyNameWhenNoColumnNameGiven()
        {
            Model<PropertyTarget>(m =>
            {
                m.Id(x => x.Id);
                m.Map(x => x.Name);
            })
                .Element("class/property[@name='Name']/column")
                .HasAttribute("name", "Name");
        }

        [Test]
        public void ColumnNameIsColumnNameWhenColumnNameGiven()
        {
            Model<PropertyTarget>(m =>
            {
                m.Id(x => x.Id);
                m.Map(x => x.Name, "column_name");
            })
                .Element("class/property[@name='Name']/column")
                .HasAttribute("name", "column_name");
        }

        [Test]
        public void ColumnNameIsColumnNameWhenColumnNameFluentGiven()
        {
            Model<PropertyTarget>(m =>
            {
                m.Id(x => x.Id);
                m.Map(x => x.Name).Columns.Add("column_name");
            })
                .Element("class/property[@name='Name']/column")
                .HasAttribute("name", "column_name");
        }

        [Test]
        public void MapWithFluentLengthOnStringUsesWithLengthOfPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Length(20);
                })
                .Element("class/property[@name='Name']/column").HasAttribute("length", "20");
        }

        [Test]
        public void MapWithFluentLengthOnDecimalUsesWithLengthOfPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.DecimalProperty).Length(1);
                })
                .Element("class/property[@name='DecimalProperty']/column").HasAttribute("length", "1");
        }

        [Test]
        public void MapWithFluentLengthAllowOnAnythingPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Id).Length(20);
                })
                .Element("class/property[@name='Id']/column").HasAttribute("length", "20");
        }

        [Test]
        public void MapUsesCanNotBeNullPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Not.Nullable();
                })
                .Element("class/property[@name='Name']").DoesntHaveAttribute("not-null")
                .Element("class/property[@name='Name']/column").HasAttribute("not-null", "true");
        }

        [Test]
        public void MapUsesAsReadOnlyPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).ReadOnly();
                })
                .Element("class/property")
                    .HasAttribute("insert", "false")
                    .HasAttribute("update", "false");
        }

        [Test]
        public void MapUsesUniqueKeyPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).UniqueKey("uniquekey");
                })
                .Element("class/property/column")
                    .HasAttribute("unique-key", "uniquekey");
        }

        [Test]
        public void MapUsesNotReadOnlyPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Not.ReadOnly();
                })
                .Element("class/property")
                    .HasAttribute("insert", "true")
                    .HasAttribute("update", "true");
        }

        [Test]
        public void MapUsesPrecisionPropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Precision(2);
                })
                .Element("class/property/column").HasAttribute("precision", "2");
        }

        [Test]
        public void MapUsesScalePropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Scale(3);
                })
                .Element("class/property/column").HasAttribute("scale", "3");
        }

        [Test]
        public void MapWithFluentFormulaUsesFormula()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Formula("foo(bar)");
                })
                .Element("class/property").HasAttribute("formula", "foo(bar)")
                .Element("class/property/column").DoesntExist();
        }

        [Test]
        public void MapWithLazyLoadUsesLazyAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).LazyLoad();
                })
                .Element("class/property").HasAttribute("lazy", "true");
        }

        [Test]
        public void MapWithIndexUsesIndexAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Index("name_index");
                })
                .Element("class/property/column[@name='Name']").HasAttribute("index", "name_index");
        }

        [Test]
        public void CanSpecifyCustomTypeAsDotNetTypeGenerically()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Data).CustomType<CustomTypeForTesting>();
                })
                .Element("class/property").HasAttribute("type", typeof(CustomTypeForTesting).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCustomTypeAsDotNetTypeViaFunctionFromPropertyType()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Data).CustomType(t => typeof(CustomGenericTypeForTesting<>).MakeGenericType(t));
                })
                .Element("class/property").HasAttribute("type", typeof(CustomGenericTypeForTesting<byte[]>).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCustomTypeAsDotNetType()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Data).CustomType(typeof(CustomTypeForTesting));
                })
                .Element("class/property").HasAttribute("type", typeof(CustomTypeForTesting).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCustomTypeAsString()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Data).CustomType("name");
                })
                .Element("class/property").HasAttribute("type", "name");
        }

        [Test]
        public void CanSpecifyCustomSqlType()
        {
            var classMap = new ClassMap<PropertyTarget>();
            classMap.Id(x => x.Id);
            classMap.Map(x => x.Data)
                .CustomSqlType("image");

            new MappingTester<PropertyTarget>()
                .ForMapping(classMap)
                .Element("class/property/column").HasAttribute("sql-type", "image");
        }

        [Test]
        public void CanSetAsUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Unique();
                })
                .Element("class/property").DoesntHaveAttribute("unique")
                .Element("class/property/column").HasAttribute("unique", "true");
        }

        [Test]
        public void CanSetAsNotUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Not.Unique();
                })
                .Element("class/property").DoesntHaveAttribute("unique")
                .Element("class/property/column").HasAttribute("unique", "false");
        }

        [Test]
        public void CanSpecifyDefault()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Default("SomeName");
                })
                .Element("class/property").DoesntHaveAttribute("default")
                .Element("class/property/column").HasAttribute("default", "SomeName");
        }

        [Test]
        public void CanSpecifyInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Insert();
                })
                .Element("class/property").HasAttribute("insert", "true");
        }

        [Test]
        public void CanSpecifyNotInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Not.Insert();
                })
                .Element("class/property").HasAttribute("insert", "false");
        }

        [Test]
        public void CanSpecifyUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Update();
                })
                .Element("class/property").HasAttribute("update", "true");
        }

        [Test]
        public void CanSpecifyNotUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Name).Not.Update();
                })
                .Element("class/property").HasAttribute("update", "false");
        }

        [Test]
        public void MapNestedGeneric()
        {
            new MappingTester<OuterTarget<String>.NestedTarget>()
                .ForMapping(m =>
                {
                    m.Id(x => x.Id);
                    m.Map(x => x.Value);
                })
                .Element("class").HasAttribute("table", "`NestedTarget_String`");
        }

        #region Custom IUserType impl for testing
        public class CustomTypeForTesting : IUserType
        {
            bool IUserType.Equals(object x, object y)
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
                get { return null; }
            }

            public bool IsMutable
            {
                get { return false; }
            }
        }

        public class CustomGenericTypeForTesting<T> : IUserType
        {
            bool IUserType.Equals(object x, object y)
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
                return null;
            }

            public object Replace(object original, object target, object owner)
            {
                return null;
            }

            public object Assemble(object cached, object owner)
            {
                return null;
            }

            public object Disassemble(object value)
            {
                return null;
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
                get { return new SqlType[] {}; }
            }
            public Type ReturnedType
            {
                get { return null; }
            }
            public bool IsMutable
            {
                get { return false; }
            }
        }
        #endregion
    }

    public class PropertyTarget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PropertyReferenceTarget Reference { get; set; }
        public IList<PropertyReferenceTarget> References { get; set; }
        public ComponentTarget Component { get; set; }
        public IList<ComponentTarget> Components { get; set; }
        public byte[] Data { get; set; }
        public decimal DecimalProperty { get; set; }
        public IDictionary ExtensionData { get; set; }
        public IDictionary<string, object> GenericExtensionData { get; set; }
    }

    public class FieldTarget
    {
        public int Id;
        public string Name;
        public PropertyReferenceTarget Reference;
        public IList<PropertyReferenceTarget> References;
        public ComponentTarget Component;
        public IList<ComponentTarget> Components;
        public byte[] Data;
        public decimal DecimalProperty;
        public IDictionary ExtensionData;
        public IDictionary<string, object> GenericExtensionData;
    }

    public class PrivatePropertyTarget
    {
        public int Id { get; set; }
        private string Name { get; set; }
        public PropertyReferenceTarget Reference { get; set; }
        public IList<PropertyReferenceTarget> References { get; set; }
        public ComponentTarget Component { get; set; }
        public IList<ComponentTarget> Components { get; set; }
        public byte[] Data { get; set; }
        public decimal DecimalProperty { get; set; }
        public IDictionary ExtensionData { get; set; }
        public IDictionary<string, object> GenericExtensionData { get; set; }
    }

    public class PropertyReferenceTarget
    {
        public string Name { get; set; }
    }

    public class PropertyReferenceTargetProxy
    {

    }

    public class ComponentTarget
    {
        public PropertyTarget MyParent { get; set; }
        public object Name { get; set; }
    }

    class OuterTarget<T>
    {
        public class NestedTarget
        {
            public virtual int Id { get; set; }
            public virtual T Value { get; set; }
        }
    }

    public class FakePropertyAccessor : IPropertyAccessor
    {
        public IGetter GetGetter(Type theClass, string propertyName)
        {
            return null;
        }

        public ISetter GetSetter(Type theClass, string propertyName)
        {
            return null;
        }

        public bool CanAccessThroughReflectionOptimizer
        {
            get { return false; }
        }
    }
}
