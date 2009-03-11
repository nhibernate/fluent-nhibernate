using System;

namespace FluentNHibernate.Conventions
{
    public interface IUserTypeConvention : IPropertyConvention
    {
        bool Accept(Type type);
    }
}