using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        public Type ContainingEntityType { get; set; }

        public bool IsSpecified<TResult>(Expression<Func<GeneratorMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<GeneratorMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<GeneratorMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}