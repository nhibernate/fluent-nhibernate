using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<PARENT, CHILD> : IMappingPart, IAccessStrategy<OneToManyPart<PARENT, CHILD>>
    {
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly PropertyInfo _property;
        private string _keyColumnName;
        private string _collectionType;
        private IndexMapping _indexMapping;
        private readonly AccessStrategyBuilder<OneToManyPart<PARENT, CHILD>> access;
        private CompositeElementPart<CHILD> _componentMapping;

        public OneToManyPart(PropertyInfo property)
        {
            access = new AccessStrategyBuilder<OneToManyPart<PARENT, CHILD>>(this);
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

            if (_componentMapping == null)
            {
                // standard one-to-many element
                element.AddElement("one-to-many")
                    .SetAttribute("class", typeof(CHILD).AssemblyQualifiedName);
            }
            else
            {
                // specified a component, so output that instead
                _componentMapping.Write(element, visitor);
            }
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this one-to-many mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            _properties.Add(name, value);
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

        /// <summary>
        /// Maps this collection as a collection of components.
        /// </summary>
        /// <param name="action">Component mapping</param>
        public OneToManyPart<PARENT, CHILD> Component(Action<CompositeElementPart<CHILD>> action)
        {
            _componentMapping = new CompositeElementPart<CHILD>();

            action(_componentMapping);

            return this;
        }

        /// <summary>
        /// Set the access and naming strategy for this one-to-many.
        /// </summary>
        public AccessStrategyBuilder<OneToManyPart<PARENT, CHILD>> Access
        {
            get { return access; }
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