using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.MappingModel.Output.HbmCollectionConverter;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSetConverter : HbmConverterBase<CollectionMapping, HbmSet>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCollectionFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();

        private HbmSet hbmSet;

        public HbmSetConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmSet Convert(CollectionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmSet;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            hbmSet = new HbmSet();

            #region Base collection attributes

            // Because the target Hbm* classes do not have a 'base' class that we can target, there isn't any straightforward
            // way to make this code reusable, despite being "effectively identical" (apart from the target type) across all
            // of the collection types.

            if (collectionMapping.IsSpecified("Access"))
                hbmSet.access = collectionMapping.Access;

            bool batchsizeSpecified = collectionMapping.IsSpecified("BatchSize");
            hbmSet.batchsizeSpecified = batchsizeSpecified;
            if (batchsizeSpecified)
                hbmSet.batchsize = collectionMapping.BatchSize;

            if (collectionMapping.IsSpecified("Cascade"))
                hbmSet.cascade = collectionMapping.Cascade;

            if (collectionMapping.IsSpecified("Check"))
                hbmSet.check = collectionMapping.Check;

            if (collectionMapping.IsSpecified("CollectionType") && collectionMapping.CollectionType != TypeReference.Empty)
                hbmSet.collectiontype = collectionMapping.CollectionType.ToString();

            bool fetchSpecified = collectionMapping.IsSpecified("Fetch");
            hbmSet.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmSet.fetch = LookupEnumValueIn(fetchDict, collectionMapping.Fetch);

            bool genericSpecified = collectionMapping.IsSpecified("Generic");
            hbmSet.genericSpecified = genericSpecified;
            if (genericSpecified)
                hbmSet.generic = collectionMapping.Generic;

            if (collectionMapping.IsSpecified("Inverse"))
                hbmSet.inverse = collectionMapping.Inverse;

            bool lazySpecified = collectionMapping.IsSpecified("Lazy");
            hbmSet.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmSet.lazy = LookupEnumValueIn(FluentHbmLazyBiDict, collectionMapping.Lazy);

            if (collectionMapping.IsSpecified("Name"))
                hbmSet.name = collectionMapping.Name;

            if (collectionMapping.IsSpecified("OptimisticLock"))
                hbmSet.optimisticlock = collectionMapping.OptimisticLock;

            if (collectionMapping.IsSpecified("Persister"))
                hbmSet.persister = collectionMapping.Persister.ToString();

            if (collectionMapping.IsSpecified("Schema"))
                hbmSet.schema = collectionMapping.Schema;

            if (collectionMapping.IsSpecified("TableName"))
                hbmSet.table = collectionMapping.TableName;

            if (collectionMapping.IsSpecified("Where"))
                hbmSet.where = collectionMapping.Where;

            if (collectionMapping.IsSpecified("Subselect"))
                hbmSet.subselect = collectionMapping.Subselect.ToHbmSubselect();

            if (collectionMapping.IsSpecified("Mutable"))
                hbmSet.mutable = collectionMapping.Mutable;

            #endregion Base collection attributes

            #region Type-specific collection attributes

            if (collectionMapping.IsSpecified("OrderBy"))
                hbmSet.orderby = collectionMapping.OrderBy;

            if (collectionMapping.IsSpecified("Sort"))
                hbmSet.sort = collectionMapping.Sort;

            #endregion Type-specific collection attributes
        }

        #region Base collection visitors

        public override void Visit(KeyMapping keyMapping)
        {
            hbmSet.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmSet.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            // HbmSet.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            // (ManyToMany and OneToMany are implementations of ICollectionRelationshipMapping, and there is no mapping that lines up with ManyToAny)
            hbmSet.Item = ConvertFluentSubobjectToHibernateNative<ICollectionRelationshipMapping, object>(collectionRelationshipMapping);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // HbmSet.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmSet.Item = ConvertFluentSubobjectToHibernateNative<CompositeElementMapping, HbmCompositeElement>(compositeElementMapping);
        }

        public override void Visit(ElementMapping elementMapping)
        {
            // HbmSet.Item is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmSet.Item = ConvertFluentSubobjectToHibernateNative<ElementMapping, HbmElement>(elementMapping);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmSet.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }

        #endregion Base collection visitors

        #region Type-specific collection visitors

        // No type-specific visitors for this type

        #endregion Type-specific collection visitors
    }
}
