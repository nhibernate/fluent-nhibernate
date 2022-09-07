using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmComponentConverter : HbmConverterBase<ComponentMapping, HbmComponent>
    {
        private HbmComponent hbmComponent;

        public HbmComponentConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmComponent Convert(ComponentMapping componentMapping)
        {
            componentMapping.AcceptVisitor(this);
            return hbmComponent;
        }

        public override void ProcessComponent(ComponentMapping componentMapping)
        {
            hbmComponent = new HbmComponent();

            if (componentMapping.IsSpecified("Access"))
                hbmComponent.access = componentMapping.Access;

            if (componentMapping.IsSpecified("Class"))
                hbmComponent.@class = componentMapping.Class.ToString();

            if (componentMapping.IsSpecified("Insert"))
                hbmComponent.insert = componentMapping.Insert;

            if (componentMapping.IsSpecified("Name"))
                hbmComponent.name = componentMapping.Name;

            if (componentMapping.IsSpecified("Update"))
                hbmComponent.update = componentMapping.Update;

            if (componentMapping.IsSpecified("Lazy"))
                hbmComponent.lazy = componentMapping.Lazy;

            if (componentMapping.IsSpecified("OptimisticLock"))
                hbmComponent.optimisticlock = componentMapping.OptimisticLock;
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        public override void Visit(ParentMapping parentMapping)
        {
            hbmComponent.parent = ConvertFluentSubobjectToHibernateNative<ParentMapping, HbmParent>(parentMapping);
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmComponent.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmComponent.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }
    }
}