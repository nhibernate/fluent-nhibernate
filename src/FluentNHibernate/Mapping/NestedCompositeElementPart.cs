using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping;

public class NestedCompositeElementPart<T> : CompositeElementPart<T>
{
    public NestedCompositeElementPart(Type entity, Member member)
        : base(entity, member)
    {
        Access = new AccessStrategyBuilder<NestedCompositeElementPart<T>>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            
        SetDefaultAccess();
    }

    public AccessStrategyBuilder<NestedCompositeElementPart<T>> Access { get; }
        
    void SetDefaultAccess()
    {
        var resolvedAccess = MemberAccessResolver.Resolve(member);

        if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
            return; // property is the default so we don't need to specify it

        attributes.Set("Access", Layer.Defaults, resolvedAccess.ToString());
    }
}
