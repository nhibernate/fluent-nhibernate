namespace FluentNHibernate.MappingModel
{
    public class CacheMapping : MappingBase
    {
        private readonly AttributeStore<CacheMapping> attributes = new AttributeStore<CacheMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessCache(this);
        }

        public string Region
        {
            get { return attributes.Get(x => x.Region); }
            set { attributes.Set(x => x.Region, value); }
        }

        public string Usage
        {
            get { return attributes.Get(x => x.Usage); }
            set { attributes.Set(x => x.Usage, value); }
        }

        public AttributeStore<CacheMapping> Attributes
        {
            get { return attributes; }
        }
    }
}