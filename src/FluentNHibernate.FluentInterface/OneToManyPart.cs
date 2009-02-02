using System;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.FluentInterface
{
    public class OneToManyPart<PARENT, CHILD> : IDeferredCollectionMapping
    {
        private readonly PropertyInfo _info;
        private readonly CollectionAttributes _attributes;

        private Func<ICollectionMapping> _collectionBuilder;

        public OneToManyPart(PropertyInfo info)
        {
            _info = info;
            _attributes = new CollectionAttributes();
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

            collection.PropertyInfo = _info;            
            collection.Key = new KeyMapping();
            collection.Contents = new OneToManyMapping {ClassName = typeof (CHILD).AssemblyQualifiedName};

            return collection;
        }

    }
}