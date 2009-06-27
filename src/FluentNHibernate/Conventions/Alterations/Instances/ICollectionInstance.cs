using System.Reflection;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Alterations.Instances
{
    public interface ICollectionInstance : IManyToManyCollectionInstance, IOneToManyCollectionInstance, ICollectionInspector, ICollectionAlteration
    {
        new IKeyInstance Key { get; }
        new IOneToManyInstance OneToMany { get; }
        new IManyToManyInstance ManyToMany { get; }
        new void Name(string name);
        new void SetTableName(string tableName);
        new bool IsMethodAccess { get; }
        new MemberInfo Member { get; }
        new string TableName { get; }
    }
}