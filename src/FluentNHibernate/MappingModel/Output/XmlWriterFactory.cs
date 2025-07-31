using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output;

public static class XmlWriterFactory
{
    static readonly Container Container = new XmlWriterContainer();

    public static IXmlWriter<HibernateMapping> CreateHibernateMappingWriter()
    {
        return Container.Resolve<IXmlWriter<HibernateMapping>>();
    }
}
