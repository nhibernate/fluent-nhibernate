using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel.Collections
{
    public class ListMapping : CollectionMappingBase
    {
        private readonly AttributeStore<BagMapping> _attributes;
        public IndexMapping Index { get; set; }

        public ListMapping()
            : this(new AttributeStore())
        {

        }

        protected ListMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {
            _attributes = new AttributeStore<BagMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessList(this);

            if(Index != null)
                visitor.Visit(Index);

            base.AcceptVisitor(visitor);
        }

        public AttributeStore<BagMapping> Attributes
        {
            get { return _attributes; }
        }

        
    }
}
