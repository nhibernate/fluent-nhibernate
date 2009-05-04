using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart : IDiscriminatorPart
    {
        private readonly DiscriminatorMapping mapping;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();

        public DiscriminatorPart(ClassMapping parentMapping, string columnName)
            : this(new DiscriminatorMapping(parentMapping) { ColumnName = columnName })
        {}

        public DiscriminatorPart(DiscriminatorMapping mapping)
        {
            this.mapping = mapping;
        }

        public DiscriminatorMapping GetDiscriminatorMapping()
        {
            return mapping;
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this discriminator mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public DiscriminatorPart SubClass<TSubClass>(object discriminatorValue, Action<SubClassPart<TSubClass>> action)
        {
            var subclass = new SubClassPart<TSubClass>(this, discriminatorValue);

            action(subclass);

            mapping.ParentClass.AddSubclass(subclass.GetSubclassMapping());

            return this;
        }

        public DiscriminatorPart SubClass<TSubClass>(Action<SubClassPart<TSubClass>> action)
        {
            return SubClass(null, action);
        }
    }
}