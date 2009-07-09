using System.Reflection;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface ICollectionInstance : ICollectionInspector, IAccessInstance
    {
        new IKeyInstance Key { get; }
        new IRelationshipInstance Relationship { get; }
        void SetTableName(string tableName);
        void Name(string name);
        void SchemaIs(string schema);
        void LazyLoad();

        ICollectionInstance Not { get; }
    }
}