using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output.Sorting;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlHibernateMappingWriter : NullMappingModelVisitor, IXmlWriter<HibernateMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlHibernateMappingWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessHibernateMapping(HibernateMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("hibernate-mapping");

            element.WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (mapping.HasValue(x => x.DefaultAccess))
                element.WithAtt("default-access", mapping.DefaultAccess);

            if (mapping.HasValue(x => x.AutoImport))
                element.WithAtt("auto-import", mapping.AutoImport);

            if (mapping.HasValue(x => x.Schema))
                element.WithAtt("schema", mapping.Schema);

            if (mapping.HasValue(x => x.DefaultCascade))
                element.WithAtt("default-cascade", mapping.DefaultCascade);

            if (mapping.HasValue(x => x.DefaultLazy))
                element.WithAtt("default-lazy", mapping.DefaultLazy);

            if (mapping.HasValue(x => x.Catalog))
                element.WithAtt("catalog", mapping.Catalog);

            if (mapping.HasValue(x => x.Namespace))
                element.WithAtt("namespace", mapping.Namespace);

            if (mapping.HasValue(x => x.Assembly))
                element.WithAtt("assembly", mapping.Assembly);
        }

        public override void Visit(ImportMapping importMapping)
        {
            var writer = serviceLocator.GetWriter<ImportMapping>();
            var import = writer.Write(importMapping);
            var newNode = document.ImportNode(import.DocumentElement, true);

            if (document.DocumentElement.ChildNodes.Count > 0)
                document.DocumentElement.InsertBefore(newNode, document.DocumentElement.ChildNodes[0]);
            else
                document.DocumentElement.AppendChild(newNode);
        }

        public override void Visit(ClassMapping classMapping)
        {
            var writer = serviceLocator.GetWriter<ClassMapping>();
            var hbmClass = writer.Write(classMapping);

            var newClassNode = document.ImportNode(hbmClass.DocumentElement, true);

            XmlNodeSorter.SortClassChildren(newClassNode);

            document.DocumentElement.AppendChild(newClassNode);
        }
    }
}