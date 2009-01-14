using FluentNHibernate.Mapping;

namespace FluentNHibernate
{
    public interface IPropertyConvention
    {
        bool CanHandle(IProperty property);
        void Process(IProperty propertyMapping);
    }
}