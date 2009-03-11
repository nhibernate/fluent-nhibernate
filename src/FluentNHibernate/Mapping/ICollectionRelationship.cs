using System.Reflection;

namespace FluentNHibernate.Mapping
{
    public interface ICollectionRelationship : IRelationship
    {
        bool IsMethodAccess { get; }
        MemberInfo Member { get; }
        string TableName { get; }
    }
}