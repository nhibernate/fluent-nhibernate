using System.Xml;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlGeneratorWriter : NullMappingModelVisitor, IXmlWriter<GeneratorMapping>
    {
        private XmlDocument document;

        public XmlDocument Write(GeneratorMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessGenerator(GeneratorMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("generator");

            if (mapping.HasValue(x => x.Class))
                element.WithAtt("class", mapping.Class);

            foreach (var param in mapping.Params)
            {
                element.AddElement("param")
                    .WithAtt("name", param.Key)
                    .InnerXml = param.Value;
            }
        }
    }
}