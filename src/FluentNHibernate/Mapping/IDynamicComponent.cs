using System;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IDynamicComponent : IClasslike, IMappingPart
    {
        IDynamicComponent WithParentReference<TEntity>(Expression<Func<TEntity, object>> exp);
        DynamicComponentMapping GetDynamicComponentMapping();
    }
}