using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmMetaValueConverter : HbmConverterBase<MetaValueMapping, HbmMetaValue>
    {
        private HbmMetaValue hbmMetaValue;

        public HbmMetaValueConverter() : base(null)
        {
        }

        public override HbmMetaValue Convert(MetaValueMapping metaValueMapping)
        {
            metaValueMapping.AcceptVisitor(this);
            return hbmMetaValue;
        }

        public override void ProcessMetaValue(MetaValueMapping metaValueMapping)
        {
            hbmMetaValue = new HbmMetaValue();

            if (metaValueMapping.IsSpecified("Class"))
                hbmMetaValue.@class = metaValueMapping.Class.ToString();

            if (metaValueMapping.IsSpecified("Value"))
                hbmMetaValue.value = metaValueMapping.Value;
        }
    }
}