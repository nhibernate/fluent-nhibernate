using System;
using System.Xml;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ImportPart : IMappingPart
    {
        private readonly ImportMapping mapping = new ImportMapping();

        public ImportPart(Type importType)
        {
            mapping.Type = importType;
        }

        public void SetAttribute(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void SetAttributes(Attributes attrs)
        {
            throw new NotImplementedException();
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {}

        public void As(string alternativeName)
        {
            mapping.Rename = alternativeName;
        }

        public int Level
        {
            get { return -1; }
        }

        public PartPosition Position
        {
            get { return PartPosition.First; }
        }

        public ImportMapping GetImportMapping()
        {
            return mapping;
        }
    }
}