namespace FluentNHibernate.MappingModel
{
    public class ComponentMapping : ComponentMappingBase<ComponentMapping>
    {
        public ComponentMapping()
            : this(new AttributeStore())
        {}

        private ComponentMapping(AttributeStore store)
            : base(store)
        {
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }
    }
}
