namespace FluentNHibernate.MappingModel.ClassBased
{
    public class ComponentMapping : ComponentMappingBase
    {
        public ComponentMapping()
            : this(new AttributeStore())
        {}

        private ComponentMapping(AttributeStore store)
            : base(store)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }
    }
}