using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IAnyPart<T> : IMappingPart, IAccessStrategy<IAnyPart<T>>
    {
        /// <summary>
        /// (REQUIRED) The identity type of the any mapping
        /// </summary>
        /// <returns></returns>
        IAnyPart<T> IdentityType(Expression<Func<T, object>> expression);

        /// <summary>
        /// (REQUIRED) Specifies the column name that will contain the type of the associated entity
        /// </summary>
        IAnyPart<T> EntityTypeColumn(string columnName);

        /// <summary>
        /// (REQUIRED) Specifies the column name that will hold the identifier
        /// </summary>
        IAnyPart<T> EntityIdentifierColumn(string columnName);

        /// <summary>
        /// This is used to map specific class types to value types stored in the EntityTypeColumn
        /// This method should only be called if the MetaType specified for this ANY mapping is a basic data type (such as string)
        /// </summary>
        /// <typeparam name="TModel">The class type to map</typeparam>
        /// <param name="valueMap">The string or character representing the value stored in the EntityTypeColumn in the table</param>
        IAnyPart<T> AddMetaValue<TModel>(string valueMap);

        /// <summary>
        /// Sets the cascade of this part. Valid options are "none", "all", and "save-update"
        /// </summary>
        CascadeExpression<IAnyPart<T>> Cascade { get; }
    }

    /// <summary>
    /// Represents the "Any" mapping in NHibernate. It is impossible to specify a foreign key constraint for this kind of association. For more information
    /// please reference chapter 5.2.4 in the NHibernate online documentation
    /// </summary>
    public class AnyPart<T> : IAnyPart<T>
    {
        private readonly AccessStrategyBuilder<IAnyPart<T>> access;
        private readonly Cache<string, string> properties = new Cache<string, string>();
        private readonly Cache<string, string> metaValues = new Cache<string, string>();
        private readonly CascadeExpression<IAnyPart<T>> cascade;
        private string entityTypeColumn;
        private string entityIdentifierColumn;

        public AnyPart(PropertyInfo property)
        {
            access = new AccessStrategyBuilder<IAnyPart<T>>(this, value => SetAttribute("access", value));
            cascade = new CascadeExpression<IAnyPart<T>>(this, value => SetAttribute("cascade", value));
            AnyProperty = property;
        }

        /// <summary>
        /// The property on your class that contains the mapped object
        /// </summary>
        public PropertyInfo AnyProperty { get; private set; }
        public PropertyInfo IdProperty { get; private set; }

        /// <summary>
        /// Used to specify attributes on the XML Element if the class does not contain the necessary Fluent mapping
        /// </summary>
        public void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
        }

        /// <summary>
        /// Used to specify attributes on the XML Element if the class does not contain the necessary Fluent mapping
        /// </summary>
        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        /// <summary>
        /// Writes the Any element to the XmlElement. Typically called during a ClassMap write
        /// </summary>
        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            if (string.IsNullOrEmpty(entityIdentifierColumn) || string.IsNullOrEmpty(entityTypeColumn))
            { throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Identifier and Entity Type Column"); }
            if (IdProperty == null)
            { throw new InvalidOperationException("<any> mapping is not valid without specifying an IdType"); }

            //Create Any element with attributes
            properties.Store("name", AnyProperty.Name);
            properties.Store("id-type", TypeMapping.GetTypeString(IdProperty.PropertyType));
            string metaType = (metaValues.Any())
                ? TypeMapping.GetTypeString(typeof(string))
                : TypeMapping.GetTypeString(AnyProperty.PropertyType);
            properties.Store("meta-type", metaType);
            XmlElement anyElement = classElement.AddElement("any").WithProperties(properties);

            //Write Metavalues collection as elements
            metaValues.ForEachPair((k, v) => anyElement.AddElement("meta-value")
                .WithAtt("class", k)
                .WithAtt("value", v));

            //Write EntityTypeColumn then EntityIdentifierColumn (required order based on NHibernate specs)
            anyElement.AddElement("column").WithAtt("name", entityTypeColumn);
            anyElement.AddElement("column").WithAtt("name", entityIdentifierColumn);
        }

        /// <summary>
        /// Indicates the level within the Position, that this Part should be written at. The Any part has no intrinsic level it's required to appear at.
        /// </summary>
        public int LevelWithinPosition
        {
            get { return 1; }
        }

        /// <summary>
        /// The general ordering of which the Part should be written in the HBM mapping. The Any Part can be placed anywhere in a mapping file after the header components have been set.
        /// </summary>
        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        /// <summary>
        /// Defines how NHibernate will access the object for persisting/hydrating (Defaults to Property)
        /// </summary>
        public AccessStrategyBuilder<IAnyPart<T>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Cascade style (Defaults to none)
        /// </summary>
        public CascadeExpression<IAnyPart<T>> Cascade
        {
            get { return cascade; }
        }

        public IAnyPart<T> IdentityType(Expression<Func<T, object>> expression)
        {
            IdProperty = ReflectionHelper.GetProperty(expression);
            return this;
        }

        public IAnyPart<T> EntityTypeColumn(string columnName)
        {
            entityTypeColumn = columnName;
            return this;
        }

        public IAnyPart<T> EntityIdentifierColumn(string columnName)
        {
            entityIdentifierColumn = columnName;
            return this;
        }

        public IAnyPart<T> AddMetaValue<TModel>(string valueMap)
        {
            metaValues.Store(TypeMapping.GetTypeString(typeof(TModel)), valueMap);
            return this;
        }
    }
}
