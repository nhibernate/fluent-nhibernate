using System;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Collections
{
    public class OneToManyMapping : MappingBase, ICollectionContentsMapping
    {
        private readonly AttributeStore<OneToManyMapping> _attributes;

        public OneToManyMapping()
        {
            _attributes = new AttributeStore<OneToManyMapping>();
        }

        public AttributeStore<OneToManyMapping> Attributes
        {
            get { return _attributes; }
        }

        public string ClassName
        {
            get { return _attributes.Get(x => x.ClassName); }
            set { _attributes.Set(x => x.ClassName, value); }
        }

        public bool ExceptionOnNotFound
        {
            get { return _attributes.Get(x => x.ExceptionOnNotFound); }
            set { _attributes.Set(x => x.ExceptionOnNotFound, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessOneToMany(this);
        }
    }
}