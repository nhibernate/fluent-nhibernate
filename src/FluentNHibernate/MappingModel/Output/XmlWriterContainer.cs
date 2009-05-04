using System;
using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlWriterContainer : Container
    {
        public XmlWriterContainer()
        {
            RegisterWriter<HibernateMapping>(c =>
                new XmlHibernateMappingWriter(c.Resolve<IXmlWriter<ClassMapping>>(), c.Resolve<IXmlWriter<ImportMapping>>()));

            RegisterWriter<ClassMapping>(c =>
                new XmlClassWriter(c.Resolve<IXmlWriter<PropertyMapping>>(), c.Resolve<IXmlWriter<DiscriminatorMapping>>(), c.Resolve<IXmlWriter<ISubclassMapping>>()));

            RegisterWriter<ImportMapping>(c =>
                new XmlImportWriter());

            RegisterWriter<PropertyMapping>(c =>
                new XmlPropertyWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            RegisterWriter<ColumnMapping>(c =>
                new XmlColumnWriter());

            RegisterWriter<DiscriminatorMapping>(c =>
                new XmlDiscriminatorWriter(c.Resolve<IXmlWriter<ColumnMapping>>()));

            // subclasses
            RegisterWriter<ISubclassMapping>(c =>
                new XmlInheritanceWriter(c.Resolve<IXmlWriter<SubclassMapping>>()));

            RegisterWriter<SubclassMapping>(c =>
                new XmlSubclassWriter(c.Resolve<IXmlWriter<PropertyMapping>>()));
        }

        private void RegisterWriter<T>(Func<Container, object> instantiate)
        {
            Register<IXmlWriter<T>>(instantiate);
        }
    }
}