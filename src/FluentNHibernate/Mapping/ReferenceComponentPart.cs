using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// The fluent-interface part for a external component reference. These are
    /// components which have their bulk/body declared external to a class mapping
    /// and are reusable.
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    public class ReferenceComponentPart<T> : IReferenceComponentMappingProvider
    {
        private readonly Member property;
        private readonly Type containingEntityType;

        public ReferenceComponentPart(Member property, Type containingEntityType)
        {
            this.property = property;
            this.containingEntityType = containingEntityType;
        }

        public IComponentMapping GetComponentMapping()
        {
            return new ReferenceComponentMapping(property, typeof(T), containingEntityType);
        }

        public Type Type
        {
            get { return typeof(T); }
        }
    }
}