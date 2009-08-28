using System;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdMapping : ColumnBasedMappingBase, IIdentityMapping
    {
        public IdMapping()
            : this(new AttributeStore())
        {}

        public IdMapping(AttributeStore underlyingStore)
            : base(underlyingStore)
        {}

        public PropertyInfo PropertyInfo { get; set; }

        public GeneratorMapping Generator
        {
            get { return attributes.Get<GeneratorMapping>("Generator"); }
            set { attributes.Set("Generator", value); }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessId(this);

            foreach (var column in Columns)
                visitor.Visit(column);

            if (Generator != null)
                visitor.Visit(Generator);
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

        public Type ContainingEntityType { get; set; }
    }
}