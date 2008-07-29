using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using ShadeTree.Validation;

namespace FluentNHibernate.Mapping
{
    public class ManyToOnePart : IMappingPart
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly PropertyInfo _property;
        private string columnName;

        public ManyToOnePart(PropertyInfo property) : this(property, null)
        {
            _property = property;

            _properties.Add("name", property.Name);
            _properties.Add("foreign-key", string.Format("FK_{0}To{1}", property.DeclaringType.Name, property.Name));
        }

        public ManyToOnePart(PropertyInfo property, string columnName) 
        {
            _property = property;

            _properties.Add("name", property.Name);
            _properties.Add("foreign-key", string.Format("FK_{0}To{1}", property.DeclaringType.Name, property.Name));

            this.columnName = columnName;
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            visitor.RegisterDependency(_property.PropertyType);

            string fkName = columnName;
            
            if (string.IsNullOrEmpty(columnName))
                fkName = visitor.Conventions.GetForeignKeyName(_property);

            _properties.Add("column", fkName);
            _properties.Add("cascade", "all");

            classElement.AddElement("many-to-one").WithProperties(_properties);

        }

        public int Level
        {
            get { return 3; }
        }

        #endregion
    }
}