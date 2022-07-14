using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.MappingModel.Output.HbmCollectionConverter;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmBagConverter : HbmConverterBase<CollectionMapping, HbmBag>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCollectionFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();

        private HbmBag hbmBag;

        public HbmBagConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmBag Convert(CollectionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmBag;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            hbmBag = new HbmBag();

            #region Base collection attributes

            // Because the target Hbm* classes do not have a 'base' class that we can target, there isn't any straightforward
            // way to make this code reusable, despite being "effectively identical" (apart from the target type) across all
            // of the collection types.

            if (collectionMapping.IsSpecified("Access"))
                hbmBag.access = collectionMapping.Access;

            bool batchsizeSpecified = collectionMapping.IsSpecified("BatchSize");
            hbmBag.batchsizeSpecified = batchsizeSpecified;
            if (batchsizeSpecified)
                hbmBag.batchsize = collectionMapping.BatchSize;

            if (collectionMapping.IsSpecified("Cascade"))
                hbmBag.cascade = collectionMapping.Cascade;

            if (collectionMapping.IsSpecified("Check"))
                hbmBag.check = collectionMapping.Check;

            if (collectionMapping.IsSpecified("CollectionType") && collectionMapping.CollectionType != TypeReference.Empty)
                hbmBag.collectiontype = collectionMapping.CollectionType.ToString();

            bool fetchSpecified = collectionMapping.IsSpecified("Fetch");
            hbmBag.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmBag.fetch = LookupEnumValueIn(fetchDict, collectionMapping.Fetch);

            bool genericSpecified = collectionMapping.IsSpecified("Generic");
            hbmBag.genericSpecified = genericSpecified;
            if (genericSpecified)
                hbmBag.generic = collectionMapping.Generic;

            if (collectionMapping.IsSpecified("Inverse"))
                hbmBag.inverse = collectionMapping.Inverse;

            bool lazySpecified = collectionMapping.IsSpecified("Lazy");
            hbmBag.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmBag.lazy = LookupEnumValueIn(FluentHbmLazyBiDict, collectionMapping.Lazy);

            if (collectionMapping.IsSpecified("Name"))
                hbmBag.name = collectionMapping.Name;

            if (collectionMapping.IsSpecified("OptimisticLock"))
                hbmBag.optimisticlock = collectionMapping.OptimisticLock;

            if (collectionMapping.IsSpecified("Persister"))
                hbmBag.persister = collectionMapping.Persister.ToString();

            if (collectionMapping.IsSpecified("Schema"))
                hbmBag.schema = collectionMapping.Schema;

            if (collectionMapping.IsSpecified("TableName"))
                hbmBag.table = collectionMapping.TableName;

            if (collectionMapping.IsSpecified("Where"))
                hbmBag.where = collectionMapping.Where;

            if (collectionMapping.IsSpecified("Subselect"))
                hbmBag.subselect = collectionMapping.Subselect.ToHbmSubselect();

            #endregion Base collection attributes

            #region Type-specific collection attributes

            if (collectionMapping.IsSpecified("OrderBy"))
                hbmBag.orderby = collectionMapping.OrderBy;

            #endregion Type-specific collection attributes
        }

        #region Base collection visitors

        public override void Visit(KeyMapping keyMapping)
        {
            hbmBag.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmBag.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            // HbmBag.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            // (ManyToMany and OneToMany are implementations of ICollectionRelationshipMapping, and there is no mapping that lines up with ManyToAny)
            hbmBag.Item = ConvertFluentSubobjectToHibernateNative<ICollectionRelationshipMapping, object>(collectionRelationshipMapping);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // HbmBag.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmBag.Item = ConvertFluentSubobjectToHibernateNative<CompositeElementMapping, HbmCompositeElement>(compositeElementMapping);
        }

        public override void Visit(ElementMapping elementMapping)
        {
            // HbmBag.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmBag.Item = ConvertFluentSubobjectToHibernateNative<ElementMapping, HbmElement>(elementMapping);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmBag.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }

        #endregion Base collection visitors

        #region Type-specific collection visitors

        // No type-specific visitors for this type

        #endregion Type-specific collection visitors
    }
}
