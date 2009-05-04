using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel
{
    public class DiscriminatorMapping : MappingBase
    {
        private readonly AttributeStore<DiscriminatorMapping> _attributes;
        public ClassMapping ParentClass { get; internal set; }
        public ColumnMapping Column { get; set; }

        public DiscriminatorMapping()
        {
            _attributes = new AttributeStore<DiscriminatorMapping>();
            _attributes.SetDefault(x => x.IsNotNullable, true);
            _attributes.SetDefault(x => x.Insert, true);
            _attributes.SetDefault(x => x.Type, typeof(string));
            
        }

        public AttributeStore<DiscriminatorMapping> Attributes
        {
            get { return _attributes; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessDiscriminator(this);

            if(Column != null)
                visitor.Visit(Column);
        }
        

        public string ColumnName
        {
            get { return _attributes.Get(x => x.ColumnName); }
            set { _attributes.Set(x => x.ColumnName, value); }
        }

        public bool IsNotNullable
        {
            get { return _attributes.Get(x => x.IsNotNullable); }
            set { _attributes.Set(x => x.IsNotNullable, value); }
        }

        public int Length
        {
            get { return _attributes.Get(x => x.Length); }
            set { _attributes.Set(x => x.Length, value); }
        }

        public bool Force
        {
            get { return _attributes.Get(x => x.Force); }
            set { _attributes.Set(x => x.Force, value); }
        }

        public bool Insert
        {
            get { return _attributes.Get(x => x.Insert); }
            set { _attributes.Set(x => x.Insert, value); }
        }

        public string Formula
        {
            get { return _attributes.Get(x => x.Formula); }
            set { _attributes.Set(x => x.Formula, value); }
        }

        public Type Type
        {
            get { return _attributes.Get(x => x.Type); }
            set { _attributes.Set(x => x.Type, value); }
        }
    }
}
