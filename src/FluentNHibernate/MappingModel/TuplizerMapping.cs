using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel
{
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


    }

    public enum TuplizerMode
    {
        Poco,
        Xml,
        DynamicMap
    }
}