using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel
{
    public class VersionMapping : ColumnBasedMappingBase
    {
        public VersionMapping()
            : this(new AttributeStore())
        {}

        public VersionMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessVersion(this);

            columns.Each(visitor.Visit);
        }

        public string Name
        {
            get { return attributes.Get("Name"); }
            set { attributes.Set("Name", value); }
        }

        public string Access
        {
            get { return attributes.Get("Access"); }
            set { attributes.Set("Access", value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get<TypeReference>("Type"); }
            set { attributes.Set("Type", value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get("UnsavedValue"); }
            set { attributes.Set("UnsavedValue", value); }
        }

        public string Generated
        {
            get { return attributes.Get("Generated"); }
            set { attributes.Set("Generated", value); }
        }

        public Type ContainingEntityType { get; set; }
    }
}