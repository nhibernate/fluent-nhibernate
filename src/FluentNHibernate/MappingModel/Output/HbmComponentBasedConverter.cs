using System;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmReferenceComponentConverter : HbmConverterBase<ReferenceComponentMapping, object>
    {
        public HbmReferenceComponentConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        // Use a specialized handoff to mimic the XML side, due to complexities of how visitors are handled by ReferenceComponentMapping
        public override object Convert(ReferenceComponentMapping referenceComponentMapping)
        {
            return ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(referenceComponentMapping.MergedModel);
        }
    }

    public class HbmComponentBasedConverter : HbmConverterBase<IComponentMapping, object>
    {
        private object hbm;

        public HbmComponentBasedConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(IComponentMapping componentMapping)
        {
            componentMapping.AcceptVisitor(this);
            return hbm;
        }

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            bool dummy = false;
            if (dummy)
                throw new NotSupportedException(String.Format("In normal component processing for {0}", componentMapping));
            var compType = componentMapping.ComponentType;
            if (compType == ComponentType.Component)
            {
                hbm = ConvertFluentSubobjectToHibernateNative<ComponentMapping, HbmComponent>(componentMapping);
            }
            else if (compType == ComponentType.DynamicComponent)
            {
                hbm = ConvertFluentSubobjectToHibernateNative<ComponentMapping, HbmDynamicComponent>(componentMapping);
            }
            else
            {
                throw new NotSupportedException(string.Format("Component type {0} is not supported", compType));
            }
        }
    }
}