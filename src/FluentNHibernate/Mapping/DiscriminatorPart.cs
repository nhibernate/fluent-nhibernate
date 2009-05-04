using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart<TDiscriminator, TParent> : IMappingPart
    {
        private readonly DiscriminatorMapping mapping;
        private readonly string _columnName;
        private TDiscriminator _discriminatorValue;
		private bool _discriminatorValueSet;
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private ClassMap<TParent> _parent;

        public DiscriminatorPart(DiscriminatorMapping mapping)
        {
            this.mapping = mapping;
        }

        #region IMappingPart Members

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            
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
            var subclassMapping = new SubclassMapping
            {
                Type = typeof(TSubClass),
                DiscriminatorValue = discriminatorValue
            };

            mapping.ParentClass.AddSubclass(subclassMapping);

            var subclass = new SubClassPart<TDiscriminator, TParent, TSubClass>(subclassMapping);

            action(subclass);

            return this;
        }

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TSubClass>(Action<SubClassPart<TDiscriminator, TParent, TSubClass>> action)
        {
            var subclassMapping = new SubclassMapping
            {
                Type = typeof(TSubClass),
            };

            var subclass = new SubClassPart<TDiscriminator, TParent, TSubClass>(subclassMapping);

            action(subclass);

            return this;
        }
    }
}