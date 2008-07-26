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
            var map = new PropertyMap(property, false, property.Name, typeof(PropertyTarget));
            map.SetAttributeOnColumnElement("unique", "true");

            var document = new XmlDocument();
            XmlElement classElement = document.CreateElement("root");
            map.Write(classElement, new MappingVisitor());

            var columnElement = (XmlElement) classElement.SelectSingleNode("property/column");
            columnElement.AttributeShouldEqual("unique", "true");
        }
    }

    public class PropertyTarget
    {
        public string Name { get; set; }
    }
}