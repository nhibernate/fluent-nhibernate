using System;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IJoinedSubclassInstance : IJoinedSubclassInspector
    {
        IJoinedSubclassInstance Not { get; }
        void Abstract();
        void CheckConstraint(string constraint);
        void DynamicInsert();
        void DynamicUpdate();
        void LazyLoad();
        void Proxy(Type type);
        void Proxy<T>();
        void Schema(string schema);
        void SelectBeforeUpdate();
        void TableName(string tableName);
    }
 }