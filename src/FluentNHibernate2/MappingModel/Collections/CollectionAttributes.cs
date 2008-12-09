using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentNHibernate.Reflection;
using NHibernate.Cfg.MappingSchema;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{

    public class CollectionAttributes
    {
        protected readonly ICollectionAttributesStore _store;

        protected CollectionAttributes(ICollectionAttributesStore store)
        {
            _store = store;
        }

        public CollectionAttributes(ICollectionMapping collection)
            : this(new HbmStore(collection))
        { }

        public bool IsLazy
        {
            get { return (bool)_store["lazy"]; }
            set { _store["lazy"] = value; }
        }

        public bool IsInverse
        {
            get { return (bool)_store["inverse"]; }
            set { _store["inverse"] = value; }
        }

        #region Nested Interface & Classes

        public class NonHbmBackedCache : CollectionAttributes
        {            
            public NonHbmBackedCache()
                : base(new DictionaryStore())
            {
            }

            public void CopyTo(CollectionAttributes target)
            {
                ((DictionaryStore)_store).CopyTo(target._store);
            }

        }

        protected interface ICollectionAttributesStore
        {
            object this[string key] { get; set; }
        }

        private class HbmStore : ICollectionAttributesStore
        {
            private readonly object _hbm;

            public HbmStore(ICollectionMapping collection)
            {
                _hbm = collection.Hbm;
            }

            public object this[string key]
            {
                get { return _hbm.GetType().GetField(key).GetValue(_hbm); }
                set { _hbm.GetType().GetField(key).SetValue(_hbm, value); }
            }
        }

        protected class DictionaryStore : ICollectionAttributesStore
        {
            private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();
            public object this[string key]
            {
                get { return _dictionary[key]; }
                set { _dictionary[key] = value; }
            }

            public void CopyTo(ICollectionAttributesStore targetStore)
            {
                foreach (var keyValuePair in _dictionary)
                    targetStore[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        #endregion
    }
}