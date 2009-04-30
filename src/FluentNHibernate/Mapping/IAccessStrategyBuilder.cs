using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    public interface IAccessStrategyBuilder
    {
        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        void AsProperty();

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        void AsField();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        void AsCamelCaseField();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsCamelCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        void AsLowerCaseField();

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsLowerCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsPascalCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        void AsReadOnlyPropertyThroughCamelCaseField();

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        void AsReadOnlyPropertyThroughLowerCaseField();

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix);

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix);

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