using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IJoinedSubclass : IClasslike, IMappingPart
    {
        void WithTableName(string tableName);
        void SchemaIs(string schema);
        void CheckConstraint(string constraintName);
        JoinedSubclassMapping GetJoinedSubclassMapping();
    }
}