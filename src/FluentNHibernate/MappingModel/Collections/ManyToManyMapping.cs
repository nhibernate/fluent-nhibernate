using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel.Collections
{
    public class ManyToManyMapping : MappingBase, ICollectionContentsMapping
    {
        private readonly AttributeStore<ManyToManyMapping> _attributes;
        public Type ParentType { get; set; }
        public Type ChildType { get; set; }

        public ManyToManyMapping()
        {
            _attributes = new AttributeStore<ManyToManyMapping>();
        }

        public AttributeStore<ManyToManyMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToMany(this);
        }

        public string ClassName
        {
            get { return _attributes.Get(x => x.ClassName); }
            set { _attributes.Set(x => x.ClassName, value); }
        }
    }
}
