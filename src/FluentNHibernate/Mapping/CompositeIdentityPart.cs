using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
	public class KeyProperty : IMappingPart
	{
		public KeyProperty(PropertyInfo property, bool isReference)
		{
			Property = property;
			IsReference = isReference;
		}

		public string ColumnName { get; set; }
		public PropertyInfo Property { get; set; }
		public bool IsReference { get; set; }

		public void SetAttribute(string name, string value)
		{
			throw new NotImplementedException();
		}

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

		public void Write(XmlElement idElement, IMappingVisitor visitor)
		{
			XmlElement element = 
				(IsReference)
					? idElement.AddElement("key-many-to-one").WithAtt("class", Property.PropertyType.AssemblyQualifiedName)
					: idElement.AddElement("key-property").WithAtt("type", TypeMapping.GetTypeString(Property.PropertyType));

			element.WithAtt("name", Property.Name);
			
			if( ColumnName != null )
			{
				element.WithAtt("column", ColumnName);
			}
		}

		public int Level
		{
			get { return 0; }
		}

	    public PartPosition Position
	    {
            get { return PartPosition.Anywhere; }
	    }
	}

	public class CompositeIdentityPart<T> : IMappingPart, IAccessStrategy<CompositeIdentityPart<T>>
	{
		private readonly AccessStrategyBuilder<CompositeIdentityPart<T>> _access;
		private readonly IList<KeyProperty> _keyProperties;

		public CompositeIdentityPart()
		{
			_keyProperties = new List<KeyProperty>();
			_access = new AccessStrategyBuilder<CompositeIdentityPart<T>>(this);
		}

		public void SetAttribute(string name, string value)
		{
			throw new System.NotImplementedException();
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
			XmlElement element = classElement.AddElement("composite-id");

			foreach( var keyProp in _keyProperties )
			{
				keyProp.Write(element, visitor);
			}
            
			//if (_unsavedValue != null)
			//    element.WithAtt("unsaved-value", _unsavedValue.ToString());

			//_elementAttributes.ForEachPair((name, value) => element.WithAtt(name, value));

			//XmlElement generatorElement = element.AddElement("generator").WithAtt("class", generatorClass);
			//_generatorParameters.ForEachPair(
			//    (name, innerXml) => generatorElement.AddElement("param").WithAtt("name", name).InnerXml = innerXml);
		}

		public int Level
		{
			get { return 2; }
		}

	    public PartPosition Position
	    {
            get { return PartPosition.First; }
	    }

	    /// <summary>
		/// Defines a property to be used as a key for this composite-id.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyProperty(Expression<Func<T, object>> expression)
		{
			return WithKeyProperty(expression, null);
		}

		/// <summary>
		/// Defines a property to be used as a key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyProperty(Expression<Func<T, object>> expression, string columnName)
		{
			var prop = ReflectionHelper.GetProperty(expression);
			
			var keyProp = new KeyProperty(prop, false)
			              	{
			              		ColumnName = columnName
			              	};

			_keyProperties.Add( keyProp );

			return this;
		}

		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyReference(Expression<Func<T, object>> expression)
		{
			return WithKeyReference(expression, null);
		}


		/// <summary>
		/// Defines a reference to be used as a many-to-one key for this composite-id with an explicit column name.
		/// </summary>
		/// <param name="expression">A member access lambda expression for the property</param>
		/// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
		/// <returns>The composite identity part fluent interface</returns>
		public CompositeIdentityPart<T> WithKeyReference(Expression<Func<T, object>> expression, string columnName)
		{
			var prop = ReflectionHelper.GetProperty(expression);

			var keyProp = new KeyProperty(prop, true)
			              	{
			              		ColumnName = columnName
			              	};

			_keyProperties.Add(keyProp);

			return this;
		}

		/// <summary>
		/// Set the access and naming strategy for this identity.
		/// </summary>
		public AccessStrategyBuilder<CompositeIdentityPart<T>> Access
		{
			get { return _access; }
		}
	}
}
