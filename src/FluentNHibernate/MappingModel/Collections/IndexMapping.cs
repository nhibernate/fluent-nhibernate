using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel.Collections
{
    public class IndexMapping : MappingBase
    {
        private readonly AttributeStore<IndexMapping> _attributes;

        public IndexMapping()
        {
            _attributes = new AttributeStore<IndexMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIndex(this);
        }

        public AttributeStore<IndexMapping> Attributes
        {
            get { return _attributes; }
        }

        public string Column
        {
            get { return _attributes.Get(x => x.Column); }
            set { _attributes.Set(x => x.Column, value); }
        }

        public string IndexType
        {
            get { return _attributes.Get(x => x.IndexType); }
            set { _attributes.Set(x => x.IndexType, value); }
        }

        public int Length
        {
            get { return _attributes.Get(x => x.Length); }
            set { _attributes.Set(x => x.Length, value); }
        }

        
    }
}
