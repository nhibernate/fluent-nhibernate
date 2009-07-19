using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    public interface IAccessStrategyBuilder
    {
        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        void Property();

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        void Field();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        void CamelCaseField();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void CamelCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        void LowerCaseField();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void LowerCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void PascalCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        void ReadOnlyPropertyThroughCamelCaseField();

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void ReadOnlyPropertyThroughCamelCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        void ReadOnlyPropertyThroughLowerCaseField();

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void ReadOnlyPropertyThroughLowerCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void ReadOnlyPropertyThroughPascalCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        void Using(string propertyAccessorAssemblyQualifiedClassName);

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        void Using(Type propertyAccessorClassType);

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor;
    }
}