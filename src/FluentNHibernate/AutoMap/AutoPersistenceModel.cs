using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public class AutoPersistenceModel : PersistenceModel
    {
        private readonly AutoMapper autoMapper;
        private Assembly assemblyContainingMaps;
        private Assembly entityAssembly;
        private Func<Type, bool> shouldIncludeType;
        private readonly List<AutoMapType> mappingTypes = new List<AutoMapType>();

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
                if (shouldIncludeType != null)
                {
                    if (!shouldIncludeType.Invoke(type))
                        continue;
                }

                if (Conventions.IsBaseType(type))
                    continue;
                if (type.IsAbstract)
                    continue;

                mappingTypes.Add(new AutoMapType(type));
            }

            foreach (var type in mappingTypes)
            {
                if (type.Type.IsClass && isnotAnonymousMethodClass(type))
                {
                    if (!type.IsMapped)
                    {
                        var mapping = FindMapping(type.Type);

                        if (mapping != null)
                            MergeMap(type.Type, mapping);
                        else
                            AddMapping(type.Type);
                    }
                }
            }

            base.Configure(configuration);
        }

        private bool isnotAnonymousMethodClass(AutoMapType type)
        {
            return type.Type.ReflectedType == null;
        }

        #region Configuation Helpers

        private void AddMapping(Type type)
        {
            Type typeToMap = GetTypeToMap(type);
            var mapping = InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                autoMapper, a => a.Map<object>(mappingTypes), new object[] {mappingTypes}, typeToMap);
            addMapping((IMapping)mapping);
        }

        private Type GetTypeToMap(Type type)
        {
            return Conventions.IsBaseType(type.BaseType) ? type : type.BaseType;
        }

        private void MergeMap(Type type, object mapping)
        {
            Type typeToMap = GetTypeToMap(type);
            InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                autoMapper, a => a.MergeMap<object>(null), new[] { mapping }, typeToMap);
        }

        private object FindMapping(Type type)
        {
            Type typeToMap = GetTypeToMap(type);
            var mapping = InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                this, a => a.FindMapping<object>(), null, typeToMap);
            return mapping;
        }

        #endregion

        public AutoPersistenceModel()
        {
            autoMapper = new AutoMapper(Conventions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapAssembly">Assembly Containing Maps</param>
        public AutoPersistenceModel(Assembly mapAssembly)
        {
            addMappingsFromAssembly(mapAssembly);
            autoMapper = new AutoMapper(Conventions);
        }

        public AutoPersistenceModel(AutoMapper customAutomapper)
        {
            autoMapper = customAutomapper;
        }

        public AutoPersistenceModel AutoMap<T>()
        {
            addMapping(autoMapper.Map<T>(mappingTypes));
            return this;
        }

        public IClassMap FindMapping<T>()
        {
            var mapping = (AutoMap<T>)_mappings.Find(t => t is AutoMap<T>);
            
            if (mapping != null) return mapping;
            
            // standard AutoMap<T> not found for the type, so looking for one for it's base type.
            return (IClassMap)_mappings.Find(t => t.GetType().GetGenericArguments()[0] == typeof(T).BaseType);
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
            if (_mappings.Exists(m => m.GetType() == typeof(AutoMap<T>)))
                throw new AutoMappingException("ForTypesThatDeriveFrom<T> called more than once for '" + typeof(T).Name + "'. Merge your calls into one.");

            var map= (AutoMap<T>) Activator.CreateInstance(typeof (AutoMap<T>));
            populateMap.Invoke(map);
            _mappings.Add(map);
            return this;
        }
    }

    public class AutoMapType
    {
        public AutoMapType(Type type)
        {
            Type = type;
        }

        public Type Type { get; set;}
        public bool IsMapped { get; set; }
    }
}
