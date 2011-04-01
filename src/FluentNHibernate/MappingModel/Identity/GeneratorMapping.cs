using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity
{
    [Serializable]
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

        public override bool IsSpecified(string property)
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

        public bool Equals(GeneratorMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes) &&
                other.Params.ContentEquals(Params) &&
                Equals(other.ContainingEntityType, ContainingEntityType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(GeneratorMapping)) return false;
            return Equals((GeneratorMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (Params != null ? Params.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }
    }
}