using System;
using FluentNHibernate.Mapping;
using NHibernate.UserTypes;

namespace FluentNHibernate.Conventions
{
    public abstract class UserTypeConvention<TType, TUserType> : IUserTypeConvention
        where TUserType : IUserType, new()
    {
        bool IConvention<IProperty>.Accept(IProperty target)
        {
            return Accept(target.PropertyType);
        }

        public virtual bool Accept(Type type)
        {
            return type == typeof(TType);
        }

        public virtual void Apply(IProperty target)
        {
            target.CustomTypeIs<TUserType>();
        }
    }
}