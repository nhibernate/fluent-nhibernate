using System;
using System.Reflection;
using System.Xml;
using NHibernate.Properties;
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

        [Test]
        public void AccessAsProperty_SetsAccessStrategyToProperty()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsProperty();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "property");
        }

        [Test]
        public void AccessAsField_SetsAccessStrategyToField()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsField();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field");
        }

        [Test]
        public void AccessAsCamelCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCase()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsCamelCaseField();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.camelcase");
        }

        [Test]
        public void AccessAsCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsCamelCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.camelcase-underscore");
        }

        [Test]
        public void AccessAsLowerCaseField_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCase()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsLowerCaseField();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.lowercase");
        }

        [Test]
        public void AccessAsLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsLowerCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.lowercase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsPascalCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.pascalcase-underscore");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMPrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseM()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsPascalCaseField(Prefix.m);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.pascalcase-m");
        }

        [Test]
        public void AccessAsPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToField_and_SetsNamingStrategyToLowerCaseMUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsPascalCaseField(Prefix.mUnderscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "field.pascalcase-m-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCase()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughCamelCaseField();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.camelcase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughCamelCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToCamelCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.camelcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseField_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCase()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughLowerCaseField();

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.lowercase");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughLowerCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToLowerCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughLowerCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.lowercase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.Underscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.pascalcase-underscore");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMPrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseM()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.m);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.pascalcase-m");
        }

        [Test]
        public void AccessAsReadOnlyPropertyThroughPascalCaseFieldWithMUnderscorePrefix_SetsAccessStrategyToNoSetter_and_SetsNamingStrategyToPascalCaseMUnderscore()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.AsReadOnlyPropertyThroughPascalCaseField(Prefix.mUnderscore);

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "nosetter.pascalcase-m-underscore");
        }

        [Test]
        public void AccessUsingClassName_SetsAccessAttributeToClassName()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.Using("fully qualified class name");

            var document = classMap.CreateMapping(new MappingVisitor());

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", "fully qualified class name");
        }

        [Test]
        public void AccessUsingClassType_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.Using(typeof(FakePropertyAccessor));

            var document = classMap.CreateMapping(new MappingVisitor());

            var assemblyQualifiedClassName = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", assemblyQualifiedClassName);
        }

        [Test]
        public void AccessUsingClassGenericParameter_SetsAccessAttributeToAssemblyQualifiedName()
        {
            var classMap = new ClassMap<PropertyTarget>();

            classMap.Map(x => x.Name)
                .Access.Using<FakePropertyAccessor>();

            var document = classMap.CreateMapping(new MappingVisitor());

            var assemblyQualifiedClassName = typeof(FakePropertyAccessor).AssemblyQualifiedName;

            // attribute on property
            var classElement = document.DocumentElement.SelectSingleNode("class");
            var propertyElement = (XmlElement)classElement.SelectSingleNode("property");
            propertyElement.AttributeShouldEqual("access", assemblyQualifiedClassName);
        }
    }

    public class PropertyTarget
    {
        public string Name { get; set; }
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
    }
}