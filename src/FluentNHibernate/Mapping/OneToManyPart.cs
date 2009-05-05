using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class OneToManyPart<TChild> : ToManyBase<OneToManyPart<TChild>, TChild>, IOneToManyPart, IAccessStrategy<OneToManyPart<TChild>> 
    {
        private readonly ColumnNameCollection<OneToManyPart<TChild>> columnNames;
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
            columnNames = new ColumnNameCollection<OneToManyPart<TChild>>(this);
            properties.Store("name", member.Name);
        }

        public override void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement collectionElement = WriteCollectionElement(classElement);
            Cache.Write(collectionElement, visitor);
            WriteKeyElement(visitor, collectionElement);

            if (indexMapping != null)
                WriteIndexElement(collectionElement);

            WriteMappingTypeElement(visitor, collectionElement);
        }

        private XmlElement WriteCollectionElement(XmlElement classElement)
        {
            if (collectionType == "array")
                properties.Remove("collection-type");

            XmlElement collectionElement = classElement.AddElement(collectionType)
                .WithProperties(properties);

            if (!string.IsNullOrEmpty(TableName))
                collectionElement.SetAttribute("table", TableName);

            if (batchSize > 0)
                collectionElement.WithAtt("batch-size", batchSize.ToString());


            return collectionElement;
        }

        private void WriteMappingTypeElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            if (elementMapping != null) 
            {
                var elementElement = collectionElement.AddElement("element");                
                elementMapping.WriteAttributesToIndexElement(elementElement);                
            } 
            else if (componentMapping == null) 
            {
                // standard one-to-many element
                collectionElement.AddElement("one-to-many")
                    .WithProperties(collectionProperties)
                    .SetAttribute("class", typeof(TChild).AssemblyQualifiedName);
                    
            }
            else
            {
                // specified a component, so output that instead
                componentMapping.Write(collectionElement, visitor);
            }
        }

        private void WriteKeyElement(IMappingVisitor visitor, XmlElement collectionElement)
        {
            var columns = columnNames.List();

            if (columns.Count == 1)
                keyProperties.Store("column", columns[0]);

            var key = collectionElement.AddElement("key")
                .WithProperties(keyProperties);

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
            properties.Store(name, value);
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

        public FetchTypeExpression<OneToManyPart<TChild>> FetchType
        {
            get
            {
                return new FetchTypeExpression<OneToManyPart<TChild>>(this, properties);
            }
        }

        public ColumnNameCollection<OneToManyPart<TChild>> KeyColumnNames
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
            return Inverse();
        }

        IOneToManyPart IOneToManyPart.LazyLoad()
        {
            return LazyLoad();
        }

        IOneToManyPart IOneToManyPart.Not
        {
            get { return Not; }
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType<TCollection>()
        {
            return CollectionType<TCollection>();
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(Type type)
        {
            return CollectionType(type);
        }

        /// <summary>
        /// Sets a custom collection type
        /// </summary>
        IOneToManyPart IOneToManyPart.CollectionType(string type)
        {
            return CollectionType(type);
        }

        IColumnNameCollection IOneToManyPart.KeyColumnNames
        {
            get { return KeyColumnNames; }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        public NotFoundExpression<OneToManyPart<TChild>> NotFound
        {
            get
            {
                return new NotFoundExpression<OneToManyPart<TChild>>(this, collectionProperties);
            }
        }

        INotFoundExpression IOneToManyPart.NotFound
        {
            get { return NotFound; }
        }

        #endregion
    }
}
