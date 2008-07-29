using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<PARENT, CHILD> : IMappingPart
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly PropertyInfo _property;
        private readonly string columnName;


        public OneToManyPart(PropertyInfo property, string columnName)
        {
            _property = property;
            this.columnName = columnName;
            _properties.Add("name", _property.Name);
            _properties.Add("cascade", "none");
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            // TODO -- hard-coded to List just for today
            XmlElement element = classElement.AddElement("bag").WithProperties(_properties);

            string foreignKeyName = columnName;

            if (string.IsNullOrEmpty(columnName))
                foreignKeyName = visitor.Conventions.GetForeignKeyNameOfParent(typeof(PARENT));

            element.AddElement("key").SetAttribute("column", foreignKeyName);
            element.AddElement("one-to-many").SetAttribute("class", typeof (CHILD).AssemblyQualifiedName);


        }

        public int Level
        {
            get { return 3; }
        }

        #endregion

        public OneToManyPart<PARENT, CHILD> LazyLoad()
        {
            _properties["lazy"] = "true";
            return this;
        }

        public OneToManyPart<PARENT, CHILD> IsInverse()
        {
            _properties.Add("inverse", "true");
            return this;
        }

        public OneToManyPart<PARENT, CHILD> CascadeAll()
        {
            _properties["cascade"] = "all";
            
            return this;
        }
    }
}