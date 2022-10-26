using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCompositeElementConverter : HbmConverterBase<CompositeElementMapping, HbmCompositeElement>
    {
        private HbmCompositeElement hbmCompositeElement;

        public HbmCompositeElementConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmCompositeElement Convert(CompositeElementMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmCompositeElement;
        }

        public override void ProcessCompositeElement(CompositeElementMapping compositeElementMapping)
        {
            hbmCompositeElement = new HbmCompositeElement();

            if (compositeElementMapping.IsSpecified("Class"))
                hbmCompositeElement.@class = compositeElementMapping.Class.ToString();
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // AddCompositeElement on the mapping can only ever take a nested composite element, so this is safe
            NestedCompositeElementMapping nestedCompositeElementMapping = compositeElementMapping as NestedCompositeElementMapping;
            AddToNullableArray(ref hbmCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<NestedCompositeElementMapping, HbmNestedCompositeElement>(nestedCompositeElementMapping));
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            AddToNullableArray(ref hbmCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            AddToNullableArray(ref hbmCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(ParentMapping parentMapping)
        {
            hbmCompositeElement.parent = ConvertFluentSubobjectToHibernateNative<ParentMapping, HbmParent>(parentMapping);
        }
    }
}