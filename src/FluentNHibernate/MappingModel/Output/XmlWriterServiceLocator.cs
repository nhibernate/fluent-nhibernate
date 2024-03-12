using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output;

public class XmlWriterServiceLocator(Container container) : IXmlWriterServiceLocator
{
    public IXmlWriter<T> GetWriter<T>()
    {
        return container.Resolve<IXmlWriter<T>>();
    }
}
