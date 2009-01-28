using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg;
using FluentNHibernate.Xml;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        private readonly IList<ClassMapping> _mappings;
        private readonly IList<IMappingModelVisitor> _visitors;

        public PersistenceModel()
        {
            _mappings = new List<ClassMapping>();
            _visitors = new List<IMappingModelVisitor>();
        }

        public PersistenceModel(IEnumerable<IMappingProvider> providers, IList<IMappingModelVisitor> visitors)
        {
            foreach(var mapping in providers.Select(p => p.GetClassMapping()))
                Add(mapping);
            _visitors = visitors;
        }

        public void Add(IMappingProvider provider)
        {
            Add(provider.GetClassMapping());
        }

        public void Add(ClassMapping mapping)
        {
            _mappings.Add(mapping);
        }

        public void AddConvention(IMappingModelVisitor visitor)
        {
            _visitors.Add(visitor);
        }

        public IEnumerable<ClassMapping> Mappings
        {
            get { return _mappings; }
        }

        public HibernateMapping BuildHibernateMapping()
        {
            var rootMapping = new HibernateMapping();
            rootMapping.DefaultLazy = false;

            foreach (var classMapping in _mappings)
                rootMapping.AddClass(classMapping);

            return rootMapping;
        }

        public void ApplyVisitors(HibernateMapping rootMapping)
        {
            foreach (var visitor in _visitors)
                rootMapping.AcceptVisitor(visitor);
        }


        public void Configure(Configuration cfg)
        {
            var rootMapping = BuildHibernateMapping();         
   
            ApplyVisitors(rootMapping);

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