namespace FluentNHibernate.MappingModel.Identity
{
    public class IdGeneratorMapping : MappingBase
    {
        private readonly AttributeStore<IdGeneratorMapping> attributes;

        public IdGeneratorMapping()
        {
            attributes = new AttributeStore<IdGeneratorMapping>();
        }

        public static IdGeneratorMapping NativeGenerator
        {
            get { return new IdGeneratorMapping {ClassName = "native"}; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessIdGenerator(this);
        }

        public string ClassName
        {
            get { return attributes.Get(x => x.ClassName); }
            set { attributes.Set(x => x.ClassName, value); }
        }
    }
}