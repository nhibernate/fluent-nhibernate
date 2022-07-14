using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using static FluentNHibernate.MappingModel.Output.HbmCollectionConverter;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmListConverter : HbmConverterBase<CollectionMapping, HbmList>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmCollectionFetchMode> fetchDict = new XmlLinkedEnumBiDictionary<HbmCollectionFetchMode>();

        private HbmList hbmList;

        public HbmListConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmList Convert(CollectionMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmList;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            hbmList = new HbmList();

            #region Base collection attributes

            // Because the target Hbm* classes do not have a 'base' class that we can target, there isn't any straightforward
            // way to make this code reusable, despite being "effectively identical" (apart from the target type) across all
            // of the collection types.

            if (collectionMapping.IsSpecified("Access"))
                hbmList.access = collectionMapping.Access;

            bool batchsizeSpecified = collectionMapping.IsSpecified("BatchSize");
            hbmList.batchsizeSpecified = batchsizeSpecified;
            if (batchsizeSpecified)
                hbmList.batchsize = collectionMapping.BatchSize;

            if (collectionMapping.IsSpecified("Cascade"))
                hbmList.cascade = collectionMapping.Cascade;

            if (collectionMapping.IsSpecified("Check"))
                hbmList.check = collectionMapping.Check;

            if (collectionMapping.IsSpecified("CollectionType") && collectionMapping.CollectionType != TypeReference.Empty)
                hbmList.collectiontype = collectionMapping.CollectionType.ToString();

            bool fetchSpecified = collectionMapping.IsSpecified("Fetch");
            hbmList.fetchSpecified = fetchSpecified;
            if (fetchSpecified)
                hbmList.fetch = LookupEnumValueIn(fetchDict, collectionMapping.Fetch);

            bool genericSpecified = collectionMapping.IsSpecified("Generic");
            hbmList.genericSpecified = genericSpecified;
            if (genericSpecified)
                hbmList.generic = collectionMapping.Generic;

            if (collectionMapping.IsSpecified("Inverse"))
                hbmList.inverse = collectionMapping.Inverse;

            bool lazySpecified = collectionMapping.IsSpecified("Lazy");
            hbmList.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmList.lazy = LookupEnumValueIn(FluentHbmLazyBiDict, collectionMapping.Lazy);

            if (collectionMapping.IsSpecified("Name"))
                hbmList.name = collectionMapping.Name;

            if (collectionMapping.IsSpecified("OptimisticLock"))
                hbmList.optimisticlock = collectionMapping.OptimisticLock;

            if (collectionMapping.IsSpecified("Persister"))
                hbmList.persister = collectionMapping.Persister.ToString();

            if (collectionMapping.IsSpecified("Schema"))
                hbmList.schema = collectionMapping.Schema;

            if (collectionMapping.IsSpecified("TableName"))
                hbmList.table = collectionMapping.TableName;

            if (collectionMapping.IsSpecified("Where"))
                hbmList.where = collectionMapping.Where;

            if (collectionMapping.IsSpecified("Subselect"))
                hbmList.subselect = collectionMapping.Subselect.ToHbmSubselect();

            #endregion Base collection attributes

            #region Type-specific collection attributes

            // No type-specific attributes for this type

            #endregion Type-specific collection attributes
        }

        #region Base collection visitors

        public override void Visit(KeyMapping keyMapping)
        {
            hbmList.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmList.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ICollectionRelationshipMapping collectionRelationshipMapping)
        {
            // HbmList.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            // (ManyToMany and OneToMany are implementations of ICollectionRelationshipMapping, and there is no mapping that lines up with ManyToAny)
            hbmList.Item1 = ConvertFluentSubobjectToHibernateNative<ICollectionRelationshipMapping, object>(collectionRelationshipMapping);
        }

        public override void Visit(CompositeElementMapping compositeElementMapping)
        {
            // HbmList.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmList.Item1 = ConvertFluentSubobjectToHibernateNative<CompositeElementMapping, HbmCompositeElement>(compositeElementMapping);
        }

        public override void Visit(ElementMapping elementMapping)
        {
            // HbmList.Item1 is CompositeElement / Element / ManyToAny / ManyToMany / OneToMany
            hbmList.Item1 = ConvertFluentSubobjectToHibernateNative<ElementMapping, HbmElement>(elementMapping);
        }

        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmList.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }

        #endregion Base collection visitors

        #region Type-specific collection visitors

        public override void Visit(IIndexMapping indexMapping)
        {
            // HbmList.Item is Index / ListIndex
            // (Index is an implementation of IIndexMapping, while ListIndex does not have a fluent mapping yet)
            hbmList.Item = ConvertFluentSubobjectToHibernateNative<IIndexMapping, object>(indexMapping);
        }

        #endregion Type-specific collection visitors
    }
}
