using System;
using System.Xml;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ImportPart
    {
        private readonly AttributeStore<ImportMapping> attributes = new AttributeStore<ImportMapping>();

        public ImportPart(Type importType)
        {
            attributes.SetDefault(x => x.Class, new TypeReference(importType));
        }

        public void As(string alternativeName)
        {
            attributes.Set( x=> x.Rename, alternativeName);
        }

        public ImportMapping GetImportMapping()
        {
            return new ImportMapping(attributes.CloneInner());
        }
    }
}