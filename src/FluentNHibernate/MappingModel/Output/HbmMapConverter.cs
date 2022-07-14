using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.MappingModel.Output.HbmCollectionConverter;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmMapConverter : HbmConverterBase<CollectionMapping, HbmMap>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCollectionFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();

        private HbmMap hbmMap;

        public HbmMapConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmMap Convert(CollectionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmMap;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            hbmMap = new HbmMap();

            #region Base collection attributes

            // Because the target Hbm* classes do not have a 'base' class that we can target, there isn't any straightforward
            // way to make this code reusable, despite being "effectively identical" (apart from the target type) across all
            // of the collection types.

            if (collectionMapping.IsSpecified("Access"))
                hbmMap.access = collectionMapping.Access;

            bool batchsizeSpecified = collectionMapping.IsSpecified("BatchSize");
            hbmMap.batchsizeSpecified = batchsizeSpecified;
            if (batchsizeSpecified)
                hbmMap.batchsize = collectionMapping.BatchSize;

            if (collectionMapping.IsSpecified("Cascade"))
                hbmMap.cascade = collectionMapping.Cascade;

            if (collectionMapping.IsSpecified("Check"))
                hbmMap.check = collectionMapping.Check;

            if (collectionMapping.IsSpecified("CollectionType") && collectionMapping.CollectionType != TypeReference.Empty)
                hbmMap.collectiontype = collectionMapping.CollectionType.ToString();

            bool fetchSpecified = collectionMapping.IsSpecified("Fetch");
            hbmMap.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmMap.fetch = LookupEnumValueIn(fetchDict, collectionMapping.Fetch);

            bool genericSpecified = collectionMapping.IsSpecified("Generic");
            hbmMap.genericSpecified = genericSpecified;
            if (genericSpecified)
                hbmMap.generic = collectionMapping.Generic;

            if (collectionMapping.IsSpecified("Inverse"))
                hbmMap.inverse = collectionMapping.Inverse;

            bool lazySpecified = collectionMapping.IsSpecified("Lazy");
            hbmMap.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmMap.lazy = LookupEnumValueIn(FluentHbmLazyBiDict, collectionMapping.Lazy);

            if (collectionMapping.IsSpecified("Name"))
                hbmMap.name = collectionMapping.Name;

            if (collectionMapping.IsSpecified("OptimisticLock"))
                hbmMap.optimisticlock = collectionMapping.OptimisticLock;

            if (collectionMapping.IsSpecified("Persister"))
                hbmMap.persister = collectionMapping.Persister.ToString();

            if (collectionMapping.IsSpecified("Schema"))
                hbmMap.schema = collectionMapping.Schema;

            if (collectionMapping.IsSpecified("TableName"))
                hbmMap.table = collectionMapping.TableName;

            if (collectionMapping.IsSpecified("Where"))
                hbmMap.where = collectionMapping.Where;

            if (collectionMapping.IsSpecified("Subselect"))
                hbmMap.subselect = collectionMapping.Subselect.ToHbmSubselect();

            #endregion Base collection attributes

            #region Type-specific collection attributes

            if (collectionMapping.IsSpecified("OrderBy"))
                hbmMap.orderby = collectionMapping.OrderBy;

            if (collectionMapping.IsSpecified("Sort"))
                hbmMap.sort = collectionMapping.Sort;

            #endregion Type-specific collection attributes
        }

        #region Base collection visitors

        public override void Visit(KeyMapping keyMapping)
        {
            hbmMap.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmMap.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            // HbmMap.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            // (ManyToMany and OneToMany are implementations of ICollectionRelationshipMapping, and there is no mapping that lines up with ManyToAny)
            hbmMap.Item1 = ConvertFluentSubobjectToHibernateNative<ICollectionRelationshipMapping, object>(collectionRelationshipMapping);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // HbmMap.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmMap.Item1 = ConvertFluentSubobjectToHibernateNative<CompositeElementMapping, HbmCompositeElement>(compositeElementMapping);
        }

        public override void Visit(ElementMapping elementMapping)
        {
            // HbmMap.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmMap.Item1 = ConvertFluentSubobjectToHibernateNative<ElementMapping, HbmElement>(elementMapping);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmMap.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }

        #endregion Base collection visitors

        #region Type-specific collection visitors

        public override void Visit(IIndexMapping indexMapping)
        {
            // HbmMap.Item is CompositeIndex / CompositeMapKey / Index / IndexManyToAny / IndexManyToMany / MapKey / MapKeyManyToMany
            // (Index and IndexManyToMany are implementations of IIndexMapping, while none of the others have fluent mappings yet)
            hbmMap.Item = ConvertFluentSubobjectToHibernateNative<IIndexMapping, object>(indexMapping);
        }

        #endregion Type-specific collection visitors
    }
}
