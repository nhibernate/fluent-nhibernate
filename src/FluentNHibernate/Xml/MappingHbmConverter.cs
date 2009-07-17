using System.Collections.Generic;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace FluentNHibernate.Xml
{
    public class MappingHbmConverter
    {
        public ConvertedClassesResult ConvertToNHClasses(HibernateMapping mapping)
        {
            return BuildXml(mapping);
        }

        private static ConvertedClassesResult BuildXml(HibernateMapping rootMapping)
        {
            var writer = NHModelWriterFactory.CreateHibernateMappingWriter();

            return (ConvertedClassesResult)writer.Write(rootMapping);
        }
    }

    public class ConvertedClassesResult
    {
        public IList<PersistentClass> Classes { get; set; }
        public IList<Collection> Collections { get; set; }
    }
}
