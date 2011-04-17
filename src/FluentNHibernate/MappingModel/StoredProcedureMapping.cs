using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class StoredProcedureMapping : MappingBase
    {
        private readonly AttributeStore attributes;

        public StoredProcedureMapping() : this("sql-insert", "")
        {
        }

        public StoredProcedureMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public StoredProcedureMapping(string spType, string innerText): this(spType, innerText, new AttributeStore())
        {
        }

        public StoredProcedureMapping(string spType, string innerText, AttributeStore attributes)
        {
            this.attributes = attributes;

            Set(x => x.SPType, Layer.Defaults, spType);
            Set(x => x.Query, Layer.Defaults, innerText);
            Set(x => x.Check, Layer.Defaults, "none");
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public Type Type
        {
            get { return attributes.GetOrDefault<Type>("Type"); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessStoredProcedure(this);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }

        public string Check
        {
            get { return attributes.GetOrDefault<string>("Check"); }
        }

        public string SPType
        {
            get { return attributes.GetOrDefault<string>("SPType"); }
        }

        public string Query
        {
            get { return attributes.GetOrDefault<string>("Query"); }
        }

        public bool Equals(StoredProcedureMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StoredProcedureMapping);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                {
                    return (base.GetHashCode() * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                }
            }
        }

        public void Set<T>(Expression<Func<StoredProcedureMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }
    }
}