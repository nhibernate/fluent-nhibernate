using System;

namespace FluentNHibernate.MappingModel.Identity
{
    public class GeneratorMapping : MappingBase
    {
        private readonly AttributeStore<GeneratorMapping> attributes;

        public GeneratorMapping()
        {
            attributes = new AttributeStore<GeneratorMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessGenerator(this);
        }

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public AttributeStore<GeneratorMapping> Attributes
        {
            get { return attributes; }
        }
    }
}