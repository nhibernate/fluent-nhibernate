using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ColumnMapping : MappingBase, INameable
    {
        private readonly AttributeStore<ColumnMapping> _attributes;

        public ColumnMapping()
            : this(new AttributeStore<ColumnMapping>())
        {}

        public ColumnMapping(AttributeStore<ColumnMapping> attributes)
        {
            _attributes = attributes;
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessColumn(this);
        }

        public PropertyInfo PropertyInfo { get; set; }

        public AttributeStore<ColumnMapping> Attributes
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

        public bool NotNull
        {
            get { return _attributes.Get(x => x.NotNull); }
            set { _attributes.Set(x => x.NotNull, value); }
        }

        public bool Unique
        {
            get { return _attributes.Get(x => x.Unique); }
            set { _attributes.Set(x => x.Unique, value); }
        }

        public string UniqueKey
        {
            get { return _attributes.Get(x => x.UniqueKey); }
            set { _attributes.Set(x => x.UniqueKey, value); }
        }

        public string SqlType
        {
            get { return _attributes.Get(x => x.SqlType); }
            set { _attributes.Set(x => x.SqlType, value); }
        }

        public string Index
        {
            get { return _attributes.Get(x => x.Index); }
            set { _attributes.Set(x => x.Index, value); }
        }

        public string Check
        {
            get { return _attributes.Get(x => x.Check); }
            set { _attributes.Set(x => x.Check, value); }
        }

        
    }
}