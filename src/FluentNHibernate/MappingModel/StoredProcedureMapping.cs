using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class StoredProcedureMapping : MappingBase
    {
        private readonly AttributeStore<StoredProcedureMapping> attributes;

        public StoredProcedureMapping() : this("sql-insert", "")
        {
        }

        public StoredProcedureMapping(AttributeStore attributes)
        {
            this.attributes = new AttributeStore<StoredProcedureMapping>(attributes);
        }

        public StoredProcedureMapping(string spType, string innerText): this(spType, innerText, new AttributeStore())
        {
        }

        public StoredProcedureMapping(string spType, string innerText, AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<StoredProcedureMapping>(underlyingStore);
            SPType = spType;
            Query = innerText;
            Check = "none";

        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public Type Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessStoredProcedure(this);
        }

        public void MergeAttributes(AttributeStore store)
        {
            attributes.Merge(new AttributeStore<StoredProcedureMapping>(store));
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }

        public string SPType
        {
            get { return attributes.Get(x => x.SPType); }
            set { attributes.Set(x => x.SPType, value); }
        }     
        
        public string Query
        {
            get { return attributes.Get(x => x.Query); }
            set { attributes.Set(x => x.Query, value); }
        }

        public void SetDefaultValue<TResult>(Expression<Func<StoredProcedureMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
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
    }
}
