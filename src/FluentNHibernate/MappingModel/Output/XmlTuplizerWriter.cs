using System;
using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlTuplizerWriter : NullMappingModelVisitor, IXmlWriter<TuplizerMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(TuplizerMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessTuplizer(TuplizerMapping tuplizerMapping)
        {
            document = new XmlDocument();

            var element = document.CreateElement("tuplizer");

            if (tuplizerMapping.HasValue(x => x.Mode))
                element.WithAtt("entity-mode", GetModeString(tuplizerMapping.Mode));

            if (tuplizerMapping.HasValue(x => x.Type))
                element.WithAtt("class", tuplizerMapping.Type);

            if (tuplizerMapping.HasValue(x => x.EntityName))
                element.WithAtt("entity-name", tuplizerMapping.EntityName);

            document.AppendChild(element);
        }

        private string GetModeString(TuplizerMode mode)
        {
            switch(mode)
            {
                case TuplizerMode.Poco:
                    return "poco";
                case TuplizerMode.DynamicMap:
                    return "dynamic-map";
                case TuplizerMode.Xml:
                    return "xml";
                default:
                    throw new ArgumentException(string.Format("Unknown tuplizer entity mode '{0}'.", mode));
            }
        }
    }
}