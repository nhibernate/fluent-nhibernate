using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdGeneratorMapping : MappingBase
    {
        private readonly AttributeStore<IdGeneratorMapping> _attributes;

        public IdGeneratorMapping()
        {
            _attributes = new AttributeStore<IdGeneratorMapping>();
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
            get { return _attributes.Get(x => x.ClassName); }
            set { _attributes.Set(x => x.ClassName, value); }
        }
    }
}