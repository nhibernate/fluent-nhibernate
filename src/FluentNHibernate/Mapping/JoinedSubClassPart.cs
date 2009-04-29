using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class JoinedSubClassPart<T> : ClasslikeMapBase<T>, IJoinedSubclass
    {
        private readonly string _keyColumn;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public JoinedSubClassPart(string keyColumn)
        {
            _keyColumn = keyColumn;
        }

        public virtual void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public virtual void SetAttributes(Attributes atts)
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

        public int LevelWithinPosition
        {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        public JoinedSubClassPart<T> WithTableName(string tableName)
        {             
            attributes.Store("table", tableName);
            return this;
        }

        void IJoinedSubclass.WithTableName(string tableName)
        {
            WithTableName(tableName);
        }
    }
}