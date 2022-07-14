using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmArrayConverter : HbmConverterBase<CollectionMapping, HbmArray>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCollectionFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();

        private HbmArray hbmArray;

        public HbmArrayConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmArray Convert(CollectionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmArray;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            hbmArray = new HbmArray();

            #region Base collection attributes

            // Because the target Hbm* classes do not have a 'base' class that we can target, there isn't any straightforward
            // way to make this code reusable, despite being "effectively identical" (apart from the target type) across all
            // of the collection types.

            if (collectionMapping.IsSpecified("Access"))
                hbmArray.access = collectionMapping.Access;

            bool batchsizeSpecified = collectionMapping.IsSpecified("BatchSize");
            hbmArray.batchsizeSpecified = batchsizeSpecified;
            if (batchsizeSpecified)
                hbmArray.batchsize = collectionMapping.BatchSize;

            if (collectionMapping.IsSpecified("Cascade"))
                hbmArray.cascade = collectionMapping.Cascade;

            if (collectionMapping.IsSpecified("Check"))
                hbmArray.check = collectionMapping.Check;

            if (collectionMapping.IsSpecified("CollectionType") && collectionMapping.CollectionType != TypeReference.Empty)
                hbmArray.collectiontype = collectionMapping.CollectionType.ToString();

            bool fetchSpecified = collectionMapping.IsSpecified("Fetch");
            hbmArray.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmArray.fetch = LookupEnumValueIn(fetchDict, collectionMapping.Fetch);

            // HbmArray, unlike HbmList, doesn't support the generic attribute
            /*
            bool genericSpecified = collectionMapping.IsSpecified("Generic");
            hbmArray.genericSpecified = genericSpecified;
            if (genericSpecified)
                hbmArray.generic = collectionMapping.Generic;
            */

            if (collectionMapping.IsSpecified("Inverse"))
                hbmArray.inverse = collectionMapping.Inverse;

            // HbmArray, unlike HbmList, doesn't support the lazy attribute
            /*
            bool lazySpecified = collectionMapping.IsSpecified("Lazy");
            hbmArray.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmArray.lazy = LookupEnumValueIn(FluentHbmLazyBiDict, collectionMapping.Lazy);
            */

            if (collectionMapping.IsSpecified("Name"))
                hbmArray.name = collectionMapping.Name;

            if (collectionMapping.IsSpecified("OptimisticLock"))
                hbmArray.optimisticlock = collectionMapping.OptimisticLock;

            if (collectionMapping.IsSpecified("Persister"))
                hbmArray.persister = collectionMapping.Persister.ToString();

            if (collectionMapping.IsSpecified("Schema"))
                hbmArray.schema = collectionMapping.Schema;

            if (collectionMapping.IsSpecified("TableName"))
                hbmArray.table = collectionMapping.TableName;

            if (collectionMapping.IsSpecified("Where"))
                hbmArray.where = collectionMapping.Where;

            if (collectionMapping.IsSpecified("Subselect"))
                hbmArray.subselect = collectionMapping.Subselect.ToHbmSubselect();

            #endregion Base collection attributes

            #region Type-specific collection attributes

            // No type-specific attributes for this type

            #endregion Type-specific collection attributes
        }

        #region Base collection visitors

        public override void Visit(KeyMapping keyMapping)
        {
            hbmArray.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmArray.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            // HbmArray.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            // (ManyToMany and OneToMany are implementations of ICollectionRelationshipMapping, and there is no mapping that lines up with ManyToAny)
            hbmArray.Item1 = ConvertFluentSubobjectToHibernateNative<ICollectionRelationshipMapping, object>(collectionRelationshipMapping);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // HbmArray.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmArray.Item1 = ConvertFluentSubobjectToHibernateNative<CompositeElementMapping, HbmCompositeElement>(compositeElementMapping);
        }

        public override void Visit(ElementMapping elementMapping)
        {
            // HbmArray.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmArray.Item1 = ConvertFluentSubobjectToHibernateNative<ElementMapping, HbmElement>(elementMapping);
        }

        // HbmArray, unlike HbmList, doesn't support filters
        /*
        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmArray.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }
        */

        #endregion Base collection visitors

        #region Type-specific collection visitors

        public override void Visit(IIndexMapping indexMapping)
        {
            // HbmArray.Item is Index / ArrayIndex
            // (Index is an implementation of IIndexMapping, while ArrayIndex does not have a fluent mapping yet)
            hbmArray.Item = ConvertFluentSubobjectToHibernateNative<IIndexMapping, object>(indexMapping);
        }

        #endregion Type-specific collection visitors
    }
}
