using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmOneToOneConverter : HbmConverterBase<OneToOneMapping, HbmOneToOne>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
        private static readonly XmlLinkedEnumBiDictionary<HbmLaziness> lazyDict = new XmlLinkedEnumBiDictionary<HbmLaziness>();

        private HbmOneToOne hbmOneToOne;

        public HbmOneToOneConverter() : base(null)
        {
        }

        public override HbmOneToOne Convert(OneToOneMapping oneToOneMapping)
        {
            oneToOneMapping.AcceptVisitor(this);
            return hbmOneToOne;
        }

        public override void ProcessOneToOne(OneToOneMapping oneToOneMapping)
        {
            hbmOneToOne = new HbmOneToOne();

            if (oneToOneMapping.IsSpecified("Access"))
                hbmOneToOne.access = oneToOneMapping.Access;

            if (oneToOneMapping.IsSpecified("Cascade"))
                hbmOneToOne.cascade = oneToOneMapping.Cascade;

            if (oneToOneMapping.IsSpecified("Class"))
                hbmOneToOne.@class = oneToOneMapping.Class.ToString();

            if (oneToOneMapping.IsSpecified("Constrained"))
                hbmOneToOne.constrained = oneToOneMapping.Constrained;

            bool fetchSpecified = oneToOneMapping.IsSpecified("Fetch");
            hbmOneToOne.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmOneToOne.fetch = LookupEnumValueIn(fetchDict, oneToOneMapping.Fetch);

            if (oneToOneMapping.IsSpecified("ForeignKey"))
                hbmOneToOne.foreignkey = oneToOneMapping.ForeignKey;

            bool lazySpecified = oneToOneMapping.IsSpecified("Lazy");
            hbmOneToOne.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmOneToOne.lazy = LookupEnumValueIn(lazyDict, oneToOneMapping.Lazy);

            if (oneToOneMapping.IsSpecified("Name"))
                hbmOneToOne.name = oneToOneMapping.Name;

            if (oneToOneMapping.IsSpecified("PropertyRef"))
                hbmOneToOne.propertyref = oneToOneMapping.PropertyRef;

            if (oneToOneMapping.IsSpecified("EntityName"))
                hbmOneToOne.entityname = oneToOneMapping.EntityName;
        }
    }
}