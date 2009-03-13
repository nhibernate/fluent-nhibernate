using System.Collections.Generic;

namespace FluentNHibernate.Mapping
{
    public interface IColumnNameCollection
    {
        void Add(string name);
        void Add(params string[] names);
        void Clear();
        IList<string> List();
    }
}