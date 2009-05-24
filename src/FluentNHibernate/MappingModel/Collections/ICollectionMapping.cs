using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionMapping : IMappingBase, INameable
    {
        bool Inverse { get; }
        bool Lazy { get; }
        string Access { get; }
        string TableName { get; }
        string Schema { get; }
        string OuterJoin { get; }
        string Fetch { get; }
        string Cascade { get; }
        string Where { get; }
        string Persister { get; }
        int BatchSize { get; }
        string Check { get; }
        string CollectionType { get; }
        string OptimisticLock { get; }
        bool Generic { get; }
        KeyMapping Key { get; set; }
        ICollectionRelationshipMapping Relationship { get; set; }
        AttributeStore<ICollectionMapping> Attributes { get; }
        PropertyInfo PropertyInfo { get; set;  }
    }
}