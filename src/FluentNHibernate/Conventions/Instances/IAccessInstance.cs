using System;
using FluentNHibernate.Conventions.Inspections;
using NHibernate.Properties;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IAccessInstance
    {
        void Property();
        void Field();
        void BackField();
        void CamelCaseField();
        void CamelCaseField(CamelCasePrefix prefix);
        void LowerCaseField();
        void LowerCaseField(LowerCasePrefix prefix);
        void PascalCaseField(PascalCasePrefix prefix);
        void ReadOnlyProperty();
        void ReadOnlyPropertyThroughCamelCaseField();
        void ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix);
        void ReadOnlyPropertyThroughLowerCaseField();
        void ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix);
        void ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix);
        void Using(string propertyAccessorAssemblyQualifiedClassName);
        void Using(Type propertyAccessorClassType);
        void Using<TPropertyAccessorClass>() where TPropertyAccessorClass : IPropertyAccessor;
        void NoOp();
        void None();
    }
}