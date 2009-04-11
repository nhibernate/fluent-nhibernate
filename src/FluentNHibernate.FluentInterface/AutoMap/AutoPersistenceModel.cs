using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate.FluentInterface.AutoMap
{
    /// <summary>
    /// Note This may be better in the lower FluentNHibernate assembly.
    /// </summary>
    public class AutoPersistenceModel
    {
        private readonly IAutoMapper[] autoMappers;
        private readonly List<ClassMapping> classesFound = new List<ClassMapping>();
        private Assembly assembly;

        public AutoPersistenceModel(Type type, IAutoMapper[] autoMappers)
        {
            this.autoMappers = autoMappers;
            assembly = type.Assembly;
        }

        public static AutoPersistenceModel MapEntitiesFromAssemblyOf<T>()
        {
            return new AutoPersistenceModel(typeof(T), new IAutoMapper[] { new IdAutoMapper(), new PropertyAutoMapper() });
        }

        public AutoPersistenceModel Where(Func<Type, bool> func)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (func.Invoke(type))
                    classesFound.Add(new ClassMapping
                                               {
                                                   Name = type.FullName,
                                                   Tablename = type.Name,
                                                   Type = type
                                               });
            }
            return this;
        }

        public HibernateMapping BuildHibernateMapping()
        {
            var rootMapping = new HibernateMapping {DefaultLazy = false};

            foreach (var classMapping in classesFound)
                rootMapping.AddClass(classMapping);

            return rootMapping;
        }

        public void Configure(Configuration cfg)
        {
            cfg.AddDocument(OutputXml());
        }

        private void autoMap()
        {
            foreach (var autoMapper in autoMappers)
            {
                foreach (var classFound in classesFound)
                {
                    autoMapper.Map(classFound);    
                }
            }
        }

        public AutoPersistenceModel ForTypesThatDeriveFrom<T>(Action<ClassMapOld<T>> populateMap)
        {
            var map = new ClassMapOld<T>();
            populateMap.Invoke(map);
            
            classesFound.RemoveAll(q => q.Type == typeof (T));
            classesFound.Add(map.GetClassMapping());

            return this;
        }

        public List<ClassMapping> ClassesFound
        {
            get { return classesFound; }
        }

        public Assembly Assembly
        {
            get { return assembly; }
        }

        public XmlDocument OutputXml()
        {
            autoMap();
            var rootMapping = BuildHibernateMapping();
            var serializer = new MappingXmlSerializer();
            return serializer.Serialize(rootMapping);
        }
    }
}