using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmNaturalIdConverter : HbmConverterBase<NaturalIdMapping, HbmNaturalId>
    {
        private HbmNaturalId hbmNaturalId;

        public HbmNaturalIdConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmNaturalId Convert(NaturalIdMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmNaturalId;
        }

        public override void ProcessNaturalId(NaturalIdMapping naturalIdMapping)
        {
            hbmNaturalId = new HbmNaturalId();

            if (naturalIdMapping.IsSpecified("Mutable"))
                hbmNaturalId.mutable = naturalIdMapping.Mutable;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            AddToNullableArray(ref hbmNaturalId.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            AddToNullableArray(ref hbmNaturalId.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }
    }
}