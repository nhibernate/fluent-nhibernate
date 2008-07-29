using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IMappingPart
    {
        private readonly PropertyInfo _property;
        private readonly string _columnName;

        public IdentityPart(PropertyInfo property, string columnName)
        {
            _property = property;
            _columnName = columnName;
        }


        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("id") 
                //.WithAtt("xmlns", "urn:nhibernate-mapping-2.2")
                .WithAtt("name", _property.Name)
                .WithAtt("column", _columnName)
                .WithAtt("type", TypeMapping.GetTypeString(_property.PropertyType))
                .WithAtt("unsaved-value", "0");

            element.AddElement("generator").WithAtt("class", "identity");
        }

        public int Level
        {
            get { return 0; }
        }
    }
}