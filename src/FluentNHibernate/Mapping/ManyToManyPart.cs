using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ManyToManyPart<PARENT,CHILD> : IMappingPart
    {
        private readonly PropertyInfo _property;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();


        public ManyToManyPart(PropertyInfo property)
        {
            _property = property;
            _properties.Add("name", _property.Name);
            _properties.Add("cascade", "none");
        }

        public ManyToManyPart<PARENT, CHILD> CascadeAll()
        {
            _properties["cascade"] = "all";
            return this;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var conventions = visitor.Conventions;

            string tableName = conventions.GetManyToManyTableName(typeof(CHILD), typeof(PARENT));
            _properties.Add("table", tableName);

            XmlElement element = classElement.AddElement("bag").WithProperties(_properties);

            string foreignKeyName = conventions.GetForeignKeyNameOfParent(typeof(PARENT));
            element.AddElement("key").AddElement("column").WithAtt("name", foreignKeyName).WithAtt("not-null", "true");

            string childForeignKeyName = conventions.GetForeignKeyNameOfParent(typeof(CHILD));

            XmlElement manyToManyElement = element.AddElement("many-to-many")
                .WithAtt("class", typeof(CHILD).AssemblyQualifiedName);
            
            manyToManyElement.AddElement("column")
                .WithAtt("name", childForeignKeyName)
                .WithAtt("not-null", "true");
        }

        public int Level
        {
            get { return 3; }
        }
    }
}