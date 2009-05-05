using System;

namespace FluentNHibernate.MappingModel.Collections
{
    public class ManyToManyMapping : MappingBase, ICollectionContentsMapping
    {
        private readonly AttributeStore<ManyToManyMapping> attributes;
        public Type ParentType { get; set; }
        public Type ChildType { get; set; }

        public ManyToManyMapping()
        {
            attributes = new AttributeStore<ManyToManyMapping>();
        }

        public AttributeStore<ManyToManyMapping> Attributes
        {
            get { return attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToMany(this);
        }

        public string ClassName
        {
            get { return attributes.Get(x => x.ClassName); }
            set { attributes.Set(x => x.ClassName, value); }
        }
    }
}
