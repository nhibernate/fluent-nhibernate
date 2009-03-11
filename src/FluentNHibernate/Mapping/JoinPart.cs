using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface IJoin : IMappingPart
    {
        void WithKeyColumn(string column);
    }
    /// <summary>
    /// Maps to the Join element in NH 2.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JoinPart<T> : ClassMapBase<T>, IJoin
    {
        private readonly Cache<string, string> properties = new Cache<string, string>();
        private string keyColumnName;

        public JoinPart(string tableName)
        {
            properties.Store("table", tableName);
            keyColumnName = GetType().GetGenericArguments()[0].Name + "ID";
        }

        public void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
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
            var joinElement = classElement.AddElement("join")
                .WithProperties(properties);

            joinElement.AddElement("key")
                .SetAttribute("column", keyColumnName);

            writeTheParts(joinElement, visitor);
        }

        public int Level
        {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Last; }
        }

        public void WithKeyColumn(string column)
        {
            keyColumnName = column;
        }
    }
}