using System;

namespace FluentNHibernate.MappingModel.Collections
{
    public class OneToManyMapping : MappingBase, ICollectionRelationshipMapping
    {
        private readonly AttributeStore<OneToManyMapping> attributes = new AttributeStore<OneToManyMapping>();
        public Type ChildType { get; set; }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessOneToMany(this);
        }

        public AttributeStore<OneToManyMapping> Attributes
        {
            get { return attributes; }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string NotFound
        {
            get { return attributes.Get(x => x.NotFound); }
            set { attributes.Set(x => x.NotFound, value); }
        }
    }
}