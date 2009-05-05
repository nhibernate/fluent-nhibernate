namespace FluentNHibernate.MappingModel.Collections
{
    public class IndexMapping : MappingBase
    {
        private readonly AttributeStore<IndexMapping> attributes;

        public IndexMapping()
        {
            attributes = new AttributeStore<IndexMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIndex(this);
        }

        public AttributeStore<IndexMapping> Attributes
        {
            get { return attributes; }
        }

        public string Column
        {
            get { return attributes.Get(x => x.Column); }
            set { attributes.Set(x => x.Column, value); }
        }

        public string IndexType
        {
            get { return attributes.Get(x => x.IndexType); }
            set { attributes.Set(x => x.IndexType, value); }
        }

        public int Length
        {
            get { return attributes.Get(x => x.Length); }
            set { attributes.Set(x => x.Length, value); }
        }

        
    }
}
