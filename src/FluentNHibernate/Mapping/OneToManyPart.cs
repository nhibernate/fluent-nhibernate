using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<CHILD> : ToManyBase<OneToManyPart<CHILD>, CHILD>, IOneToManyPart, IAccessStrategy<OneToManyPart<CHILD>> 
    {
        private readonly ColumnNameCollection<OneToManyPart<CHILD>> columnNames;
        private readonly Cache<string, string> collectionProperties = new Cache<string, string>();

        public OneToManyPart(Type entity, PropertyInfo property)
            : this(entity, property, property.PropertyType)
        {}

        public OneToManyPart(Type entity, MethodInfo method)
            : this(entity, method, method.ReturnType)
        {}

        protected OneToManyPart(Type entity, MemberInfo member, Type collectionType)
            : base(entity, member, collectionType)
        {
            columnNames = new ColumnNameCollection<OneToManyPart<CHILD>>(this);
            _properties.Store("name", member.Name);
        }

        public override void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement collectionElement = WriteCollectionElement(classElement);
            Cache.Write(collectionElement, visitor);
            WriteKeyElement(visitor, collectionElement);

            if (_indexMapping != null)
                WriteIndexElement(collectionElement);

            WriteMappingTypeElement(visitor, collectionElement);
        }

        private XmlElement WriteCollectionElement(XmlElement classElement)
        {
            if (_collectionType == "array")
                _properties.Remove("collection-type");

            XmlElement collectionElement = classElement.AddElement(_collectionType)
                .WithProperties(_properties);

            if (!string.IsNullOrEmpty(TableName))
                collectionElement.SetAttribute("table", TableName);

            if (batchSize > 0)
                collectionElement.WithAtt("batch-size", batchSize.ToString());


            return collectionElement;
        }

        private void WriteMappingTypeElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            if (_elementMapping != null) 
            {
                var elementElement = collectionElement.AddElement("element");                
                _elementMapping.WriteAttributesToIndexElement(elementElement);                
            } 
            else if (_componentMapping == null) 
            {
                // standard one-to-many element
                collectionElement.AddElement("one-to-many")
                    .WithProperties(collectionProperties)
                    .SetAttribute("class", typeof(CHILD).AssemblyQualifiedName);
                    
            }
            else
            {
                // specified a component, so output that instead
                _componentMapping.Write(collectionElement, visitor);
            }
        }

        private void WriteKeyElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            var columns = columnNames.List();

            if (columns.Count == 1)
                _keyProperties.Store("column", columns[0]);

            var key = collectionElement.AddElement("key")
                .WithProperties(_keyProperties);

            if (columns.Count <= 1) return;

            foreach (var columnName in columns)
            {
                key.AddElement("column")
                    .WithAtt("name", columnName);
            }
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this one-to-many mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public override void SetAttribute(string name, string value)
        {
            _properties.Store(name, value);
        }

        public override void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public override int LevelWithinPosition
        {
            get { return 1; }
        }

        public override PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        public FetchTypeExpression<OneToManyPart<CHILD>> FetchType
        {
            get
            {
                return new FetchTypeExpression<OneToManyPart<CHILD>>(this, _properties);
            }
        }

        public ColumnNameCollection<OneToManyPart<CHILD>> KeyColumnNames
        {
            get { return columnNames; }
        }

        #region Explicit IOneToManyPart Implementation

        CollectionCascadeExpression<IOneToManyPart> IOneToManyPart.Cascade
        {
            get { return new CollectionCascadeExpression<IOneToManyPart>(this); }
        }

        IOneToManyPart IOneToManyPart.Inverse()
        {
            return this.Inverse();
        }

        IOneToManyPart IOneToManyPart.LazyLoad()
        {
            return this.LazyLoad();
        }

        IOneToManyPart IOneToManyPart.Not
        {
            get { return this.Not; }
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType<TCollection>()
        {
            return this.CollectionType<TCollection>();
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(Type type)
        {
            return this.CollectionType(type);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(string type)
        {
            return this.CollectionType(type);
        }

        IColumnNameCollection IOneToManyPart.KeyColumnNames
        {
            get { return KeyColumnNames; }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        public NotFoundExpression<OneToManyPart<CHILD>> NotFound
        {
            get
            {
                return new NotFoundExpression<OneToManyPart<CHILD>>(this, collectionProperties);
            }
        }

        INotFoundExpression IOneToManyPart.NotFound
        {
            get { return NotFound; }
        }

        #endregion
    }
}
