namespace FluentNHibernate.MappingModel
{
    public class OneToOneMapping : MappingBase
    {
        private readonly AttributeStore<OneToOneMapping> attributes = new AttributeStore<OneToOneMapping>();
        
        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessOneToOne(this);
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string Cascade
        {
            get { return attributes.Get(x => x.Cascade); }
            set { attributes.Set(x => x.Cascade, value); }
        }
        public bool Constrained
        {
            get { return attributes.Get(x => x.Constrained); }
            set { attributes.Set(x => x.Constrained, value); }
        }

        public string OuterJoin
        {
            get { return attributes.Get(x => x.OuterJoin); }
            set { attributes.Set(x => x.OuterJoin, value); }
        }

        public string Fetch
        {
            get { return attributes.Get(x => x.Fetch); }
            set { attributes.Set(x => x.Fetch, value); }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyRef
        {
            get { return attributes.Get(x => x.PropertyRef); }
            set { attributes.Set(x => x.PropertyRef, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public AttributeStore<OneToOneMapping> Attributes
        {
            get { return attributes; }
        }
    }
}