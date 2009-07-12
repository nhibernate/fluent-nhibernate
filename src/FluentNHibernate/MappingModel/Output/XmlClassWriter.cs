using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlClassWriter : XmlClassWriterBase, IXmlWriter<ClassMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        public XmlClassWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ClassMapping mapping)
        {
            document = null;
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            document = new XmlDocument();

            var classElement = document.AddElement("class")
                .WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (classMapping.IsSpecified(x => x.BatchSize))
                classElement.WithAtt("batch-size", classMapping.BatchSize);

            if (classMapping.IsSpecified(x => x.DiscriminatorValue))
                classElement.WithAtt("discriminator-value", classMapping.DiscriminatorValue.ToString());

            if (classMapping.IsSpecified(x => x.DynamicInsert))
                classElement.WithAtt("dynamic-insert", classMapping.DynamicInsert);

            if (classMapping.IsSpecified(x => x.DynamicUpdate))
                classElement.WithAtt("dynamic-update", classMapping.DynamicUpdate);

            if (classMapping.IsSpecified(x => x.Lazy))
                classElement.WithAtt("lazy", classMapping.Lazy);

            if (classMapping.IsSpecified(x => x.Schema))
                classElement.WithAtt("schema", classMapping.Schema);

            if (classMapping.IsSpecified(x => x.Mutable))
                classElement.WithAtt("mutable", classMapping.Mutable);

            if (classMapping.IsSpecified(x => x.Polymorphism))
                classElement.WithAtt("polymorphism", classMapping.Polymorphism);

            if (classMapping.IsSpecified(x => x.Persister))
                classElement.WithAtt("persister", classMapping.Persister);

            if (classMapping.IsSpecified(x => x.Where))
                classElement.WithAtt("where", classMapping.Where);

            if (classMapping.IsSpecified(x => x.OptimisticLock))
                classElement.WithAtt("optimistic-lock", classMapping.OptimisticLock);

            if (classMapping.IsSpecified(x => x.Check))
                classElement.WithAtt("check", classMapping.Check);

            if (classMapping.IsSpecified(x => x.Name))
                classElement.WithAtt("name", classMapping.Name);

            if (classMapping.IsSpecified(x => x.TableName))
                classElement.WithAtt("table", classMapping.TableName);

            if (classMapping.IsSpecified(x => x.Proxy))
                classElement.WithAtt("proxy", classMapping.Proxy);

            if (classMapping.IsSpecified(x => x.SelectBeforeUpdate))
                classElement.WithAtt("select-before-update", classMapping.SelectBeforeUpdate);

            if (classMapping.IsSpecified(x => x.Abstract))
                classElement.WithAtt("abstract", classMapping.Abstract);
        }

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            var writer = serviceLocator.GetWriter<DiscriminatorMapping>();
            var discriminatorXml = writer.Write(discriminatorMapping);

            document.ImportAndAppendChild(discriminatorXml);
        }

        public override void Visit(ISubclassMapping subclassMapping)
        {
            var writer = serviceLocator.GetWriter<ISubclassMapping>();
            var subclassXml = writer.Write(subclassMapping);

            document.ImportAndAppendChild(subclassXml);
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            var writer = serviceLocator.GetWriter<IComponentMapping>();
            var componentXml = writer.Write(componentMapping);

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(JoinMapping joinMapping)
        {
            var writer = serviceLocator.GetWriter<JoinMapping>();
            var joinXml = writer.Write(joinMapping);

            document.ImportAndAppendChild(joinXml);
        }

        public override void Visit(IIdentityMapping mapping)
        {
            var writer = serviceLocator.GetWriter<IIdentityMapping>();
            var idXml = writer.Write(mapping);

            document.ImportAndAppendChild(idXml);
        }

        public override void Visit(CacheMapping mapping)
        {
            var writer = serviceLocator.GetWriter<CacheMapping>();
            var cacheXml = writer.Write(mapping);

            document.ImportAndAppendChild(cacheXml);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            var writer = serviceLocator.GetWriter<ManyToOneMapping>();
            var manyToOneXml = writer.Write(manyToOneMapping);

            document.ImportAndAppendChild(manyToOneXml);
        }
    }
}
