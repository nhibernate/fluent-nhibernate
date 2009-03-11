using System;
using System.Reflection;
using NHibernate.UserTypes;

namespace FluentNHibernate.Mapping.Conventions
{
    public abstract class BaseUserTypeConvention<TType, TUserType> : IUserTypeConvention
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

        public virtual void Apply(IProperty target, ConventionOverrides overrides)
        {
            target.CustomTypeIs<TUserType>();
        }
    }
}