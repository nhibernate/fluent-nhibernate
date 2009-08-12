using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel
{
    public class AttributeStore
    {
        private readonly IDictionary<string, object> attributes;
        private readonly IDictionary<string, object> defaults;

        public AttributeStore()
        {
            attributes = new Dictionary<string, object>();
            defaults = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get
            {
                if (attributes.ContainsKey(key))
                    return attributes[key];
                
                if (defaults.ContainsKey(key))
                    return defaults[key];

                return null;
            }
            set { attributes[key] = value; }
        }

        public bool IsSpecified(string key)
        {
            return attributes.ContainsKey(key);
        }

        public bool HasValue(string key)
        {
            return attributes.ContainsKey(key) || defaults.ContainsKey(key);
        }

        public void CopyTo(AttributeStore store)
        {
            foreach (var pair in attributes)
                store.attributes[pair.Key] = pair.Value;

            foreach (var pair in defaults)
                store.defaults[pair.Key] = pair.Value;
        }

        public void SetDefault(string key, object value)
        {
            defaults[key] = value;
        }

        public void Merge(AttributeStore otherStore)
        {
            foreach (var key in otherStore.defaults.Keys)
                defaults[key] = otherStore.defaults[key];

            foreach (var key in otherStore.attributes.Keys)
                attributes[key] = otherStore.attributes[key];
        }
    }

    public class AttributeStore<T>
    {
        private readonly AttributeStore store;

        public AttributeStore()
            : this(new AttributeStore())
        {

        }

        public AttributeStore(AttributeStore store)
        {
            this.store = store;
        }

        public TResult Get<TResult>(Expression<Func<T, TResult>> exp)
        {
            return (TResult)(store[GetKey(exp)] ?? default(TResult));
        }

        public void Set<TResult>(Expression<Func<T, TResult>> exp, TResult value)
        {
            store[GetKey(exp)] = value;
        }

        public void SetDefault<TResult>(Expression<Func<T, TResult>> exp, TResult value)
        {
            store.SetDefault(GetKey(exp), value);
        }

        /// <summary>
        /// Returns whether the user has set a value for a property.
        /// </summary>
        public bool IsSpecified<TResult>(Expression<Func<T, TResult>> exp)
        {
            return store.IsSpecified(GetKey(exp));
        }

        /// <summary>
        /// Returns whether a property has any value, default or user specified.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool HasValue<TResult>(Expression<Func<T, TResult>> exp)
        {
            return store.HasValue(GetKey(exp));
        }

        public void CopyTo(AttributeStore<T> target)
        {
            store.CopyTo(target.store);
        }

        private static string GetKey<TResult>(Expression<Func<T, TResult>> exp)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(exp);
            return info.Name;
        }

        public AttributeStore<T> Clone()
        {
            var clonedStore = new AttributeStore<T>();

            store.CopyTo(clonedStore.store);

            return clonedStore;
        }

        public AttributeStore CloneInner()
        {
            var clonedStore = new AttributeStore();

            store.CopyTo(clonedStore);

            return clonedStore;
        }

        public void Merge(AttributeStore<T> otherStore)
        {
            store.Merge(otherStore.store);
        }
    }

}