using System;
using NHibernate.Cfg.MappingSchema;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class PropertyMapping : MappingBase, INameable
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

        public PropertyInfo PropertyInfo { get; set; }

        public AttributeStore<PropertyMapping> Attributes
        {
            get { return _attributes; }
        }

        public bool IsNameSpecified
        {
            get { return Attributes.IsSpecified(x => x.Name); }
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

        public bool IsNotNullable
        {
            get { return _attributes.Get(x => x.IsNotNullable); }
            set { _attributes.Set(x => x.IsNotNullable, value); }
        }

        public string ColumnName
        {
            get { return _attributes.Get(x => x.ColumnName); }
            set { _attributes.Set(x => x.ColumnName, value); }
        }
    }
}