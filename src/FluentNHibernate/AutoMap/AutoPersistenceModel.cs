using System;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoPersistenceModel : PersistenceModel
    {
        private readonly AutoMapper autoMap;
        private Assembly assemblyContainingMaps;
        private Func<Type, bool> shouldIncludeType = t => true;
        private Assembly entityAssembly;

        public ConventionBuilder WithConvention
        {
            get { return new ConventionBuilder(this); }
        }

        public static AutoPersistenceModel MapEntitiesFromAssemblyOf<T>()
        {
            var persistenceModel = new AutoPersistenceModel();
            persistenceModel.AddEntityAssembly(Assembly.GetAssembly(typeof (T)));
            return persistenceModel;
        }

        public AutoPersistenceModel Where(Func<Type, bool> shouldIncludeType)
        {
            this.shouldIncludeType = shouldIncludeType;
            return this;
        }

        public AutoPersistenceModel MergeWithAutoMapsFromAssemblyOf<T>()
        {
            assemblyContainingMaps = Assembly.GetAssembly(typeof (T));
            return this;
        }

        public override void Configure(NHibernate.Cfg.Configuration configuration)
        {
            if (assemblyContainingMaps != null)
                addMappingsFromAssembly(assemblyContainingMaps);

            foreach (var obj in entityAssembly.GetTypes())
            {
		if (obj.IsClass && obj.BaseType == typeof(object))
                {
                    if (shouldIncludeType.Invoke(obj))
                    {
                        // Find the map that already exists
                        var findMapMethod = typeof (AutoPersistenceModel).GetMethod("FindMapping");
                        var genericFindMapMethod = findMapMethod.MakeGenericMethod(obj);
                        var mapping = genericFindMapMethod.Invoke(this, null);

                        if (mapping != null)
                        {
                            // Merge Mappings together
                            var findAutoMapMethod = typeof (AutoMapper).GetMethod("MergeMap");
                            var genericfindAutoMapMethod = findAutoMapMethod.MakeGenericMethod(obj);
                            genericfindAutoMapMethod.Invoke(autoMap, new[] {mapping});
                        }
                        else
                        {
                            //Auto magically map the entity
                            var findAutoMapMethod = typeof (AutoMapper).GetMethod("Map", new Type[0]);
                            var genericfindAutoMapMethod = findAutoMapMethod.MakeGenericMethod(obj);
                            addMapping((IMapping) genericfindAutoMapMethod.Invoke(autoMap, null));
                        }
                    }
                }
            }

            base.Configure(configuration);
        }

        public AutoPersistenceModel()
        {
            autoMap = new AutoMapper(Conventions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapAssembly">Assembly Containing Maps</param>
        public AutoPersistenceModel(Assembly mapAssembly)
        {
            addMappingsFromAssembly(mapAssembly);
            autoMap = new AutoMapper(Conventions);
        }

        public AutoPersistenceModel AutoMap<T>()
        {
            addMapping(autoMap.Map<T>());
            return this;
        }

        public AutoMap<T> FindMapping<T>()
        {
            return (AutoMap<T>)_mappings.Find(t => t is AutoMap<T>);
        }

        public void OutputMappings()
        {
            foreach(var map in _mappings)
                Console.WriteLine(map);
        }

        public AutoPersistenceModel AddEntityAssembly(Assembly assembly)
        {
            entityAssembly = assembly;
            return this;
        }

        public AutoPersistenceModel ForTypesThatDeriveFrom<T>(Action<AutoMap<T>> populateMap)
        {
            var map= (AutoMap<T>) Activator.CreateInstance(typeof (AutoMap<T>));
            populateMap.Invoke(map);
            _mappings.Add(map);
            return this;
        }
    }
}