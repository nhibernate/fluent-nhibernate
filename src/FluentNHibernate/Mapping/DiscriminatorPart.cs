using System;
using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart<T, PARENT> : IMappingPart
    {
        private readonly string _columnName;
        private readonly List<IMappingPart> _properties;
        private T _discriminatorValue;
		private bool _discriminatorValueSet;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public DiscriminatorPart(string columnName, List<IMappingPart> properties, T discriminatorValue) : this(columnName, properties) 
		{
			if (discriminatorValue != null)
            {
				_discriminatorValue = discriminatorValue;
				_discriminatorValueSet = true;
			}
		}

        public DiscriminatorPart(string columnName, List<IMappingPart> _properties)
        {
            _columnName = columnName;
            this._properties = _properties;
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            string typeString = TypeMapping.GetTypeString(typeof (T));
            classElement.AddElement("discriminator")
                .WithAtt("column", _columnName)
                .WithAtt("type", typeString)
                .WithProperties(attributes);

            if (_discriminatorValueSet) 
				classElement.WithAtt("discriminator-value", _discriminatorValue.ToString());
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this discriminator mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public int Level
        {
            get { return 1; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Anywhere; }
        }

        #endregion

        public SubClassExpression<T, SUBCLASS> SubClass<SUBCLASS>()
        {
            return new SubClassExpression<T, SUBCLASS>(this);
        }

        #region Nested type: SubClassExpression

        public class SubClassExpression<DISC, SUBCLASS>
        {
            private readonly DiscriminatorPart<T, PARENT> _parent;
            private string _discriminatorValue;

            public SubClassExpression(DiscriminatorPart<T, PARENT> parent)
            {
                _parent = parent;
            }

            public SubClassExpression<DISC, SUBCLASS> IsIdentifiedBy(DISC discriminator)
            {
                _discriminatorValue = discriminator.ToString();
                return this;
            }

            public DiscriminatorPart<T, PARENT> MapSubClassColumns(Action<SubClassPart<SUBCLASS>> action)
            {
                var subclass = new SubClassPart<SUBCLASS>(_discriminatorValue);
                action(subclass);

                _parent._properties.Add(subclass);

                return _parent;
            }
        }

        #endregion
    }
}
