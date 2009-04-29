using System;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public interface ISubclass : IClasslike, IMappingPart
    {
        void SubClass<TChild>(object discriminatorValue, Action<ISubclass> action);
        void SubClass<TChild>(Action<ISubclass> action);

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        ISubclass LazyLoad();

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        ISubclass Not { get; }
    }

    public class SubClassPart<TDiscriminator, TParent, TSubClass> : ClasslikeMapBase<TSubClass>, ISubclass
    {
        private readonly bool discriminatorSet;
        private readonly TDiscriminator _discriminator;
        private readonly Cache<string, string> attributes = new Cache<string, string>();
        private readonly DiscriminatorPart<TDiscriminator, TParent> parent;
        private bool nextBool = true;

        public SubClassPart(DiscriminatorPart<TDiscriminator, TParent> parent)
        {
            this.parent = parent;
        }

        public SubClassPart(TDiscriminator discriminator, DiscriminatorPart<TDiscriminator, TParent> parent)
            : this(parent)
        {
            if (discriminator != null)
            {
                _discriminator = discriminator;
                discriminatorSet = true;
            }   
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

        public int LevelWithinPosition
        {
            get { return 3; }
        }

        public PartPosition PositionOnDocument
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

        void ISubclass.SubClass<TChild>(object discriminatorValue, Action<ISubclass> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TChild>((TDiscriminator)discriminatorValue, parent);

            action(subclass);

            AddPart(subclass);
        }

        public DiscriminatorPart<TDiscriminator, TParent> SubClass<TChild>(Action<SubClassPart<TDiscriminator, TParent, TChild>> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TChild>(default(TDiscriminator), parent);

            action(subclass);

            AddPart(subclass);

            return parent;
        }

        void ISubclass.SubClass<TChild>(Action<ISubclass> action)
        {
            var subclass = new SubClassPart<TDiscriminator, TParent, TChild>(default(TDiscriminator), parent);

            action(subclass);

            AddPart(subclass);
        }

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        public SubClassPart<TDiscriminator, TParent, TSubClass> LazyLoad()
        {
            attributes.Store("lazy", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public SubClassPart<TDiscriminator, TParent, TSubClass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        ISubclass ISubclass.LazyLoad()
        {
            return LazyLoad();
        }

        ISubclass ISubclass.Not
        {
            get { return Not; }
        }
    }
}
