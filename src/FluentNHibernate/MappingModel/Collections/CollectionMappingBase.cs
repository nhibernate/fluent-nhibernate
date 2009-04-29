using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase : MappingBase, ICollectionMapping
    {
        private readonly AttributeStore<ICollectionMapping> _attributes;
        public KeyMapping Key { get; set; }
        public ICollectionContentsMapping Contents { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public CollectionMappingBase(AttributeStore underlyingStore)
        {
            _attributes = new AttributeStore<ICollectionMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Key != null)
                visitor.Visit(Key);

            if (Contents != null)
                visitor.Visit(Contents);
        }

        AttributeStore<ICollectionMapping> ICollectionMapping.Attributes
        {
            get { return _attributes; }
        }

        public bool IsLazy
        {
            get { return _attributes.Get(x => x.IsLazy); }
            set { _attributes.Set(x => x.IsLazy, value); }
        }

        public bool IsInverse
        {
            get { return _attributes.Get(x => x.IsInverse); }
            set { _attributes.Set(x => x.IsInverse, value); }
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return _attributes.IsSpecified(x => x.Name); }
        }

    }

}