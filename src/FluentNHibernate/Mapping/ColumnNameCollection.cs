using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public class ColumnNameCollection<TParent>
    {
        private readonly IList<string> columnNames = new List<string>();
        private readonly TParent parent;

        public ColumnNameCollection(TParent parent)
        {
            this.parent = parent;
        }

        public TParent Add(string name)
        {
            columnNames.Add(name);
            return parent;
        }

        public TParent Add(params string[] names)
        {
            foreach (var name in names)
            {
                Add(name);
            }
            return parent;
        }

        public TParent Clear()
        {
            columnNames.Clear();
            return parent;
        }

        public IList<string> List()
        {
            return new List<string>(columnNames).AsReadOnly();
        }
    }
}