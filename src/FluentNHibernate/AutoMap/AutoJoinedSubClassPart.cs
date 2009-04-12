using System.Xml;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoJoinedSubClassPart<T> : AutoMap<T>, IMappingPart
    {
        private readonly string _keyColumn;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public AutoJoinedSubClassPart(string keyColumn)
        {
            _keyColumn = keyColumn;
        }

        public override void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public override void SetAttributes(Attributes atts)
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
            get { return 4; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        public AutoJoinedSubClassPart<T> WithTableName(string tableName)
        {             
            attributes.Store("table", tableName);
            return this;
        }
    }
}