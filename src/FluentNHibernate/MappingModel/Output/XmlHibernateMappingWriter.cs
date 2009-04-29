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

            SortChildren(newClassNode);

            document.DocumentElement.AppendChild(newClassNode);
        }
        
        private class SortValue
        {
            public PartPosition Position { get; set; }
            public int Level { get; set; }
        }

        private void SortChildren(XmlNode node)
        {
            var sorting = new Dictionary<string, SortValue>
            {
                { "cache", new SortValue { Position = PartPosition.First, Level = 1 } },
                { "id", new SortValue { Position = PartPosition.First, Level = 2 } },
                { "composite-id", new SortValue { Position = PartPosition.First, Level = 2 } },
                { "discriminator", new SortValue { Position = PartPosition.First, Level = 3 } },
                { "version", new SortValue { Position = PartPosition.First, Level = 4 } },
                { "property", new SortValue { Position = PartPosition.Anywhere, Level = 2 } },
                { "many-to-one", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "array", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "bag", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "set", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "map", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "list", new SortValue { Position = PartPosition.Anywhere, Level = 3 } },
                { "joined-subclass", new SortValue { Position = PartPosition.Anywhere, Level = 4 } },
                { "subclass", new SortValue { Position = PartPosition.Last, Level = 3 } },
                { "join", new SortValue { Position = PartPosition.Last, Level = 3 } },
            };
            var children = new List<XmlNode>();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                children.Add(childNode);
            }

            children.Sort((x, y) =>
            {
                if (!sorting.ContainsKey(x.Name) || !sorting.ContainsKey(y.Name)) return 0;

                var xSort = sorting[x.Name];
                var ySort = sorting[y.Name];

                // this isn't exactly nice, but it works (and it's covered by tests... hint hint)
                if (xSort.Position == PartPosition.First && ySort.Position != PartPosition.First) return -1;
                if (xSort.Position == PartPosition.Last && ySort.Position != PartPosition.Last) return 1;
                if (xSort.Position == PartPosition.Anywhere && ySort.Position == PartPosition.First) return 1;
                if (xSort.Position == PartPosition.Anywhere && ySort.Position == PartPosition.Last) return -1;

                return xSort.Level.CompareTo(ySort.Level);
            });

            for (var i = 0; i < node.ChildNodes.Count; i++)
            {
                node.RemoveChild(node.ChildNodes[i]);
            }

            foreach (var child in children)
            {
                Console.WriteLine(child.Name);
                node.AppendChild(child);
            }
        }
    }
}