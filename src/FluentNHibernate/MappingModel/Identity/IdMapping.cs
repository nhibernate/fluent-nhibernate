using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdMapping : MappingBase, IIdentityMapping, INameable
    {
        private readonly AttributeStore<IdMapping> attributes = new AttributeStore<IdMapping>();
        private readonly IList<ColumnMapping> columns = new List<ColumnMapping>();

        public GeneratorMapping Generator { get; set; }

        public void AddColumn(ColumnMapping column)
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

            foreach (var column in Columns)
                visitor.Visit(column);

            if (Generator != null)
                visitor.Visit(Generator);
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

        public string Access
        {
            get { return attributes.Get(x => x.Access); }
            set { attributes.Set(x => x.Access, value); }
        }

        public string Type
        {
            get { return attributes.Get(x => x.Type); }
            set { attributes.Set(x => x.Type, value); }
        }

        public string UnsavedValue
        {
            get { return attributes.Get(x => x.UnsavedValue); }
            set { attributes.Set(x => x.UnsavedValue, value); }
        }

        public AttributeStore<IdMapping> Attributes
        {
            get { return attributes; }
        }
    }
}