using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class PropertyMapping : MappingBase
    {
        private readonly AttributeStore<PropertyMapping> _attributes;        

        public PropertyMapping()
        {
            _attributes = new AttributeStore<PropertyMapping>();   
        }
       
        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessProperty(this);
        }

        public AttributeStore<PropertyMapping> Attributes
        {
            get { return _attributes; }
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        public int Length
        {
            get { return _attributes.Get(x => x.Length); }
            set { _attributes.Set(x => x.Length, value); }
        }

        public bool AllowNull
        {
            get { return _attributes.Get(x => x.AllowNull); }
            set { _attributes.Set(x => x.AllowNull, value); }
        }
    }
}