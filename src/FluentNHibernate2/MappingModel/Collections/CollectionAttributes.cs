using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentNHibernate.Reflection;
using NHibernate.Cfg.MappingSchema;
using System.Reflection;
using System.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public class CollectionAttributes
    {
        private readonly AttributeStore<CollectionMappingBase> _store;

        public CollectionAttributes()
            : this(new AttributeStore())
        {

        }

        public CollectionAttributes(AttributeStore underlyingStore)
        {
            _store = new AttributeStore<CollectionMappingBase>(underlyingStore);
        }

        public void CopyTo(CollectionAttributes target)
        {
            _store.CopyTo(target._store);
        }

        public bool IsLazy
        {
            get { return _store.Get(x => x.IsLazy); }
            set { _store.Set(x => x.IsLazy, value); }
        }

        public bool IsInverse
        {
            get { return _store.Get(x => x.IsInverse); }
            set { _store.Set(x => x.IsInverse, value); }
        }

        public string Name
        {
            get { return _store.Get(x => x.Name); }
            set { _store.Set(x => x.Name, value); }
        }

        public bool IsSpecified(Expression<Func<CollectionMappingBase, object>> exp)
        {
            return _store.IsSpecified(exp);
        }
    

    }

    

}