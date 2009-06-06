namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentMapping : ComponentMappingBase
    {
        private readonly AttributeStore<ComponentMapping> attributes = new AttributeStore<ComponentMapping>();

        public ComponentMapping()
            : this(new AttributeStore())
        {}

        private ComponentMapping(AttributeStore store)
            : base(store)
        {
            attributes = new AttributeStore<ComponentMapping>(store);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public new AttributeStore<ComponentMapping> Attributes
        {
            get { return attributes; }
        }
    }
}