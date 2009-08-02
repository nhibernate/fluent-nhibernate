using System;
using NHibernate.Properties;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Access strategy mapping builder.
    /// </summary>
    public class AccessStrategyBuilder
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
        public void Property()
        {
            setValue("property");
        }

        /// <summary>
        /// Sets the access-strategy to field.
        /// </summary>
        public void Field()
        {
            setValue("field");
        }

        /// <summary>
        /// Sets the access-strategy to use the backing-field of an auto-property.
        /// </summary>
        public void BackingField()
        {
            setValue("backfield");
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase (field.camelcase).
        /// </summary>
        public void CamelCaseField()
        {
            ((AccessStrategyBuilder)this).CamelCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void CamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setValue("field.camelcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase.
        /// </summary>
        public void LowerCaseField()
        {
            ((AccessStrategyBuilder)this).LowerCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to lowercase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void LowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setValue("field.lowercase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to field and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void PascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setValue("field.pascalcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase.
        /// </summary>
        public void ReadOnlyPropertyThroughCamelCaseField()
        {
            ((AccessStrategyBuilder)this).ReadOnlyPropertyThroughCamelCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to camelcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setValue("nosetter.camelcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        public void ReadOnlyPropertyThroughLowerCaseField()
        {
            ((AccessStrategyBuilder)this).ReadOnlyPropertyThroughLowerCaseField(Prefix.None);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to lowercase.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setValue("nosetter.lowercase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to read-only property (nosetter) and the naming-strategy to pascalcase, with the specified prefix.
        /// </summary>
        /// <param name="prefix">Naming-strategy prefix</param>
        public void ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setValue("nosetter.pascalcase" + prefix.Value);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorAssemblyQualifiedClassName">Assembly qualified name of the type to use as the access-strategy</param>
        public void Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            setValue(propertyAccessorAssemblyQualifiedClassName);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <param name="propertyAccessorClassType">Type to use as the access-strategy</param>
        public void Using(Type propertyAccessorClassType)
        {
            ((AccessStrategyBuilder)this).Using(propertyAccessorClassType.AssemblyQualifiedName);
        }

        /// <summary>
        /// Sets the access-strategy to use the type referenced.
        /// </summary>
        /// <typeparam name="TPropertyAccessorClass">Type to use as the access-strategy</typeparam>
        public void Using<TPropertyAccessorClass>()
        {
            ((AccessStrategyBuilder)this).Using(typeof(TPropertyAccessorClass));
        }

        public void NoOp()
        {
            setValue("noop");
        }

        public void None()
        {
            setValue("none");
        }
    }
}