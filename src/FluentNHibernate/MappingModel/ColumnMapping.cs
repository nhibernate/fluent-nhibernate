using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ColumnMapping : MappingBase
    {
        private readonly AttributeStore<ColumnMapping> attributes;

        public ColumnMapping()
            : this(new AttributeStore<ColumnMapping>())
        {}

        public ColumnMapping(AttributeStore<ColumnMapping> attributes)
        {
            this.attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessColumn(this);
        }

        public PropertyInfo PropertyInfo { get; set; }

        public AttributeStore<ColumnMapping> Attributes
        {
            get { return attributes; }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public int Length
        {
            get { return attributes.Get(x => x.Length); }
            set { attributes.Set(x => x.Length, value); }
        }

        public bool NotNull
        {
            get { return attributes.Get(x => x.NotNull); }
            set { attributes.Set(x => x.NotNull, value); }
        }

        public bool Unique
        {
            get { return attributes.Get(x => x.Unique); }
            set { attributes.Set(x => x.Unique, value); }
        }

        public string UniqueKey
        {
            get { return attributes.Get(x => x.UniqueKey); }
            set { attributes.Set(x => x.UniqueKey, value); }
        }

        public string SqlType
        {
            get { return attributes.Get(x => x.SqlType); }
            set { attributes.Set(x => x.SqlType, value); }
        }

        public string Index
        {
            get { return attributes.Get(x => x.Index); }
            set { attributes.Set(x => x.Index, value); }
        }

        public string Check
        {
            get { return attributes.Get(x => x.Check); }
            set { attributes.Set(x => x.Check, value); }
        }
    }
}