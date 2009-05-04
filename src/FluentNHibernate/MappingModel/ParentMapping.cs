namespace FluentNHibernate.MappingModel
{
    public class ParentMapping : MappingBase
    {
        private readonly AttributeStore<ParentMapping> attributes = new AttributeStore<ParentMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessParent(this);
        }

        public AttributeStore<ParentMapping> Attributes
        {
            get { return attributes; }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }
    }
}