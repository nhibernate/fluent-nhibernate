using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.UserTypes;

namespace FluentNHibernate.Conventions;

/// <summary>
/// Base class for user type conventions. Create a subclass of this to automatically
/// map all properties that the user type can be used against. Override Accept or
/// Apply to alter the behavior.
/// </summary>
/// <typeparam name="TUserType">IUserType implementation</typeparam>
public abstract class UserTypeConvention<TUserType> : IUserTypeConvention
    where TUserType : IUserType, new()
{
    public virtual void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
    {
        var userType = Activator.CreateInstance<TUserType>();
        var returnedType = userType.ReturnedType;
        if (returnedType.IsValueType)
        {
            var nullableReturnedType = typeof(Nullable<>).MakeGenericType(returnedType);
            criteria.Expect(x => x.Type == returnedType || x.Type == nullableReturnedType);
        }
        else
            criteria.Expect(x => x.Type == returnedType);
    }

    public virtual void Apply(IPropertyInstance instance)
    {
        instance.CustomType<TUserType>();
    }
}
