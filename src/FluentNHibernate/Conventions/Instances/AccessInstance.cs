using System;
using FluentNHibernate.Mapping;
using NHibernate.Properties;

namespace FluentNHibernate.Conventions.Instances
{
    public class AccessInstance : IAccessInstance
    {
        private const string InvalidPrefixCamelCaseFieldM = "m is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixCamelCaseFieldMUnderscore = "m_ is not a valid prefix for a CamelCase Field.";
        private const string InvalidPrefixLowerCaseFieldM = "m is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixLowerCaseFieldMUnderscore = "m_ is not a valid prefix for a LowerCase Field.";
        private const string InvalidPrefixPascalCaseFieldNone = "None is not a valid prefix for a PascalCase Field.";

        private readonly Action<string> setter;

        public AccessInstance(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Property()
        {
            setter("property");
        }

        public void Field()
        {
            setter("field");
        }

        public void CamelCaseField()
        {
            CamelCaseField(Prefix.None);
        }

        public void CamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setter("field.camelcase" + prefix.Value);
        }

        public void LowerCaseField()
        {
            LowerCaseField(Prefix.None);
        }

        public void LowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setter("field.lowercase" + prefix.Value);
        }

        public void PascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setter("field.pascalcase" + prefix.Value);
        }

        public void ReadOnlyPropertyThroughCamelCaseField()
        {
            ReadOnlyPropertyThroughCamelCaseField(Prefix.None);
        }

        public void ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixCamelCaseFieldMUnderscore);

            setter("nosetter.camelcase" + prefix.Value);
        }

        public void ReadOnlyPropertyThroughLowerCaseField()
        {
            ReadOnlyPropertyThroughLowerCaseField(Prefix.None);
        }

        public void ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
        {
            if (prefix == Prefix.m) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldM);
            if (prefix == Prefix.mUnderscore) throw new InvalidPrefixException(InvalidPrefixLowerCaseFieldMUnderscore);

            setter("nosetter.lowercase" + prefix.Value);
        }

        public void ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
        {
            if (prefix == Prefix.None) throw new InvalidPrefixException(InvalidPrefixPascalCaseFieldNone);

            setter("nosetter.pascalcase" + prefix.Value);
        }

        public void Using(string propertyAccessorAssemblyQualifiedClassName)
        {
            setter(propertyAccessorAssemblyQualifiedClassName);
        }

        public void Using(Type propertyAccessorClassType)
        {
            Using(propertyAccessorClassType.AssemblyQualifiedName);
        }

        public void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor
        {
            Using(typeof(TPropertyAccessorClass));
        }
    }
}