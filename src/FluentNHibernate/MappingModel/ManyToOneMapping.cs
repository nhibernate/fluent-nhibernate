using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.MappingModel
{
    public class ManyToOneMapping : MappingBase
    {
        private readonly AttributeStore<ManyToOneMapping> attributes = new AttributeStore<ManyToOneMapping>();
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToOne(this);

            foreach (var column in columns)
                visitor.Visit(column);
        }

        public PropertyInfo PropertyInfo { get; set; }

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

        public string Class
        {
            get { return attributes.Get(x => x.Class); }
            set { attributes.Set(x => x.Class, value); }
        }

        public string Cascade
        {
            get { return attributes.Get(x => x.Cascade); }
            set { attributes.Set(x => x.Cascade, value); }
        }

        public string OuterJoin
        {
            get { return attributes.Get(x => x.OuterJoin); }
            set { attributes.Set(x => x.OuterJoin, value); }
        }

        public string Fetch
        {
            get { return attributes.Get(x => x.Fetch); }
            set { attributes.Set(x => x.Fetch, value); }
        }

        public bool Update
        {
            get { return attributes.Get(x => x.Update); }
            set { attributes.Set(x => x.Update, value); }
        }

        public bool Insert
        {
            get { return attributes.Get(x => x.Insert); }
            set { attributes.Set(x => x.Insert, value); }
        }

        public string ForeignKey
        {
            get { return attributes.Get(x => x.ForeignKey); }
            set { attributes.Set(x => x.ForeignKey, value); }
        }

        public string PropertyRef
        {
            get { return attributes.Get(x => x.PropertyRef); }
            set { attributes.Set(x => x.PropertyRef, value); }
        }

        public string NotFound
        {
            get { return attributes.Get(x => x.NotFound); }
            set { attributes.Set(x => x.NotFound, value); }
        }

        public bool Lazy
        {
            get { return attributes.Get(x => x.Lazy); }
            set { attributes.Set(x => x.Lazy, value); }
        }

        public AttributeStore<ManyToOneMapping> Attributes
        {
            get { return attributes; }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public void AddColumn(ColumnMapping column)
        {
            columns.Add(column);
        }
    }
}