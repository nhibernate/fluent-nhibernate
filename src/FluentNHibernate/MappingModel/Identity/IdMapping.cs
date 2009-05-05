using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdMapping : MappingBase, IIdentityMapping, INameable
    {
        private readonly AttributeStore<IdMapping> attributes;
        private readonly IList<ColumnMapping> columns;

        public IdMapping()
        {
            attributes = new AttributeStore<IdMapping>();
            columns = new List<ColumnMapping>();
        }

        public IdMapping(ColumnMapping columnMapping) : this()
        {
            AddIdColumn(columnMapping);
        }

        public IdGeneratorMapping Generator { get; set; }

        public void AddIdColumn(ColumnMapping column)
        {
            columns.Add(column);
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return columns; }
        }

        public PropertyInfo PropertyInfo { get; set; }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessId(this);

            if (Generator != null)
                visitor.Visit(Generator);

            foreach (var column in Columns)
                visitor.Visit(column);
        }

        public bool IsNameSpecified
        {
            get { return Attributes.IsSpecified(x => x.Name); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public AttributeStore<IdMapping> Attributes
        {
            get { return attributes; }
        }
        
    }
}