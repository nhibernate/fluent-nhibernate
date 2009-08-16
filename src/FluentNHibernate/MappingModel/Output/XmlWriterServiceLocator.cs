using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlWriterServiceLocator : IXmlWriterServiceLocator
    {
        private readonly Container container;

        public XmlWriterServiceLocator(Container container)
        {
            this.container = container;
        }

        public IXmlWriter<T> GetWriter<T>()
        {
            return container.Resolve<IXmlWriter<T>>();
        }
    }
}