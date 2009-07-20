using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface IJoinedSubclass : IClasslike, IMappingPart
    {
        void KeyColumnName(string columnName);
        void WithTableName(string tableName);
        void SchemaIs(string schema);
        void CheckConstraint(string constraintName);
        void Proxy(Type type);
        void Proxy<T>();
        void LazyLoad();
        void DynamicUpdate();
        void DynamicInsert();
        void SelectBeforeUpdate();
        void Abstract();
        IJoinedSubclass Not { get; }

        JoinedSubclassMapping GetJoinedSubclassMapping();
    }
}