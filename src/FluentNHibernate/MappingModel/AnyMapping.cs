using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class AnyMapping : MappingBase, INameable
    {
        private readonly AttributeStore<AnyMapping> attributes = new AttributeStore<AnyMapping>();
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessAny(this);

            foreach (var column in columns)
                visitor.Visit(column);
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

        public string IdType
        {
            get { return attributes.Get(x => x.IdType); }
            set { attributes.Set(x => x.IdType, value); }
        }

        public string MetaType
        {
            get { return attributes.Get(x => x.MetaType); }
            set { attributes.Set(x => x.MetaType, value); }
        }

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public bool Insert
        {
            get { return attributes.Get(x => x.Insert); }
            set { attributes.Set(x => x.Insert, value); }
        }

        public bool Update
        {
            get { return attributes.Get(x => x.Update); }
            set { attributes.Set(x => x.Update, value); }
        }

        public string Cascade
        {
            get { return attributes.Get(x => x.Cascade); }
            set { attributes.Set(x => x.Cascade, value); }
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public AttributeStore<AnyMapping> Attributes
        {
            get { return attributes; }
        }

        public void AddColumn(ColumnMapping column)
        {
            columns.Add(column);
        }
    }
}