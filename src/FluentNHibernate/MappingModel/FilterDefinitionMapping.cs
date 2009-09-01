using System;
using System.Collections.Generic;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel
{
    public class FilterDefinitionMapping : MappingBase
    {
        private readonly AttributeStore<FilterMapping> attributes;
        private readonly IDictionary<string, IType> parameters;

        public FilterDefinitionMapping()
            : this(new AttributeStore())
        { }

        public FilterDefinitionMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<FilterMapping>(underlyingStore);
            parameters = new Dictionary<string, IType>();
        }

        public IDictionary<string, IType> Parameters
        {
            get { return parameters; }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Condition
        {
            get { return attributes.Get(x => x.Condition); }
            set { attributes.Set(x => x.Condition, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessFilterDefinition(this);
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }
    }
}
