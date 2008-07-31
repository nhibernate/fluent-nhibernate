using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Linq.Expressions;
using System;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<PARENT, CHILD> : IMappingPart
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly PropertyInfo _property;
        private string _keyColumnName;
        private string _collectionType;
        private IndexMapping _indexMapping;

        public OneToManyPart(PropertyInfo property)
        {
            _keyColumnName = string.Empty;
            _property = property;            
            _properties.Add("name", _property.Name);
            _properties.Add("cascade", "none");
            
            // default the collection type to bag for now
            _collectionType = "bag";
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement(_collectionType)
                .WithProperties(_properties);

            string foreignKeyName = _keyColumnName;
            if (string.IsNullOrEmpty(_keyColumnName))
                foreignKeyName = visitor.Conventions.GetForeignKeyNameOfParent(typeof(PARENT));

            element.AddElement("key").SetAttribute("column", foreignKeyName);

            // TODO: Revisit. Not so sure about this.
            if (_indexMapping != null)
            {
                var indexElement = element.AddElement("index");
                _indexMapping.WriteAttributesToIndexElement(indexElement);
            }

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

        public OneToManyPart<PARENT, CHILD> AsSet()
        {
            _collectionType = "set";
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsBag()
        {
            _collectionType = "bag";
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsList()
        {
            _indexMapping = new IndexMapping();
            _collectionType = "list";
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsList(Action<IndexMapping> action)
        {
            AsList();
            action(_indexMapping);
            return this;
        }

        public OneToManyPart<PARENT, CHILD> WithKeyColumn(string columnName)
        {
            _keyColumnName = columnName;
            return this;
        }

        public class IndexMapping
        {
            private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();

            public IndexMapping WithColumn(string indexColumnName)
            {
                _properties["column"] = indexColumnName;
                return this;
            }

            public IndexMapping WithType<INDEXTYPE>()
            {
                _properties["type"] = typeof(INDEXTYPE).AssemblyQualifiedName;
                return this;
            }

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(_properties);
            }
        }
    }


}