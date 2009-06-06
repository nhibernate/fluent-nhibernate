using System;

namespace FluentNHibernate.MappingModel
{
    public class MetaValueMapping : MappingBase
    {
        private readonly AttributeStore<MetaValueMapping> attributes = new AttributeStore<MetaValueMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessMetaValue(this);
        }

        public string Value
        {
            get { return attributes.Get(x => x.Value); }
            set { attributes.Set(x => x.Value, value); }
        }

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public AttributeStore<MetaValueMapping> Attributes
        {
            get { return attributes; }
        }
    }
}