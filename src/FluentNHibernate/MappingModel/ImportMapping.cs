using System;

namespace FluentNHibernate.MappingModel
{
    public class ImportMapping : MappingBase
    {
        private readonly AttributeStore<ImportMapping> attributes = new AttributeStore<ImportMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessImport(this);
        }

        public AttributeStore<ImportMapping> Attributes
        {
            get { return attributes; }
        }

        public string Rename
        {
            get { return attributes.Get(x => x.Rename); }
            set { attributes.Set(x => x.Rename, value); }
        }

        public TypeReference Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }
    }
}