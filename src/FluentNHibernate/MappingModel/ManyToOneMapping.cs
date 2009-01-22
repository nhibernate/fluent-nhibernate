using System;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel
{
    public class ManyToOneMapping : MappingBase
    {
        private readonly AttributeStore<ManyToOneMapping> _attributes;

        public ManyToOneMapping()
        {
            _attributes = new AttributeStore<ManyToOneMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToOne(this);
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        public bool IsNotNullable
        {
            get { return _attributes.Get(x => x.IsNotNullable); }
            set { _attributes.Set(x => x.IsNotNullable, value); }
        }

        public AttributeStore<ManyToOneMapping> Attributes
        {
            get { return _attributes; }
        }
    }
}