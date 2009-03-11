using System;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// User type convention, implement this interface to apply changes to
    /// property's that use a IUserType. Subclassing <see cref="UserTypeConvention{TUserType}"/>
    /// is recommended instead of directly implementing this interface.
    /// </summary>
    public interface IUserTypeConvention : IPropertyConvention
    {
        /// <summary>
        /// Should apply changes to property's of this type?
        /// </summary>
        /// <param name="type">Property type</param>
        /// <returns>Accept or not</returns>
        bool Accept(Type type);
    }
}