using System;
using System.Reflection;
using System.Xml;
using ShadeTree.Core;

namespace FluentNHibernate.Mapping
{
	public class IdentityPart : IMappingPart, IAccessStrategy<IdentityPart>
	{
		private readonly string _columnName;
		private readonly IdentityGenerationStrategyBuilder _generatedBy;
		private readonly Cache<string, string> _generatorParameters = new Cache<string, string>();
        private readonly Cache<string, string> _elementAttributes = new Cache<string, string>();
		private readonly PropertyInfo _property;
		private string _generatorClass;
        private readonly AccessStrategyBuilder<IdentityPart> access;

		public IdentityPart(PropertyInfo property, string columnName)
		{
            access = new AccessStrategyBuilder<IdentityPart>(this);

			_property = property;
			_columnName = columnName;
			_generatedBy = new IdentityGenerationStrategyBuilder(this);
		}

		public IdentityPart(PropertyInfo property) : this(property, property.Name)
		{
		}

		public IdentityGenerationStrategyBuilder GeneratedBy
		{
			get { return _generatedBy; }
		}

		private string generatorClass
		{
			get
			{
				if (_generatorClass != null) return _generatorClass;
				if (IdentityType == typeof (Guid)) return "guid.comb";
				if (IdentityType == typeof (int) || IdentityType == typeof (long))
					return "identity";
				return "assigned";
			}
		}

		public Type IdentityType
		{
			get { return _property.PropertyType; }
		}

		public void Write(XmlElement classElement, IMappingVisitor visitor)
		{
			XmlElement element = classElement.AddElement("id")
				.WithAtt("name", _property.Name)
				.WithAtt("column", _columnName)
				.WithAtt("type", TypeMapping.GetTypeString(_property.PropertyType))
				.WithAtt("unsaved-value", "0");

            _elementAttributes.ForEachPair((name, value) => element.WithAtt(name, value));

			XmlElement generatorElement = element.AddElement("generator").WithAtt("class", generatorClass);
			_generatorParameters.ForEachPair(
				(name, innerXml) => generatorElement.AddElement("param").WithAtt("name", name).InnerXml = innerXml);
		}

        /// <summary>
        /// Set an attribute on the xml element produced by this identity mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
	    public void SetAttribute(string name, string value)
	    {
	        _elementAttributes.Store(name, value);
	    }

	    public int Level
		{
			get { return 0; }
		}

		public void SetGeneratorClass(string generator)
		{
			_generatorClass = generator;
		}

		public void AddGeneratorParam(string name, string innerXml)
		{
			_generatorParameters.Store(name, innerXml);
		}

        /// <summary>
        /// Set the access and naming strategy for this identity.
        /// </summary>
	    public AccessStrategyBuilder<IdentityPart> Access
	    {
	        get { return access; }
	    }
	}
}
