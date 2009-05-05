using System;

namespace FluentNHibernate.MappingModel.Collections
{
    public class OneToManyMapping : MappingBase, ICollectionContentsMapping
    {
        private readonly AttributeStore<OneToManyMapping> attributes;
        public Type ChildType { get; set; }

        public OneToManyMapping()
        {
            attributes = new AttributeStore<OneToManyMapping>();
            attributes.SetDefault(x => x.ExceptionOnNotFound, true);
        }

        public AttributeStore<OneToManyMapping> Attributes
        {
            get { return attributes; }
        }

        public string ClassName
        {
            get { return attributes.Get(x => x.ClassName); }
            set { attributes.Set(x => x.ClassName, value); }
        }

        public bool ExceptionOnNotFound
        {
            get { return attributes.Get(x => x.ExceptionOnNotFound); }
            set { attributes.Set(x => x.ExceptionOnNotFound, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessOneToMany(this);
        }
    }
}