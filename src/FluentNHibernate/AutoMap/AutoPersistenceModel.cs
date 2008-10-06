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
        private Assembly entityAssembly;
        private Func<Type, bool> shouldIncludeType;

        public AutoPersistenceModel WithConvention(Conventions convention)
        {
            Conventions = convention;
            return this;
        }

        public AutoPersistenceModel WithConvention(Action<Conventions> conventionAction)
        {
            conventionAction.Invoke(Conventions);
            return this;
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

            foreach (var type in entityAssembly.GetTypes())
            {
                if (shouldIncludeType!= null)
                {
                    if (!shouldIncludeType.Invoke(type))
                        continue;
                }

                if (type.IsClass)
                {
                    var mapping = FindMapping(type);

                    if (mapping != null)
                        MergeMap(type, mapping);
                    else
                        AddMapping(type);
                }
            }

            base.Configure(configuration);
        }

        #region Configuation Helpers

        private object AddMapping(Type type)
        {
            var mapping = InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                autoMap, a => a.Map<object>(), null, type);
            addMapping((IMapping)mapping);
            return mapping;
        }

        private void MergeMap(Type type, object mapping)
        {
            InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                autoMap, a => a.MergeMap<object>(null), new[] { mapping }, type);
        }

        private object FindMapping(Type type)
        {
            var mapping = InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                this, a => a.FindMapping<object>(), null, type);
            return mapping;
        }

        #endregion

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
