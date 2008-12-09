using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;
using FluentNHibernate.Xml;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        private readonly IList<ClassMapping> _mappings;

        public PersistenceModel()
        {
            _mappings = new List<ClassMapping>();
        }        

        public PersistenceModel(IEnumerable<IMappingProvider> providers)
        {
            foreach(var mapping in providers.Select(p => p.GetClassMapping()))
                Add(mapping);
        }

        public void Add(IMappingProvider provider)
        {
            Add(provider.GetClassMapping());
        }

        public void Add(ClassMapping mapping)
        {
            _mappings.Add(mapping);
        }

        public IEnumerable<ClassMapping> Mappings
        {
            get { return _mappings; }
        }

        public void Configure(Configuration cfg)
        {
            var rootMapping = new HibernateMapping();
            rootMapping.DefaultLazy = false;
            
            foreach(var classMapping in _mappings)
                rootMapping.AddClass(classMapping);

            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(rootMapping);

            cfg.AddDocument(document);
        }
    }

    public interface IMappingProvider
    {
        ClassMapping GetClassMapping();
    }
}