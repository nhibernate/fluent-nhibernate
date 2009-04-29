using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class SetMapping : CollectionMappingBase
    {
        private readonly AttributeStore<SetMapping> _attributes;

        public SetMapping() : this(new AttributeStore())
        {
            
        }
        
        protected SetMapping(AttributeStore underlyingStore) : base(underlyingStore)
        {
            _attributes = new AttributeStore<SetMapping>(underlyingStore);
        }

        public AttributeStore<SetMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessSet(this);
            base.AcceptVisitor(visitor);
        }

        public string OrderBy
        {
            get { return _attributes.Get(x => x.OrderBy); }
            set { _attributes.Set(x => x.OrderBy, value); }
        }
    }
}