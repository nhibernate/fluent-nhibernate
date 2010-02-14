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
    public abstract class AttributeCollectionConvention<T> : ICollectionConvention, ICollectionConventionAcceptance
        where T : Attribute
    {
        public void Accept(IAcceptanceCriteria<ICollectionInspector> criteria)
        {
            criteria.Expect(property => Attribute.GetCustomAttribute(property.Member, typeof(T)) as T != null);
        }

        public void Apply(ICollectionInstance instance)
        {
            var attribute = Attribute.GetCustomAttribute(instance.Member, typeof(T)) as T;

            Apply(attribute, instance);
        }

        /// <summary>
        /// Apply changes to a property with an attribute matching T.
        /// </summary>
        /// <param name="attribute">Instance of attribute found on property.</param>
        /// <param name="instance">Property with attribute</param>
        protected abstract void Apply(T attribute, ICollectionInstance instance);
    }
}