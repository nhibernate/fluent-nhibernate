using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmDynamicComponentConverter : HbmConverterBase<ComponentMapping, HbmDynamicComponent>
    {
        private HbmDynamicComponent hbmDynamicComponent;

        public HbmDynamicComponentConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmDynamicComponent Convert(ComponentMapping dynamicComponentMapping)
        {
            dynamicComponentMapping.AcceptVisitor(this);
            return hbmDynamicComponent;
        }

        public override void ProcessComponent(ComponentMapping dynamicComponentMapping)
        {
            hbmDynamicComponent = new HbmDynamicComponent();

            if (dynamicComponentMapping.IsSpecified("Access"))
                hbmDynamicComponent.access = dynamicComponentMapping.Access;

            if (dynamicComponentMapping.IsSpecified("Insert"))
                hbmDynamicComponent.insert = dynamicComponentMapping.Insert;

            if (dynamicComponentMapping.IsSpecified("Name"))
                hbmDynamicComponent.name = dynamicComponentMapping.Name;

            if (dynamicComponentMapping.IsSpecified("Update"))
                hbmDynamicComponent.update = dynamicComponentMapping.Update;

            if (dynamicComponentMapping.IsSpecified("OptimisticLock"))
                hbmDynamicComponent.optimisticlock = dynamicComponentMapping.OptimisticLock;
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            // (DynamicComponentMapping, ExternalDynamicComponentMapping, and ReferenceDynamicComponentMapping are implementations of IDynamicComponentMapping, while 
            // DynamicDynamicComponentMapping is a refinement of DynamicComponentMapping)
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmDynamicComponent.Items is Any / Array / Bag / Component / DynamicComponent / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmDynamicComponent.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }
    }
}