using System;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class SubClassPart<TDiscriminator, TParent, TSubClass> : ClassMapBase<TSubClass>, IMappingPart
    {
        private readonly bool discriminatorSet;
        private readonly TDiscriminator _discriminator;
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly DiscriminatorPart<TDiscriminator, TParent> parent;

        public SubClassPart(DiscriminatorPart<TDiscriminator, TParent> parent)
        {
            this.parent = parent;
        }

        public SubClassPart(TDiscriminator discriminator, DiscriminatorPart<TDiscriminator, TParent> parent)
        {
            if (discriminator != null)
            {
                _discriminator = discriminator;
                discriminatorSet = true;
            }

            this.parent = parent;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement subclassElement = classElement.AddElement("subclass")
                .WithAtt("name", typeof(TSubClass).AssemblyQualifiedName)
                .WithProperties(attributes);

            if (discriminatorSet)
                subclassElement.WithAtt("discriminator-value", _discriminator.ToString());

            writeTheParts(subclassElement, visitor);
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this sub-class mapping.
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

        public int Level
        {
            get { return 3; }
        }

        public PartPosition Position
        {
            get { return PartPosition.Last; }
        }

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TChild>(TDiscriminator discriminatorValue, Action<SubClassPart<TDiscriminator, TParent, TChild>> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TChild>(discriminatorValue, parent);

            action(subclass);

            AddPart(subclass);

            return parent;
        }

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TChild>(Action<SubClassPart<TDiscriminator, TParent, TChild>> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TChild>(default(TDiscriminator), parent);

            action(subclass);

            AddPart(subclass);

            return parent;
        }
    }
}
