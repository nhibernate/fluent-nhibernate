using System.Reflection;
using System.Xml;
using NUnit.Framework;
using ShadeTree.Core;
using ShadeTree.DomainModel;
using ShadeTree.DomainModel.Mapping;

namespace ShadeTree.Testing.DomainModel.Mapping
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
    }

    public class PropertyTarget
    {
        public string Name { get; set; }
    }
}