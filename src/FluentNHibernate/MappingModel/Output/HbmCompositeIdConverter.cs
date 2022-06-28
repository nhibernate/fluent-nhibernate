using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCompositeIdConverter : HbmConverterBase<CompositeIdMapping, HbmCompositeId>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmUnsavedValueType> unsavedDict = new XmlLinkedEnumBiDictionary<HbmUnsavedValueType>();

        private HbmCompositeId hbmCompositeId;

        public HbmCompositeIdConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmCompositeId Convert(CompositeIdMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmCompositeId;
        }

        public override void ProcessCompositeId(CompositeIdMapping compositeIdMapping)
        {
            hbmCompositeId = new HbmCompositeId();

            if (compositeIdMapping.IsSpecified("Access"))
                hbmCompositeId.access = compositeIdMapping.Access;

            if (compositeIdMapping.IsSpecified("Name"))
                hbmCompositeId.name = compositeIdMapping.Name;

            if (compositeIdMapping.IsSpecified("Class"))
                hbmCompositeId.@class = compositeIdMapping.Class.ToString();

            if (compositeIdMapping.IsSpecified("Mapped"))
                hbmCompositeId.mapped = compositeIdMapping.Mapped;

            if (compositeIdMapping.IsSpecified("UnsavedValue"))
                hbmCompositeId.unsavedvalue = unsavedDict[compositeIdMapping.UnsavedValue];
        }

        public override void Visit(KeyPropertyMapping keyPropertyMapping)
        {
            AddToNullableArray(ref hbmCompositeId.Items, ConvertFluentSubobjectToHibernateNative<KeyPropertyMapping, HbmKeyProperty>(keyPropertyMapping));
        }

        public override void Visit(KeyManyToOneMapping keyManyToOneMapping)
        {
            AddToNullableArray(ref hbmCompositeId.Items, ConvertFluentSubobjectToHibernateNative<KeyManyToOneMapping, HbmKeyManyToOne>(keyManyToOneMapping));
        }
    }
}