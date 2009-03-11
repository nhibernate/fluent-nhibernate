using System;

namespace FluentNHibernate.Mapping.Conventions
{
    public interface IUserTypeConvention : IPropertyConvention
    {
        bool Accept(Type type);
    }
}