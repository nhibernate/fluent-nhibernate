using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class AttributeStore
    {
        private readonly IDictionary<string, object> _attributes;
        private readonly IDictionary<string, object> _defaults;

        public AttributeStore()
        {
            _attributes = new Dictionary<string, object>();
            _defaults = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                if (_attributes.ContainsKey(key))
                    return _attributes[key];
                
                if (_defaults.ContainsKey(key))
                    return _defaults[key];

                return null;
            }
            set { _attributes[key] = value; }
        }

        public bool IsSpecified(string key)
        {
            return _attributes.ContainsKey(key);
        }

        public void CopyTo(AttributeStore store)
        {
            foreach (KeyValuePair<string, object> pair in _attributes)
                store._attributes[pair.Key] = pair.Value;
        }

        public void SetDefault(string key, object value)
        {
            _defaults[key] = value;
        }
    }

    public class AttributeStore<T>
    {
        private readonly AttributeStore _store;

        public AttributeStore()
            : this(new AttributeStore())
        {

        }

        public AttributeStore(AttributeStore store)
        {
            _store = store;
        }

        public U Get<U>(Expression<Func<T, U>> exp)
        {
            return (U)(_store[GetKey(exp)] ?? default(U));
        }

        public void Set<U>(Expression<Func<T, U>> exp, U value)
        {
            _store[GetKey(exp)] = value;
        }

        public void SetDefault<U>(Expression<Func<T, U>> exp, U value)
        {
            _store.SetDefault(GetKey(exp), value);
        }

        public bool IsSpecified<U>(Expression<Func<T, U>> exp)
        {
            return _store.IsSpecified(GetKey(exp));
        }

        public void CopyTo(AttributeStore<T> target)
        {
            _store.CopyTo(target._store);
        }

        private string GetKey<U>(Expression<Func<T, U>> exp)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(exp);
            return info.Name;
        }
    }

}