using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class FilterDefinitionMapping : MappingBase
    {
        readonly AttributeStore attributes;
        readonly IDictionary<string, IType> parameters;

        public FilterDefinitionMapping()
            : this(new AttributeStore())
        { }

        public FilterDefinitionMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
            parameters = new Dictionary<string, IType>();
        }

        public IDictionary<string, IType> Parameters
        {
            get { return parameters; }
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string Condition
        {
            get { return attributes.GetOrDefault<string>("Condition"); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessFilterDefinition(this);
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

        public void Set<T>(Expression<Func<FilterDefinitionMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}
