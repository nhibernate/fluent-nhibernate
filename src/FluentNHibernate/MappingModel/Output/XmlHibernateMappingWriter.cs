using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Mapping;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlHibernateMappingWriter : NullMappingModelVisitor, IXmlWriter<HibernateMapping>
    {
        private readonly IXmlWriter<ClassMapping> _classWriter;
        private readonly IXmlWriter<ImportMapping> _importWriter;
        private XmlDocument document;

        public XmlHibernateMappingWriter(IXmlWriter<ClassMapping> classWriter, IXmlWriter<ImportMapping> importWriter)
        {
            _classWriter = classWriter;
            _importWriter = importWriter;
        }

        public XmlDocument Write(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            document = new XmlDocument();
            
            var element = document.CreateElement("hibernate-mapping");

            element.WithAtt("xmlns", "urn:nhibernate-mapping-2.2");

            if (hibernateMapping.Attributes.IsSpecified(x => x.DefaultAccess))
                element.WithAtt("default-access", hibernateMapping.DefaultAccess);

            if (hibernateMapping.Attributes.IsSpecified(x => x.AutoImport))
                element.WithAtt("auto-import", hibernateMapping.AutoImport.ToString().ToLowerInvariant());

            document.AppendChild(element);
        }

        public override void Visit(ImportMapping importMapping)
        {
            var import = _importWriter.Write(importMapping);
            var newNode = document.ImportNode(import.DocumentElement, true);

            if (document.DocumentElement.ChildNodes.Count > 0)
                document.DocumentElement.InsertBefore(newNode, document.DocumentElement.ChildNodes[0]);
            else
                document.DocumentElement.AppendChild(newNode);
        }

        public override void Visit(ClassMapping classMapping)
        {
            var hbmClass = _classWriter.Write(classMapping);

            var newClassNode = document.ImportNode(hbmClass.DocumentElement, true);

            document.DocumentElement.AppendChild(newClassNode);
        }
    }
}