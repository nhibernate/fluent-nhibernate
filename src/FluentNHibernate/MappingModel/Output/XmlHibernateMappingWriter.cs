using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlHibernateMappingWriter : NullMappingModelVisitor, IXmlWriter<HibernateMapping>
    {
        private readonly IXmlWriter<ClassMapping> _classWriter;
        private XmlDocument document;

        public XmlHibernateMappingWriter(IXmlWriter<ClassMapping> classWriter)
        {
            _classWriter = classWriter;
        }

        public XmlDocument Write(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return document;
        }

        public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream = executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + ".Mapping.Template.xml");
            
            document = new XmlDocument();
            document.Load(stream);
        }

        public override void Visit(ClassMapping classMapping)
        {
            var hbmClass = _classWriter.Write(classMapping);

            var newClassNode = document.ImportNode(hbmClass.DocumentElement, true);

            document.DocumentElement.AppendChild(newClassNode);
        }
    }
}