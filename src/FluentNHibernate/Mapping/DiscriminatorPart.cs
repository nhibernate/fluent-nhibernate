using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart<TDiscriminator, TParent> : IMappingPart
    {
        private readonly string _columnName;
        private TDiscriminator _discriminatorValue;
		private bool _discriminatorValueSet;
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private ClassMap<TParent> _parent;

        public DiscriminatorPart(string columnName, TDiscriminator discriminatorValue, ClassMap<TParent> parent)
            : this(columnName, parent) 
		{
			if (discriminatorValue != null)
            {
				_discriminatorValue = discriminatorValue;
				_discriminatorValueSet = true;
			}
		}

        public DiscriminatorPart(string columnName, ClassMap<TParent> parent)
        {
            _columnName = columnName;
            _parent = parent;
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            string typeString = TypeMapping.GetTypeString(typeof (TDiscriminator));
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

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int LevelWithinPosition
        {
            get { return 3; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.First; }
        }

        #endregion

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TSubClass>(TDiscriminator discriminatorValue, Action<SubClassPart<TDiscriminator, TParent, TSubClass>> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TSubClass>(discriminatorValue, this);
                
            action(subclass);

            _parent.AddPart(subclass);

            return this;
        }

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TSubClass>(Action<SubClassPart<TDiscriminator, TParent, TSubClass>> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TSubClass>(this);

            action(subclass);

            _parent.AddPart(subclass);

            return this;
        }
    }
}