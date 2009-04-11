using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ColumnMapping : MappingBase, INameable
    {
        private readonly AttributeStore<ColumnMapping> _attributes;

        public ColumnMapping()
        {
            _attributes = new AttributeStore<ColumnMapping>();
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

        public bool IsNotNullable
        {
            get { return _attributes.Get(x => x.IsNotNullable); }
            set { _attributes.Set(x => x.IsNotNullable, value); }
        }

        public bool IsUnique
        {
            get { return _attributes.Get(x => x.IsUnique); }
            set { _attributes.Set(x => x.IsUnique, value); }
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