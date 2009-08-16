using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public interface IDefaultableList<T> : IList<T>, IDefaultableEnumerable<T>
    {
        void AddDefault(T item);
        void ClearDefaults();
        void ClearAll();
        bool ContainsDefault(T item);
        void CopyDefaultsTo(T[] array, int arrayIndex);
        void CopyAllTo(T[] array, int arrayIndex);
        bool RemoveDefault(T item1);
        int CountDefaults { get; }
        int CountAll { get; }
        int IndexOfDefault(T item);
        void InsertDefault(int index, T item);
        void RemoveDefaultAt(int index);
    }
}