using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmNestedCompositeElementConverter : HbmConverterBase<NestedCompositeElementMapping, HbmNestedCompositeElement>
    {
        private HbmNestedCompositeElement hbmNestedCompositeElement;

        public HbmNestedCompositeElementConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmNestedCompositeElement Convert(NestedCompositeElementMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmNestedCompositeElement;
        }

        public override void ProcessCompositeElement(CompositeElementMapping compositeElementMapping)
        {
            // Should always be true if this converter is being invoked
            NestedCompositeElementMapping nestedCompositeElementMapping = compositeElementMapping as NestedCompositeElementMapping;

            hbmNestedCompositeElement = new HbmNestedCompositeElement();

            if (nestedCompositeElementMapping.IsSpecified("Class"))
                hbmNestedCompositeElement.@class = nestedCompositeElementMapping.Class.ToString();

            hbmNestedCompositeElement.name = nestedCompositeElementMapping.Name;
            
            if (nestedCompositeElementMapping.IsSpecified("Access"))
                hbmNestedCompositeElement.access = nestedCompositeElementMapping.Access;
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // AddCompositeElement on the mapping can only ever take a nested composite element, so this is safe
            NestedCompositeElementMapping nestedCompositeElementMapping = compositeElementMapping as NestedCompositeElementMapping;
            AddToNullableArray(ref hbmNestedCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<NestedCompositeElementMapping, HbmNestedCompositeElement>(nestedCompositeElementMapping));
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            AddToNullableArray(ref hbmNestedCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            AddToNullableArray(ref hbmNestedCompositeElement.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(ParentMapping parentMapping)
        {
            hbmNestedCompositeElement.parent = ConvertFluentSubobjectToHibernateNative<ParentMapping, HbmParent>(parentMapping);
        }
    }
}