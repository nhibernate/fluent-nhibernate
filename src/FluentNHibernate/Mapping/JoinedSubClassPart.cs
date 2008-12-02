using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<T> : ClassMapBase<T>, IMappingPart
    {
        private readonly string _keyColumn;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public JoinedSubClassPart(string keyColumn)
        {
            _keyColumn = keyColumn;
        }

        public void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement subclassElement = classElement.AddElement("joined-subclass")
                .WithAtt("name", typeof(T).AssemblyQualifiedName);
            subclassElement.AddElement("key")
                .WithAtt("column", _keyColumn);
            subclassElement.WithProperties(attributes);

            writeTheParts(subclassElement, visitor);
        }

        public int Level
        {
            get { return 4; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        public JoinedSubClassPart<T> WithTableName(string tableName)
        {             
            attributes.Store("table", tableName);
            return this;
        }
    }
}