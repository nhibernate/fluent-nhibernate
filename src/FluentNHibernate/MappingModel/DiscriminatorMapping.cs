using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel
{
    public class DiscriminatorMapping : ColumnBasedMappingBase
    {
        public DiscriminatorMapping()
            : this(new AttributeStore())
        {}

        public DiscriminatorMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessDiscriminator(this);

            columns.Each(visitor.Visit);
        }

        public bool Force
        {
            get { return attributes.Get<bool>("Force"); }
            set { attributes.Set("Force", value); }
        }

        public bool Insert
        {
            get { return attributes.Get<bool>("Insert"); }
            set { attributes.Set("Insert", value); }
        }

        public string Formula
        {
            get { return attributes.Get("Formula"); }
            set { attributes.Set("Formula", value); }
        }

        public TypeReference Type
        {
            get { return attributes.Get<TypeReference>("Type"); }
            set { attributes.Set("Type", value); }
        }

        public Type ContainingEntityType { get; set; }
    }
}
