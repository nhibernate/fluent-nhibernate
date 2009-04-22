using System;

namespace FluentNHibernate.MappingModel
{
    public class KeyMapping : MappingBase
    {
        private readonly AttributeStore<KeyMapping> _attributes;

        public KeyMapping()
        {
            _attributes = new AttributeStore<KeyMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKey(this);
        }

        public AttributeStore<KeyMapping> Attributes
        {
            get { return _attributes; }
        }

        public string Column
        {
            get { return _attributes.Get(x => x.Column); }
            set { _attributes.Set(x => x.Column, value); }
        }

        public string ForeignKey
        {
            get { return _attributes.Get(x => x.ForeignKey); }
            set { _attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyReference
        {
            get { return _attributes.Get(x => x.PropertyReference); }
            set { _attributes.Set(x => x.PropertyReference, value); }
        }

        public bool CascadeOnDelete
        {
            get { return _attributes.Get(x => x.CascadeOnDelete); }
            set { _attributes.Set(x => x.CascadeOnDelete, value); }
        }


    }
}