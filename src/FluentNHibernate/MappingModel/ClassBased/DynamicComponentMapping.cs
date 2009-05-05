namespace FluentNHibernate.MappingModel.ClassBased
{
    public class DynamicComponentMapping : ComponentMappingBase<DynamicComponentMapping>
    {

        public DynamicComponentMapping()
            : this(new AttributeStore())
        {}

        private DynamicComponentMapping(AttributeStore store)
            : base(store)
        {
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessDynamicComponent(this);

            base.AcceptVisitor(visitor);
        }
    }
}