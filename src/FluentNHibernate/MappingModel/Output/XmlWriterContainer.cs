using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlWriterContainer : Container
    {
        public XmlWriterContainer()
        {
            Register<IXmlWriter<HibernateMapping>>(c =>
                new XmlHibernateMappingWriter(c.Resolve<IXmlWriter<ClassMapping>>(), c.Resolve<IXmlWriter<ImportMapping>>()));

            Register<IXmlWriter<ClassMapping>>(c =>
                new XmlClassWriter(c.Resolve<IXmlWriter<PropertyMapping>>()));

            Register<IXmlWriter<ImportMapping>>(c =>
                new XmlImportWriter());

            Register<IXmlWriter<PropertyMapping>>(c =>
                new XmlPropertyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            Register<IXmlWriter<ColumnMapping>>(c =>
                new XmlColumnWriter());
        }
    }
}