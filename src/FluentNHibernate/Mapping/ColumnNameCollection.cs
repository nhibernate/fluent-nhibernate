using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public class ColumnNameCollection<TParent> : IColumnNameCollection
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

        void IColumnNameCollection.Add(string name)
        {
            Add(name);
        }

        public TParent Add(params string[] names)
        {
            foreach (var name in names)
            {
                Add(name);
            }
            return parent;
        }

        void IColumnNameCollection.Add(params string[] names)
        {
            Add(names);
        }

        public TParent Clear()
        {
            columnNames.Clear();
            return parent;
        }

        void IColumnNameCollection.Clear()
        {
            Clear();
        }

        public IList<string> List()
        {
            return new List<string>(columnNames).AsReadOnly();
        }
    }
}