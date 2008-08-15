using System;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoPersistenceModel : PersistenceModel
    {
        private readonly AutoMapper autoMap;

        public AutoPersistenceModel()
        {
            autoMap = new AutoMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapAssembly">Assembly Containing Maps</param>
        public AutoPersistenceModel(Assembly mapAssembly)
        {
            addMappingsFromAssembly(mapAssembly);
            autoMap = new AutoMapper();
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


        /// <summary>
        /// NOTE: Experimental: Doesn't take into account existing mappings so will map properties twice at the moment
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        public AutoPersistenceModel AddEntityAssembly(Assembly assembly, Func<Type, bool> evaluation)
        {
            foreach(var obj in assembly.GetTypes())
            {
                if (evaluation.Invoke(obj))
                {
                    // Find the map that already exists
                    var findMapMethod = typeof(AutoPersistenceModel).GetMethod("FindMapping");
                    var genericFindMapMethod = findMapMethod.MakeGenericMethod(obj);
                    var mapping = genericFindMapMethod.Invoke(this, null);

                    if (mapping != null)
                    {
                        // Merge Mappings together
                        var findAutoMapMethod = typeof(AutoMapper).GetMethod("MergeMap");
                        var genericfindAutoMapMethod = findAutoMapMethod.MakeGenericMethod(obj);
                        genericfindAutoMapMethod.Invoke(autoMap, new[] {mapping});
                    }
                    else
                    {
                        //Auto magically map the entity
                        var findAutoMapMethod = typeof(AutoMapper).GetMethod("Map", new Type[0]);
                        var genericfindAutoMapMethod = findAutoMapMethod.MakeGenericMethod(obj);
                        addMapping((IMapping) genericfindAutoMapMethod.Invoke(autoMap, null));
                    }
                }
            }

            return this;
        }
    }
}