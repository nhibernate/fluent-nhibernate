using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmManyToManyConverter : HbmConverterBase<ManyToManyMapping, HbmManyToMany>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmFetchMode>();
        private static readonly XmlLinkedEnumBiDictionary<HbmNotFoundMode> notFoundDict = new XmlLinkedEnumBiDictionary<HbmNotFoundMode>();

        private HbmManyToMany hbmManyToMany;

        public HbmManyToManyConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmManyToMany Convert(ManyToManyMapping manyToManyMapping)
        {
            manyToManyMapping.AcceptVisitor(this);
            return hbmManyToMany;
        }

        public override void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            hbmManyToMany = new HbmManyToMany();

            if (manyToManyMapping.IsSpecified("Class"))
                hbmManyToMany.@class = manyToManyMapping.Class.ToString();

            bool fetchSpecified = manyToManyMapping.IsSpecified("Fetch");
            hbmManyToMany.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmManyToMany.fetch = LookupEnumValueIn(fetchDict, manyToManyMapping.Fetch);

            if (manyToManyMapping.IsSpecified("ForeignKey"))
                hbmManyToMany.foreignkey = manyToManyMapping.ForeignKey;

            if (manyToManyMapping.IsSpecified("ChildPropertyRef"))
                hbmManyToMany.propertyref = manyToManyMapping.ChildPropertyRef;

            bool lazySpecified = manyToManyMapping.IsSpecified("Lazy");
            hbmManyToMany.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmManyToMany.lazy = manyToManyMapping.Lazy ? HbmRestrictedLaziness.Proxy : HbmRestrictedLaziness.False;

            if (manyToManyMapping.IsSpecified("NotFound"))
                hbmManyToMany.notfound = LookupEnumValueIn(notFoundDict, manyToManyMapping.NotFound);

            if (manyToManyMapping.IsSpecified("Where"))
                hbmManyToMany.where = manyToManyMapping.Where;

            if (manyToManyMapping.IsSpecified("EntityName"))
                hbmManyToMany.entityname = manyToManyMapping.EntityName;

            if (manyToManyMapping.IsSpecified("OrderBy"))
                hbmManyToMany.orderby = manyToManyMapping.OrderBy;
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            AddToNullableArray(ref hbmManyToMany.Items, ConvertFluentSubobjectToHibernateNative<ColumnMapping, HbmColumn>(columnMapping));
        }
    }
}