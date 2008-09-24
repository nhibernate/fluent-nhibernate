using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NHibernate.Properties;
using NUnit.Framework;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class PropertyMapTester
    {
        [Test]
        public void SetAttributeOnColumnElement()
        {
            PropertyInfo property = ReflectionHelper.GetProperty<PropertyTarget>(x => x.Name);
            var map = new PropertyMap(property, false, typeof(PropertyTarget));
            map.SetAttributeOnColumnElement("unique", "true");

            var document = new XmlDocument();
            XmlElement classElement = document.CreateElement("root");
            map.Write(classElement, new MappingVisitor());

            var columnElement = (XmlElement) classElement.SelectSingleNode("property/column");
            columnElement.AttributeShouldEqual("unique", "true");
        }

        [Test]
        public void Map_WithoutColumnName_UsesPropertyNameFor_PropertyColumnAttribute()
        {
        	var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("column", "Name");
        }

        [Test]
        public void Map_WithoutColumnName_UsesPropertyNameFor_ColumnNameAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var columnElement = (XmlElement)classElement.SelectSingleNode("property/column");
            columnElement.AttributeShouldEqual("name", "Name");
        }


        [Test]
        public void Map_WithColumnName_UsesColumnNameFor_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();
            
            classMap.Map(x => x.Name, "column_name");

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("column", "column_name");
        }

        [Test]
        public void Map_WithColumnName_UsesColumnNameFor_ColumnNameAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name, "column_name");

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var columnElement = (XmlElement)classElement.SelectSingleNode("property/column");
            columnElement.AttributeShouldEqual("name", "column_name");
        }

        [Test]
        public void Map_WithFluentColumnName_UsesColumnNameFor_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .TheColumnNameIs("column_name");

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("column", "column_name");
        }

        
        [Test]
        public void Map_WithFluentColumnName_UsesColumnNameFor_ColumnNameAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .TheColumnNameIs("column_name");

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var columnElement = (XmlElement)classElement.SelectSingleNode("property/column");
            columnElement.AttributeShouldEqual("name", "column_name");
        }

        [Test]
        public void ColumnName_IsPropertyName_WhenNoColumnNameGiven()
        {
            var property = new ClassMap<PropertyTarget>()
                .Map(x => x.Name);

            Assert.AreEqual("Name", property.ColumnName());
        }

        [Test]
        public void ColumnName_IsColumnName_WhenColumnNameGiven()
        {
            var property = new ClassMap<PropertyTarget>()
                .Map(x => x.Name, "column_name");

            Assert.AreEqual("column_name", property.ColumnName());
        }

        [Test]
        public void ColumnName_IsColumnName_WhenColumnNameFluentGiven()
        {
            var property = new ClassMap<PropertyTarget>()
                .Map(x => x.Name)
                .TheColumnNameIs("column_name");

            Assert.AreEqual("column_name", property.ColumnName());
        }

        [Test]
        public void Map_WithFluentLength_UsesWithLengthOf_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .WithLengthOf(20);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("length", "20");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Map_WithFluentLength_UsesInvalidWithLengthOf_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Id)
                .WithLengthOf(20);

            classMap.CreateMapping(new MappingVisitor());

        }

        [Test]
        public void Map_WithFluentLength_UsesCanNotBeNull_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .CanNotBeNull();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("not-null", "true");
        }

        [Test]
        public void Map_WithFluentLength_UsesAsReadOnly_PropertyColumnAttribute()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .AsReadOnly();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("insert", "false");
            propertyElement.AttributeShouldEqual("update", "false");
        }

        [Test]
        public void Map_WithFluentFormula_UsesFormula() 
        {
            new MappingTester<PropertyTarget>()
                .ForMapping(m => m.Map(x => x.Name).FormulaIs("foo(bar)"))
                .Element("class/property").HasAttribute("formula", "foo(bar)");
        }

        [Test]
        public void CanSpecifyCustomType()
        {
            var classMap = new ClassMap<PropertyTarget>();
            var propertyMap = classMap.Map(x => x.Data)
                .CustomTypeIs("BinaryBlob");

            new MappingTester<PropertyTarget>()
                .ForMapping(classMap)
                .Element("class/property").HasAttribute("type", "BinaryBlob");
        }

        [Test]
        public void CanSpecifyCustomSqlType()
        {
            var classMap = new ClassMap<PropertyTarget>();
            var propertyMap = classMap.Map(x => x.Data)
                .CustomSqlTypeIs("image");

            new MappingTester<PropertyTarget>()
                .ForMapping(classMap)                
                .Element("class/property/column").HasAttribute("sql-type", "image");
        }
    }

    public class PropertyTarget
    {
        public string Name { get; set; }
        public PropertyReferenceTarget Reference { get; set; }
        public IList<PropertyReferenceTarget> References { get; set; }
        public ComponentTarget Component { get; set; }
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }

    public class PropertyReferenceTarget {}
    
    public class ComponentTarget
    {
        public object Name { get; set; }
    }

    public class FakePropertyAccessor : IPropertyAccessor
    {
        public IGetter GetGetter(Type theClass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public ISetter GetSetter(Type theClass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool CanAccessTroughReflectionOptimizer
        {
            get { throw new NotImplementedException(); }
        }

    }
}
