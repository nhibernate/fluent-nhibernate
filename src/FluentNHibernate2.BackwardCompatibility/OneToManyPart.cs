using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.BackwardCompatibility
{
    public class OneToManyPart<PARENT, CHILD> : IDeferredCollectionMapping
    {
        private readonly PropertyInfo _property;
        private readonly CollectionAttributes.NonHbmBackedCache _attributes;

        private Func<ICollectionMapping> _collectionBuilder;

        public OneToManyPart(PropertyInfo property)
        {
            _property = property;
            _attributes = new CollectionAttributes.NonHbmBackedCache();
            AsBag();   
        }

        private OneToManyPart<PARENT, CHILD> AsBag()
        {
            _collectionBuilder = () => new BagMapping();
            return this;
        }

        public OneToManyPart<PARENT, CHILD> AsSet()
        {
            _collectionBuilder = () => new SetMapping();
            return this;
        }

        public OneToManyPart<PARENT, CHILD> IsInverse()
        {
            _attributes.IsInverse = true;
            return this;
        }

        ICollectionMapping IDeferredCollectionMapping.ResolveCollectionMapping()
        {
            var collection = _collectionBuilder();
            _attributes.CopyTo(collection.Attributes);

            collection.Name = _property.Name;
            collection.Key = new KeyMapping();
            collection.Contents = new OneToManyMapping(typeof (CHILD).AssemblyQualifiedName);

            return collection;
        }

    }
}