using System;

namespace FluentNHibernate.MappingModel
{
    public class VersionMapping : MappingBase, INameable
    {
        private readonly AttributeStore<VersionMapping> attributes = new AttributeStore<VersionMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessVersion(this);
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public string Column
        {
            get { return attributes.Get(x => x.Column); }
            set { attributes.Set(x => x.Column, value); }
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get(x => x.UnsavedValue); }
            set { attributes.Set(x => x.UnsavedValue, value); }
        }

        public string Generated
        {
            get { return attributes.Get(x => x.Generated); }
            set { attributes.Set(x => x.Generated, value); }
        }

        public AttributeStore<VersionMapping> Attributes
        {
            get { return attributes; }
        }
    }
}