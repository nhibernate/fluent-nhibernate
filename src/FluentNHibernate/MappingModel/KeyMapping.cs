namespace FluentNHibernate.MappingModel
{
    public class KeyMapping : MappingBase
    {
        private readonly AttributeStore<KeyMapping> attributes;

        public KeyMapping()
        {
            attributes = new AttributeStore<KeyMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessKey(this);
        }

        public AttributeStore<KeyMapping> Attributes
        {
            get { return attributes; }
        }

        public string Column
        {
            get { return attributes.Get(x => x.Column); }
            set { attributes.Set(x => x.Column, value); }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyReference
        {
            get { return attributes.Get(x => x.PropertyReference); }
            set { attributes.Set(x => x.PropertyReference, value); }
        }

        public bool CascadeOnDelete
        {
            get { return attributes.Get(x => x.CascadeOnDelete); }
            set { attributes.Set(x => x.CascadeOnDelete, value); }
        }
    }
}