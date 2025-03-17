using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class NestedCompositeElementMapping(AttributeStore attributes) : CompositeElementMapping(attributes)
{
    readonly AttributeStore attributes = attributes;

    public NestedCompositeElementMapping()
        : this(new AttributeStore())
    { }

    public string Name => attributes.GetOrDefault<string>();

    public string Access => attributes.GetOrDefault<string>();

    public void Set<T>(Expression<Func<NestedCompositeElementMapping, T>> expression, int layer, T value)
    {
        Set(expression.ToMember().Name, layer, value);
    }
}
