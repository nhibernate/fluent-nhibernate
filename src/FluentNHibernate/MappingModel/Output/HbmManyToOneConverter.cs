using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmManyToOneConverter : HbmConverterBase<ManyToOneMapping, HbmManyToOne>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
        private static readonly XmlLinkedEnumBiDictionary<HbmLaziness> lazyDict = new XmlLinkedEnumBiDictionary<HbmLaziness>();
        private static readonly XmlLinkedEnumBiDictionary<HbmNotFoundMode> notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();

        private HbmManyToOne hbmManyToOne;

        public HbmManyToOneConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmManyToOne Convert(ManyToOneMapping manyToOneMapping)
        {
            manyToOneMapping.AcceptVisitor(this);
            return hbmManyToOne;
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            hbmManyToOne = new HbmManyToOne();

            if (manyToOneMapping.IsSpecified("Access"))
                hbmManyToOne.access = manyToOneMapping.Access;

            if (manyToOneMapping.IsSpecified("Cascade"))
                hbmManyToOne.cascade = manyToOneMapping.Cascade;

            if (manyToOneMapping.IsSpecified("Class"))
                hbmManyToOne.@class = manyToOneMapping.Class.ToString();

            bool fetchSpecified = manyToOneMapping.IsSpecified("Fetch");
            hbmManyToOne.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmManyToOne.fetch = LookupEnumValueIn(fetchDict, manyToOneMapping.Fetch);

            if (manyToOneMapping.IsSpecified("ForeignKey"))
                hbmManyToOne.foreignkey = manyToOneMapping.ForeignKey;

            if (manyToOneMapping.IsSpecified("Insert"))
                hbmManyToOne.insert = manyToOneMapping.Insert;

            bool lazySpecified = manyToOneMapping.IsSpecified("Lazy");
            hbmManyToOne.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmManyToOne.lazy = LookupEnumValueIn(lazyDict, manyToOneMapping.Lazy);

            if (manyToOneMapping.IsSpecified("Name"))
                hbmManyToOne.name = manyToOneMapping.Name;

            if (manyToOneMapping.IsSpecified("NotFound"))
                hbmManyToOne.notfound = LookupEnumValueIn(notFoundDict, manyToOneMapping.NotFound);

            if (manyToOneMapping.IsSpecified("Formula"))
                hbmManyToOne.formula = manyToOneMapping.Formula;

            if (manyToOneMapping.IsSpecified("PropertyRef"))
                hbmManyToOne.propertyref = manyToOneMapping.PropertyRef;

            if (manyToOneMapping.IsSpecified("Update"))
                hbmManyToOne.update = manyToOneMapping.Update;

            if (manyToOneMapping.IsSpecified("EntityName"))
                hbmManyToOne.entityname = manyToOneMapping.EntityName;

            if (manyToOneMapping.IsSpecified("OptimisticLock"))
                hbmManyToOne.optimisticlock = manyToOneMapping.OptimisticLock;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmManyToOne.Items, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}