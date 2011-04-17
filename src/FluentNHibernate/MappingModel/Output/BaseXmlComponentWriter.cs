using System;
using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public abstract class BaseXmlComponentWriter : XmlClassWriterBase
    {
        private readonly IXmlWriterServiceLocator serviceLocator;

        protected BaseXmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
            : base(serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        protected XmlDocument WriteComponent(string element, IComponentMapping mapping)
        {
            var doc = new XmlDocument();
            var componentElement = doc.AddElement(element);

            if (mapping.IsSpecified("Name"))
                componentElement.WithAtt("name", mapping.Name);

            if (mapping.IsSpecified("Insert"))
                componentElement.WithAtt("insert", mapping.Insert);

            if (mapping.IsSpecified("Update"))
                componentElement.WithAtt("update", mapping.Update);

            if (mapping.IsSpecified("Access"))
                componentElement.WithAtt("access", mapping.Access);

            if (mapping.IsSpecified("OptimisticLock"))
                componentElement.WithAtt("optimistic-lock", mapping.OptimisticLock);

            return doc;
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            var writer = new XmlComponentWriter(serviceLocator);
            var componentXml = writer.Write(componentMapping);

            document.ImportAndAppendChild(componentXml);
        }

        public override void Visit(ParentMapping parentMapping)
        {
            var writer = serviceLocator.GetWriter<ParentMapping>();
            var parentXml = writer.Write(parentMapping);

            document.ImportAndAppendChild(parentXml);
        }
    }
}
