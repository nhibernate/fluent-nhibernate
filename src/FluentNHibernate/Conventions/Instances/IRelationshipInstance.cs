using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IRelationshipInstance : IRelationshipInspector
    {
        //IDefaultableEnumerable<IColumnInstance> Columns { get; }
        void CustomClass<T>();
        void CustomClass(Type type);
    }
}
