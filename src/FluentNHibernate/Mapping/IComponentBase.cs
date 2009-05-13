using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public interface IComponentBase : IClasslike, IMappingPart
    {
        IComponentBase WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp);
        ComponentMappingBase GetComponentMapping();

        IComponentBase Not { get; }
        IComponentBase ReadOnly();
        IComponentBase Insert();
        IComponentBase Update();
    }
}
