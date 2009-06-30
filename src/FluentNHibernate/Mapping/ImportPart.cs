using System;
using System.Xml;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ImportPart
    {
        private readonly ImportMapping mapping = new ImportMapping();

        public ImportPart(Type importType)
        {
            mapping.Class = new TypeReference(importType);
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