using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.UserTypes;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Base class for user type conventions. Create a subclass of this to automatically
    /// map all properties that the user type can be used against. Override Accept or
    /// Apply to alter the behavior.
    /// </summary>
    /// <typeparam name="TUserType">IUserType implementation</typeparam>
    public abstract class UserTypeConvention<TUserType> : IPropertyConvention
        where TUserType : IUserType, new()
    {
        public virtual void Accept(IAcceptanceCriteria<IPropertyInspector> acceptance)
        {
            var userType = Activator.CreateInstance<TUserType>();

            acceptance.Expect(x => x.PropertyType == userType.ReturnedType);
        }

        public virtual void Apply(IPropertyAlteration alteration, IPropertyInspector inspector)
        {
            alteration.CustomTypeIs<TUserType>();
        }
    }
}