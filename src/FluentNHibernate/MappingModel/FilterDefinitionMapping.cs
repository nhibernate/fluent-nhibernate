using System;
using System.Collections.Generic;
using FluentNHibernate.Visitors;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
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

        public bool Equals(FilterDefinitionMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) &&
                other.parameters.ContentEquals(parameters);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(FilterDefinitionMapping)) return false;
            return Equals((FilterDefinitionMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((attributes != null ? attributes.GetHashCode() : 0) * 397) ^ (parameters != null ? parameters.GetHashCode() : 0);
            }
        }
    }
}
