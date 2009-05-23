using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Access strategy mapping builder.
    /// </summary>
    public class AccessStrategyBuilder : IAccessStrategyBuilder
    {
        private const string InvalidPrefixCamelCaseFieldM = "m is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixCamelCaseFieldMUnderscore = "m_ is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixLowerCaseFieldM = "m is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixLowerCaseFieldMUnderscore = "m_ is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixPascalCaseFieldNone = "None is not a valid prefix for a PascalCase Field.";
        
        internal Action<string> setValue;

        /// <summary>
        /// Access strategy mapping builder.
        /// </summary>
        protected AccessStrategyBuilder(Action<string> setter)
        {
            this.setValue = setter;
        }

        /// <summary>
        /// Sets the access-strategy to property.
        /// </summary>
        void IAccessStrategyBuilder.AsProperty()
        {
            setValue("property");
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        void IAccessStrategyBuilder.AsField()
        {
            setValue("field");
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        void IAccessStrategyBuilder.AsCamelCaseField()
        {
            ((IAccessStrategyBuilder)this).AsCamelCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setValue("field.camelcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        void IAccessStrategyBuilder.AsLowerCaseField()
        {
            ((IAccessStrategyBuilder)this).AsLowerCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setValue("field.lowercase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setValue("field.pascalcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughCamelCaseField()
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughCamelCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setValue("nosetter.camelcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughLowerCaseField()
        {
            ((IAccessStrategyBuilder)this).AsReadOnlyPropertyThroughLowerCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setValue("nosetter.lowercase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        void IAccessStrategyBuilder.AsReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setValue("nosetter.pascalcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        void IAccessStrategyBuilder.Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            setValue(propertyAccessorAssemblyQualifiedClassName);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        void IAccessStrategyBuilder.Using(Type propertyAccessorClassType)
        {
            ((IAccessStrategyBuilder)this).Using(propertyAccessorClassType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        void IAccessStrategyBuilder.Using<TPropertyAccessorClass>()
        {
            ((IAccessStrategyBuilder)this).Using(typeof(TPropertyAccessorClass));
        }
    }
}