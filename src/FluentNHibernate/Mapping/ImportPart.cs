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
            mapping.Class = importType.AssemblyQualifiedName;
        }

        public void As(string alternativeName)
        {
            mapping.Rename = alternativeName;
        }

        public ImportMapping GetImportMapping()
        {
            return mapping;
        }
    }
}