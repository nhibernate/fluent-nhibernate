using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class JoinedSubclassMapping : ClassMappingBase, ISubclassMapping
    {
        private readonly AttributeStore<JoinedSubclassMapping> attributes;

        public JoinedSubclassMapping() : this(new AttributeStore())
        {}

        protected JoinedSubclassMapping(AttributeStore store) : base(store)
        {
            attributes = new AttributeStore<JoinedSubclassMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessJoinedSubclass(this);

            if(Key != null)
                visitor.Visit(Key);

            base.AcceptVisitor(visitor);
        }

        public KeyMapping Key
        {
            get { return attributes.Get(x => x.Key); }
            set { attributes.Set(x => x.Key, value); }
        }

        public string TableName
        {
            get { return attributes.Get(x => x.TableName); }
            set { attributes.Set(x => x.TableName, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public string Extends
        {
            get { return attributes.Get(x => x.Extends); }
            set { attributes.Set(x => x.Extends, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public string Proxy
        {
            get { return attributes.Get(x => x.Proxy); }
            set { attributes.Set(x => x.Proxy, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public bool DynamicUpdate
        {
            get { return attributes.Get(x => x.DynamicUpdate); }
            set { attributes.Set(x => x.DynamicUpdate, value); }
        }

        public bool DynamicInsert
        {
            get { return attributes.Get(x => x.DynamicInsert); }
            set { attributes.Set(x => x.DynamicInsert, value); }
        }

        public bool SelectBeforeUpdate
        {
            get { return attributes.Get(x => x.SelectBeforeUpdate); }
            set { attributes.Set(x => x.SelectBeforeUpdate, value); }
        }

        public bool Abstract
        {
            get { return attributes.Get(x => x.Abstract); }
            set { attributes.Set(x => x.Abstract, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<JoinedSubclassMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<JoinedSubclassMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<JoinedSubclassMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}