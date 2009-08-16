using System;
using System.Data;
using FluentNHibernate.Mapping;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ComponentPropertyMapTester
    {
        [Test]
        public void Map_WithoutColumnName_UsesPropertyNameFor_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name)))
                .Element("//property[@name='Name']/column")
                .HasAttribute("name", "Name");
        }

        [Test]
        public void Map_WithoutColumnName_UsesPropertyNameFor_ColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name)))
                .Element("//property/column").HasAttribute("name", "Name");
        }


        [Test]
        public void Map_WithColumnName_UsesColumnNameFor_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name, "column_name")))
                .Element("//property[@name='Name']/column")
                .HasAttribute("name", "column_name");
        }

        [Test]
        public void Map_WithColumnName_UsesColumnNameFor_ColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name, "column_name")))
                .Element("//property/column").HasAttribute("name", "column_name");
        }

        [Test]
        public void Map_WithFluentColumnName_UsesColumnNameFor_ColumnNameAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Column("column_name")))
                .Element("//property/column").HasAttribute("name", "column_name");
        }

        private MappingTester<T> Model<T>(Action<ClassMap<T>> mapping)
        {
            return new MappingTester<T>()
                .ForMapping(mapping);
        }

        [Test]
        public void ShouldAddOnlyOneColumnWhenNotSpecified()
        {
            Model<PropertyTarget>(m => m.Component(x => x.Component, c => c.Map(x => x.Name)))
                .Element("//property[@name='Name']").HasThisManyChildNodes(1);
        }

        [Test]
        public void ShouldAddAllColumns()
        {
            Model<PropertyTarget>(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Columns.Add("one", "two", "three")))
                .Element("//property[@name='Name']").HasThisManyChildNodes(3)
                .Element("//property[@name='Name']/column[@name='one']").Exists()
                .Element("//property[@name='Name']/column[@name='two']").Exists()
                .Element("//property[@name='Name']/column[@name='three']").Exists();
        }

        [Test]
        public void ColumnName_IsPropertyName_WhenNoColumnNameGiven()
        {
            Model<PropertyTarget>(m => m.Component(x => x.Component, c => c.Map(x => x.Name)))
                .Element("//property[@name='Name']/column")
                .HasAttribute("name", "Name");
        }

        [Test]
        public void ColumnName_IsColumnName_WhenColumnNameGiven()
        {
            Model<PropertyTarget>(m => m.Component(x => x.Component, c => c.Map(x => x.Name, "column_name")))
                .Element("//property[@name='Name']/column")
                .HasAttribute("name", "column_name");
        }

        [Test]
        public void ColumnName_IsColumnName_WhenColumnNameFluentGiven()
        {
            Model<PropertyTarget>(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Columns.Add("column_name")))
                .Element("//property[@name='Name']/column")
                .HasAttribute("name", "column_name");
        }

        [Test]
        public void Map_WithFluentLength_OnString_UsesWithLengthOf_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Length(20)))
                .Element("//property[@name='Name']/column").HasAttribute("length", "20");
        }

        [Test]
        public void Map_WithFluentLength_AllowOnAnything_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Length(20)))
                .Element("//property[@name='Name']/column").HasAttribute("length", "20");
        }

        [Test]
        public void Map_UsesCanNotBeNull_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Not.Nullable()))
                .Element("//property[@name='Name']").DoesntHaveAttribute("not-null")
                .Element("//property[@name='Name']/column").HasAttribute("not-null", "true");
        }

        [Test]
        public void Map_UsesAsReadOnly_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).ReadOnly()))
                .Element("//property")
                .HasAttribute("insert", "false")
                .HasAttribute("update", "false");
        }

        [Test]
        public void Map_UsesUniqueKey_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).UniqueKey("uniquekey")))
                .Element("//property/column")
                .HasAttribute("unique-key", "uniquekey");
        }

        [Test]
        public void Map_UsesNotReadOnly_PropertyColumnAttribute()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Not.ReadOnly()))
                .Element("//property")
                .HasAttribute("insert", "true")
                .HasAttribute("update", "true");
        }

        [Test]
        public void Map_WithFluentFormula_UsesFormula()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Formula("foo(bar)")))
                .Element("//property").HasAttribute("formula", "foo(bar)");
        }

        [Test]
        public void CanSpecifyCustomTypeAsDotNetTypeGenerically()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).CustomType<custom_type_for_testing>()))
                .Element("//property").HasAttribute("type", typeof(custom_type_for_testing).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCustomTypeAsDotNetType()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).CustomType(typeof(custom_type_for_testing))))
                .Element("//property").HasAttribute("type", typeof(custom_type_for_testing).AssemblyQualifiedName);
        }

        [Test]
        public void CanSpecifyCustomSqlType()
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).CustomSqlType("image")))
                .Element("//property/column").HasAttribute("sql-type", "image");
        }

        [Test]
        public void CanSetAsUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Unique()))
                .Element("//property").DoesntHaveAttribute("unique")
                .Element("//property/column").HasAttribute("unique", "true");
        }

        [Test]
        public void CanSetAsNotUnique()
        {
            new MappingTester<MappedObject>()
                .ForMapping(m => m.Component(x => x.Component, c => c.Map(x => x.Name).Not.Unique()))
                .Element("//property").DoesntHaveAttribute("unique")
                .Element("//property/column").HasAttribute("unique", "false");
        }

        #region Custom IUserType impl for testing
        public class custom_type_for_testing : IUserType
        {
            public bool Equals(object x, object y)
            {
                throw new System.NotImplementedException();
            }

            public int GetHashCode(object x)
            {
                throw new System.NotImplementedException();
            }

            public object NullSafeGet(IDataReader rs, string[] names, object owner)
            {
                throw new System.NotImplementedException();
            }

            public void NullSafeSet(IDbCommand cmd, object value, int index)
            {
                throw new System.NotImplementedException();
            }

            public object DeepCopy(object value)
            {
                throw new System.NotImplementedException();
            }

            public object Replace(object original, object target, object owner)
            {
                throw new System.NotImplementedException();
            }

            public object Assemble(object cached, object owner)
            {
                throw new System.NotImplementedException();
            }

            public object Disassemble(object value)
            {
                throw new System.NotImplementedException();
            }

            public SqlType[] SqlTypes
            {
                get { throw new System.NotImplementedException(); }
            }

            public Type ReturnedType
            {
                get { throw new System.NotImplementedException(); }
            }

            public bool IsMutable
            {
                get { throw new System.NotImplementedException(); }
            }
        }
        #endregion
    }
}