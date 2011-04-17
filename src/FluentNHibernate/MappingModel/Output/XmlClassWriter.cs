using System.Xml;
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

            if (classMapping.IsSpecified("BatchSize"))
                classElement.WithAtt("batch-size", classMapping.BatchSize);

            if (classMapping.IsSpecified("DiscriminatorValue"))
                classElement.WithAtt("discriminator-value", classMapping.DiscriminatorValue.ToString());

            if (classMapping.IsSpecified("DynamicInsert"))
                classElement.WithAtt("dynamic-insert", classMapping.DynamicInsert);

            if (classMapping.IsSpecified("DynamicUpdate"))
                classElement.WithAtt("dynamic-update", classMapping.DynamicUpdate);

            if (classMapping.IsSpecified("Lazy"))
                classElement.WithAtt("lazy", classMapping.Lazy);

            if (classMapping.IsSpecified("Schema"))
                classElement.WithAtt("schema", classMapping.Schema);

            if (classMapping.IsSpecified("Mutable"))
                classElement.WithAtt("mutable", classMapping.Mutable);

            if (classMapping.IsSpecified("Polymorphism"))
                classElement.WithAtt("polymorphism", classMapping.Polymorphism);

            if (classMapping.IsSpecified("Persister"))
                classElement.WithAtt("persister", classMapping.Persister);

            if (classMapping.IsSpecified("Where"))
                classElement.WithAtt("where", classMapping.Where);

            if (classMapping.IsSpecified("OptimisticLock"))
                classElement.WithAtt("optimistic-lock", classMapping.OptimisticLock);

            if (classMapping.IsSpecified("Check"))
                classElement.WithAtt("check", classMapping.Check);

            if (classMapping.IsSpecified("Name"))
                classElement.WithAtt("name", classMapping.Name);

            if (classMapping.IsSpecified("TableName"))
                classElement.WithAtt("table", classMapping.TableName);

            if (classMapping.IsSpecified("Proxy"))
                classElement.WithAtt("proxy", classMapping.Proxy);

            if (classMapping.IsSpecified("SelectBeforeUpdate"))
                classElement.WithAtt("select-before-update", classMapping.SelectBeforeUpdate);

            if (classMapping.IsSpecified("Abstract"))
                classElement.WithAtt("abstract", classMapping.Abstract);

            if (classMapping.IsSpecified("Subselect"))
                classElement.WithAtt("subselect", classMapping.Subselect);

            if (classMapping.IsSpecified("SchemaAction"))
                classElement.WithAtt("schema-action", classMapping.SchemaAction);

            if (classMapping.IsSpecified("EntityName"))
                classElement.WithAtt("entity-name", classMapping.EntityName);
        }

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            var writer = serviceLocator.GetWriter<DiscriminatorMapping>();
            var discriminatorXml = writer.Write(discriminatorMapping);

            document.ImportAndAppendChild(discriminatorXml);
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            var writer = serviceLocator.GetWriter<SubclassMapping>();
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

        public override void Visit(NaturalIdMapping naturalIdMapping)
        {
            var writer = serviceLocator.GetWriter<NaturalIdMapping>();
            var naturalIdXml = writer.Write(naturalIdMapping);

            document.ImportAndAppendChild(naturalIdXml);
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

        public override void Visit(FilterMapping filterMapping)
        {
            var writer = serviceLocator.GetWriter<FilterMapping>();
            var filterXml = writer.Write(filterMapping);

            document.ImportAndAppendChild(filterXml);
        }

        public override void Visit(TuplizerMapping mapping)
        {
            var writer = serviceLocator.GetWriter<TuplizerMapping>();
            var filterXml = writer.Write(mapping);

            document.ImportAndAppendChild(filterXml);
        }
    }
}
