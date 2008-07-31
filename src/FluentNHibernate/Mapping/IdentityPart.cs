using System;
using System.Reflection;
using System.Xml;
using ShadeTree.Core;

namespace FluentNHibernate.Mapping
{
	public class IdentityPart : IMappingPart
	{
		private readonly string _columnName;
		private readonly IdentityGenerationStrategyBuilder _generatedBy;
		private readonly Cache<string, string> _generatorParameters = new Cache<string, string>();
		private readonly PropertyInfo _property;
		private string _generatorClass;

		public IdentityPart(PropertyInfo property, string columnName)
		{
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


			XmlElement generatorElement = element.AddElement("generator").WithAtt("class", generatorClass);
			_generatorParameters.ForEachPair(
				(name, innerXml) => generatorElement.AddElement("param").WithAtt("name", name).InnerXml = innerXml);
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
	}
}
