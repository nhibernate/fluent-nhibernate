using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmCollectionConverter : HbmConverterBase<CollectionMapping, object>
    {
        private object hbmCollection;

        public HbmCollectionConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(CollectionMapping collectionMapping)
        {
            collectionMapping.AcceptVisitor(this);
            return hbmCollection;
        }

        public override void ProcessCollection(CollectionMapping collectionMapping)
        {
            // C# doesn't allow wildcard generic types, so we have to do the assignment inline for each case,
            // rather than being able to simply assign the converter and then use it at the end.
            switch (collectionMapping.Collection)
            {
                case Collection.Array:
                    hbmCollection = ConvertFluentSubobjectToHibernateNative<CollectionMapping, HbmArray>(collectionMapping);
                    break;
                case Collection.Bag:
                    hbmCollection = ConvertFluentSubobjectToHibernateNative<CollectionMapping, HbmBag>(collectionMapping);
                    break;
                case Collection.List:
                    hbmCollection = ConvertFluentSubobjectToHibernateNative<CollectionMapping, HbmList>(collectionMapping);
                    break;
                case Collection.Map:
                    hbmCollection = ConvertFluentSubobjectToHibernateNative<CollectionMapping, HbmMap>(collectionMapping);
                    break;
                case Collection.Set:
                    hbmCollection = ConvertFluentSubobjectToHibernateNative<CollectionMapping, HbmSet>(collectionMapping);
                    break;
                default:
                    throw new InvalidOperationException("Unrecognised collection type " + collectionMapping.Collection);
            }
        }
    }
}