using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Identity
{
    public class GeneratorMapping : MappingBase
    {
        private readonly AttributeStore<GeneratorMapping> attributes = new AttributeStore<GeneratorMapping>();

        public GeneratorMapping()
        {
            Params = new Dictionary<string, string>();
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

        public IDictionary<string, string> Params { get; private set; }

        public AttributeStore<GeneratorMapping> Attributes
        {
            get { return attributes; }
        }
    }
}