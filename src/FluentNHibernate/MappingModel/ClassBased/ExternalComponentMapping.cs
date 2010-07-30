using System;

namespace FluentNHibernate.MappingModel.ClassBased
{
    /// <summary>
    /// A component that is declared external to a class mapping.
    /// </summary>
    [Serializable]
    public class ExternalComponentMapping : ComponentMapping
    {
        public ExternalComponentMapping(ComponentType componentType)
            : this(componentType, new AttributeStore())
        {}

        public ExternalComponentMapping(ComponentType componentType, AttributeStore underlyingStore)
            : base(componentType, underlyingStore)
        {}
    }
}