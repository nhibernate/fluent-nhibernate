using System;
using FluentNHibernate.Mapping;
using NHibernate.Properties;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IAccessInstance
    {
        void Property();
        void Field();
        void CamelCaseField();
        void CamelCaseField(Prefix prefix);
        void LowerCaseField();
        void LowerCaseField(Prefix prefix);
        void PascalCaseField(Prefix prefix);
        void ReadOnlyPropertyThroughCamelCaseField();
        void ReadOnlyPropertyThroughCamelCaseField(Prefix prefix);
        void ReadOnlyPropertyThroughLowerCaseField();
        void ReadOnlyPropertyThroughLowerCaseField(Prefix prefix);
        void ReadOnlyPropertyThroughPascalCaseField(Prefix prefix);
        void Using(string propertyAccessorAssemblyQualifiedClassName);
        void Using(Type propertyAccessorClassType);
        void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor;
    }
}