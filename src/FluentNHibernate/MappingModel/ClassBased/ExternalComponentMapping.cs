using System;
using FluentNHibernate.Visitors;
using System.Collections.Generic;

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

        private readonly IList<IMappingModelVisitor> previousVisitors = new List<IMappingModelVisitor>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            // This external mapping might be shared amongst several ReferenceComponentMappings. Allowing a particular visitor to be applied to this component mapping
            // more than once can cause issues, so lets ensure that doesn't happen.
            if(previousVisitors.Contains(visitor))
                return;

            previousVisitors.Add(visitor);
            base.AcceptVisitor(visitor);
        }
    }
}