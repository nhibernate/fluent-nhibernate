using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ColumnMapping : MappingBase
    {
        private readonly AttributeStore<ColumnMapping> attributes;

        public ColumnMapping()
            : this(new AttributeStore())
        {}

        public ColumnMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ColumnMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessColumn(this);
        }

        public PropertyInfo PropertyInfo { get; set; }

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

        public int Precision
        {
            get { return attributes.Get(x => x.Precision); }
            set { attributes.Set(x => x.Precision, value); }
        }

        public int Scale
        {
            get { return attributes.Get(x => x.Scale); }
            set { attributes.Set(x => x.Scale, value); }
        }

        public string Default
        {
            get { return attributes.Get(x => x.Default); }
            set { attributes.Set(x => x.Default, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<ColumnMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<ColumnMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<ColumnMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }

        public static ColumnMapping BaseOn(ColumnMapping originalMapping)
        {
            return new ColumnMapping(originalMapping.attributes.CloneInner());
        }
    }
}