using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ImportPart
    {
        readonly AttributeStore attributes = new AttributeStore();

        public ImportPart(Type importType)
        {
            attributes.Set("Class", Layer.Defaults, new TypeReference(importType));
        }

        /// <summary>
        /// Specify an alternative name for the type
        /// </summary>
        /// <param name="alternativeName">Alternative name</param>
        public void As(string alternativeName)
        {
            attributes.Set("Rename", Layer.UserSupplied, alternativeName);
        }

        internal ImportMapping GetImportMapping()
        {
            return new ImportMapping(attributes.Clone());
        }
    }
}