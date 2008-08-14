using System;
using System.Reflection;
using FluentNHibernate;

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
        }

        public AutoPersistenceModel AutoMap<T>()
        {
            addMapping(autoMap.Map<T>());
            return this;
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
                    var mapping = executeMethod(obj, this, "findMapping", null);
                    if (mapping != null)
                    {
                        executeMethod(obj, autoMap, "Map", new[] {mapping});
                    }
                    else
                    {
                        executeMethod(obj, autoMap, "Map", null);
                    }
                }
            }

            return this;
        }

        private object executeMethod(Type obj, Object invokeOn, string methodName, object[] arguements)
        {
            var findMapMethod = autoMap.GetType().GetMethod(methodName);
            var genericFindMapMethod = findMapMethod.MakeGenericMethod(obj);
            return genericFindMapMethod.Invoke(invokeOn, arguements);
        }
    }
}