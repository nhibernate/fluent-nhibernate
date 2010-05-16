using System;
using System.Linq.Expressions;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class TuplizerMapping : MappingBase
    {
        private readonly AttributeStore<TuplizerMapping> attributes;

        public TuplizerMapping()
            : this(new AttributeStore())
        {}

        public TuplizerMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<TuplizerMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessTuplizer(this);
        }

        public TuplizerMode Mode
        {
            get { return attributes.Get(x => x.Mode); }
            set { attributes.Set(x => x.Mode, value); }
        }

        public string EntityName
        {
            get { return attributes.Get(x => x.EntityName); }
            set { attributes.Set(x => x.EntityName, value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public override bool IsSpecified(string property)
        {
            return attributes.IsSpecified(property);            
        }

        public bool HasValue<TResult>(Expression<Func<TuplizerMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public bool Equals(TuplizerMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(TuplizerMapping)) return false;
            return Equals((TuplizerMapping)obj);
        }

        public override int GetHashCode()
        {
            return (attributes != null ? attributes.GetHashCode() : 0);
        }
    }

    public enum TuplizerMode
    {
        Poco,
        Xml,
        DynamicMap
    }
}