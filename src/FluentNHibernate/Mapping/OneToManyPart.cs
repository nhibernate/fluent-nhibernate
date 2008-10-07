using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{

    public class OneToManyPart<PARENT, CHILD> : IMappingPart, IAccessStrategy<OneToManyPart<PARENT, CHILD>>, IOneToManyPart
    {
		private readonly Cache<string, string> _properties = new Cache<string, string>();
        private readonly PropertyInfo _property;
        //private string _keyColumnName;
        private readonly Cache<string, string> _keyProperties = new Cache<string, string>();
        private string _collectionType;
        private IndexMapping _indexMapping;
        private string _elementColumn;
        private readonly AccessStrategyBuilder<OneToManyPart<PARENT, CHILD>> access;
        private CompositeElementPart<CHILD> _componentMapping;
        private string _tableName;

        public OneToManyPart(PropertyInfo property)
        {
            access = new AccessStrategyBuilder<OneToManyPart<PARENT, CHILD>>(this);
            _property = property;            
            _properties.Store("name", _property.Name);
            
            // default the collection type to bag for now
            _collectionType = "bag";
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            visitor.Conventions.AlterOneToManyMap(this);

            XmlElement collectionElement = WriteCollectionElement(classElement);
            WriteKeyElement(visitor, collectionElement);

            if (_indexMapping != null)
                WriteIndexElement(collectionElement);

            WriteMappingTypeElement(visitor, collectionElement);
        }

        private XmlElement WriteCollectionElement(XmlElement classElement)
        {
            XmlElement collectionElement = classElement.AddElement(_collectionType)
                .WithProperties(_properties);

            if (!string.IsNullOrEmpty(_tableName))
                collectionElement.SetAttribute("table", _tableName);
            return collectionElement;
        }

        private void WriteMappingTypeElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            if (!string.IsNullOrEmpty(_elementColumn)) 
            { 
                collectionElement.AddElement("element") 
                    .SetAttribute("column", _elementColumn);
                collectionElement.SetAttributeOnChild("element", "type", typeof(CHILD).Name); 
            } 
            else if (_componentMapping == null) 
            {
                // standard one-to-many element
                collectionElement.AddElement("one-to-many")
                    .SetAttribute("class", typeof(CHILD).AssemblyQualifiedName);
            }
            else
            {
                // specified a component, so output that instead
                _componentMapping.Write(collectionElement, visitor);
            }
        }

        private void WriteIndexElement(XmlElement collectionElement)
        {
            var indexElement = collectionElement.AddElement("index");
            _indexMapping.WriteAttributesToIndexElement(indexElement);
        }

        private void WriteKeyElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            if(!_keyProperties.Has("column"))
                _keyProperties.Store("column", visitor.Conventions.GetForeignKeyNameOfParent(typeof(PARENT)));

            collectionElement.AddElement("key")
                .WithProperties(_keyProperties);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this one-to-many mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            _properties.Store(name, value);
        }

        public int Level
        {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        #endregion

        public OneToManyPart<PARENT, CHILD> LazyLoad()
        {
            _properties.Store("lazy", "true");
            return this;
        }

        public OneToManyPart<PARENT, CHILD> IsInverse()
        {
			_properties.Store("inverse", "true");
            return this;
        }

        public CollectionCascadeExpression<OneToManyPart<PARENT, CHILD>> Cascade
        {
			get { return new CollectionCascadeExpression<OneToManyPart<PARENT, CHILD>>(this); }
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

        public OneToManyPart<PARENT, CHILD> AsList(Action<IndexMapping> customIndexMapping)
        {
            AsList();
            customIndexMapping(_indexMapping);
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsMap<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector)
        {
            return AsMap(indexSelector, null);
        }

        public OneToManyPart<PARENT, CHILD> AsMap<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
           _collectionType = "map";
           return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        public OneToManyPart<PARENT, CHILD> AsArray<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector)
        {
            return AsArray(indexSelector, null);
        }

        public OneToManyPart<PARENT, CHILD> AsArray<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            _collectionType = "array";
            return AsIndexedCollection(indexSelector, customIndexMapping);
        }

        private OneToManyPart<PARENT, CHILD> AsIndexedCollection<INDEX_TYPE>(Expression<Func<CHILD, INDEX_TYPE>> indexSelector, Action<IndexMapping> customIndexMapping)
        {
            var indexProperty = ReflectionHelper.GetProperty(indexSelector);
            _indexMapping = new IndexMapping();
            _indexMapping.WithType<INDEX_TYPE>();
            _indexMapping.WithColumn(indexProperty.Name);

            if (customIndexMapping != null)
                customIndexMapping(_indexMapping);
            
            return this;
        }

        public OneToManyPart<PARENT, CHILD> WithKeyColumn(string columnName)
        {
            _keyProperties.Store("column", columnName);
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsElement(string columnName) 
        { 
            _elementColumn = columnName; 
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
        /// Sets the table name for this one-to-many.
        /// </summary>
        /// <param name="name">Table name</param>
        public OneToManyPart<PARENT, CHILD> WithTableName(string name)
        {
            _tableName = name;
            return this;
        }

        public OneToManyPart<PARENT, CHILD> WithForeignKeyConstraintName(string foreignKeyName)
        {
            _keyProperties.Store("foreign-key", foreignKeyName);
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
            private readonly Cache<string, string> _properties = new Cache<string, string>();

            public IndexMapping WithColumn(string indexColumnName)
            {
				_properties.Store("column", indexColumnName);
                return this;
            }

            public IndexMapping WithType<INDEXTYPE>()
            {
				_properties.Store("type", typeof(INDEXTYPE).AssemblyQualifiedName);
                return this;
            }            

            internal void WriteAttributesToIndexElement(XmlElement indexElement)
            {
                indexElement.WithProperties(_properties);
            }
        }

        #region Explicit IOneToManyPart Implementation

        CollectionCascadeExpression<IOneToManyPart> IOneToManyPart.Cascade
        {
            get { return new CollectionCascadeExpression<IOneToManyPart>(this); }
        }

        IOneToManyPart IOneToManyPart.IsInverse()
        {
            return this.IsInverse();
        }

        IOneToManyPart IOneToManyPart.LazyLoad()
        {
            return this.LazyLoad();
        }

        #endregion

    }
}
