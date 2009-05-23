using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class IdentityPart : IIdentityPart
    {
		private readonly IdentityGenerationStrategyBuilder generatedBy;
		private readonly Cache<string, string> generatorParameters = new Cache<string, string>();
        private readonly Cache<string, string> elementAttributes = new Cache<string, string>();
		private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<IIdentityPart> access;
	    private object unsavedValue;

	    public IdentityPart(Type entity, PropertyInfo property, string columnName)
		{
            access = new AccessStrategyBuilder<IIdentityPart>(this, value => SetAttribute("access", value));

	        EntityType = entity;
			this.property = property;
			ColumnName(columnName);
			generatedBy = new IdentityGenerationStrategyBuilder(this);
		}

		public IdentityPart(Type entity, PropertyInfo property) : this(entity, property, null)
		{
		}

		public IdentityGenerationStrategyBuilder GeneratedBy
		{
			get { return generatedBy; }
		}

        private string m_GeneratorClass;
        private string GeneratorClass
		{
			get
			{
				if (m_GeneratorClass != null) return m_GeneratorClass;
				if (IdentityType == typeof (Guid)) return "guid.comb";
				if (IdentityType == typeof (int) || IdentityType == typeof (long))
					return "identity";
				return "assigned";
			}
		}

		public Type IdentityType
		{
			get { return property.PropertyType; }
		}

        public Type EntityType { get; private set; }

        public PropertyInfo Property
        {
            get { return property; }
        }

		public void Write(XmlElement classElement, IMappingVisitor visitor)
		{
			XmlElement element = classElement.AddElement("id")
				.WithAtt("name", property.Name)
				.WithAtt("type", TypeMapping.GetTypeString(property.PropertyType));

            if (unsavedValue != null)
				element.WithAtt("unsaved-value", unsavedValue.ToString());

            elementAttributes.ForEachPair((name, value) => element.WithAtt(name, value));

			XmlElement generatorElement = element.AddElement("generator").WithAtt("class", GeneratorClass);
			generatorParameters.ForEachPair(
				(name, innerXml) => generatorElement.AddElement("param").WithAtt("name", name).InnerXml = innerXml);
		}

        /// <summary>
        /// Set an attribute on the xml element produced by this identity mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
	    public void SetAttribute(string name, string value)
	    {
	        elementAttributes.Store(name, value);
	    }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

	    public int LevelWithinPosition
		{
			get { return 2; }
		}

	    public PartPosition PositionOnDocument
	    {
            get { return PartPosition.First; }
	    }

        public IIdentityPart SetGeneratorClass(string generator)
		{
			m_GeneratorClass = generator;
			return this;
		}

        public IIdentityPart AddGeneratorParam(string name, string innerXml)
		{
			generatorParameters.Store(name, innerXml);
			return this;
		}

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
        public AccessStrategyBuilder<IIdentityPart> Access
	    {
	        get { return access; }
	    }

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        public IIdentityPart WithUnsavedValue(object unsavedValue)
        {
            this.unsavedValue = unsavedValue;
            return this;
        }

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        public IIdentityPart ColumnName(string columnName)
        {
            SetAttribute("column", columnName);
            return this;
        }

        /// <summary>
        /// Gets the column name
        /// </summary>
        /// <returns></returns>
        public string GetColumnName()
        {
            if (elementAttributes.Has("column"))
                return elementAttributes.Get("column");

            return null;
        }
    }
}
