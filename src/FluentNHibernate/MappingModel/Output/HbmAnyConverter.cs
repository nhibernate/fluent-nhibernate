using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmAnyConverter : HbmConverterBase<AnyMapping, HbmAny>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
        private static readonly XmlLinkedEnumBiDictionary<HbmLaziness> lazyDict = new XmlLinkedEnumBiDictionary<HbmLaziness>();
        private static readonly XmlLinkedEnumBiDictionary<HbmNotFoundMode> notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();

        private HbmAny hbmAny;

        public HbmAnyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmAny Convert(AnyMapping anyMapping)
        {
            anyMapping.AcceptVisitor(this);
            return hbmAny;
        }

        public override void ProcessAny(AnyMapping anyMapping)
        {
            hbmAny = new HbmAny();

            if (anyMapping.IsSpecified("Access"))
                hbmAny.access = anyMapping.Access;

            if (anyMapping.IsSpecified("Cascade"))
                hbmAny.cascade = anyMapping.Cascade;

            if (anyMapping.IsSpecified("IdType"))
                hbmAny.idtype = anyMapping.IdType;

            if (anyMapping.IsSpecified("Insert"))
                hbmAny.insert = anyMapping.Insert;

            if (anyMapping.IsSpecified("MetaType"))
                hbmAny.metatype = anyMapping.MetaType.ToString();

            if (anyMapping.IsSpecified("Name"))
                hbmAny.name = anyMapping.Name;

            if (anyMapping.IsSpecified("Update"))
                hbmAny.update = anyMapping.Update;

            if (anyMapping.IsSpecified("Lazy"))
                hbmAny.lazy = anyMapping.Lazy;

            if (anyMapping.IsSpecified("OptimisticLock"))
                hbmAny.optimisticlock = anyMapping.OptimisticLock;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmAny.column, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }

        public override void Visit(MetaValueMapping metaValueMapping)
        {
            AddToNullableArray(ref hbmAny.metavalue, ConvertFluentSubobjectToHibernateNative<MetaValueMapping, HbmMetaValue>(metaValueMapping));
        }
    }
}