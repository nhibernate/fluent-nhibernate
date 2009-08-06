using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Base class for attribute based conventions. Create a subclass of this to supply your own
    /// attribute based conventions.
    /// </summary>
    /// <typeparam name="T">Attribute identifier</typeparam>
    public abstract class AttributePropertyConvention<T> : IPropertyConvention, IPropertyConventionAcceptance
        where T : Attribute
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(property => Attribute.GetCustomAttribute(property.Property, typeof(T)) as T != null);
        }

        public void Apply(IPropertyInstance instance)
        {
            var attribute = Attribute.GetCustomAttribute(instance.Property, typeof(T)) as T;

            Apply(attribute, instance);
        }

        /// <summary>
        /// Apply changes to a property with an attribute matching T.
        /// </summary>
        /// <param name="attribute">Instance of attribute found on property.</param>
        /// <param name="instance">Property with attribute</param>
        protected abstract void Apply(T attribute, IPropertyInstance instance);
    }
}