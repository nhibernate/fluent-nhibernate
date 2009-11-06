using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class StoredProcedureMapping : ClassMappingBase
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

        public override string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public override Type Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessStoredProcedure(this);
        }

        public override void MergeAttributes(AttributeStore store)
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
    }



 
}
