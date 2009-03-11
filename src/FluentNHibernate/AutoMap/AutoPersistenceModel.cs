using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            autoMapper = new PrivateAutoMapper(Conventions);
        }
    }

    public class AutoPersistenceModel : PersistenceModel
    {
        protected AutoMapper autoMapper;
        private Assembly assemblyContainingMaps;
        private Assembly entityAssembly;
        private Func<Type, bool> shouldIncludeType;
        private readonly List<AutoMapType> mappingTypes = new List<AutoMapType>();
        private bool autoMappingsCreated;
        private readonly AutoMappingAlterationCollection alterations = new AutoMappingAlterationCollection();

        /// <summary>
        /// Specify alterations to be used with this AutoPersisteceModel
        /// </summary>
        /// <param name="alterationDelegate">Lambda to declare alterations</param>
        /// <returns>AutoPersistenceModel</returns>
        public AutoPersistenceModel WithAlterations(Action<AutoMappingAlterationCollection> alterationDelegate)
        {
            alterationDelegate(alterations);
            return this;
        }

        /// <summary>
        /// Use auto mapping overrides defined in the assembly of T.
        /// </summary>
        /// <typeparam name="T">Type to get assembly from</typeparam>
        /// <returns>AutoPersistenceModel</returns>
        public AutoPersistenceModel UseOverridesFromAssemblyOf<T>()
        {
            alterations.Add(new AutoMappingOverrideAlteration(typeof(T).Assembly));
            return this;
        }

        public AutoPersistenceModel WithConvention(Action<ConventionOverrides> conventionAction)
        {
            conventionAction(Conventions);
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

        public void CompileMappings()
        {
            if (assemblyContainingMaps != null)
                addMappingsFromAssembly(assemblyContainingMaps);

            alterations.Apply(this);

            foreach (var type in entityAssembly.GetTypes())
            {
                if (shouldIncludeType != null)
                {
                    if (!shouldIncludeType.Invoke(type))
                        continue;
                }

                if (Conventions.IsBaseType(type) || type == typeof(object) || type.IsAbstract)
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

            autoMappingsCreated = true;
        }

        public override void Configure(NHibernate.Cfg.Configuration configuration)
        {
            if (!autoMappingsCreated)
                CompileMappings();

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
            AddMapping((IClassMap)mapping);
        }

        private Type GetTypeToMap(Type type)
        {
            return Conventions.IsBaseType(type.BaseType) || type.BaseType == typeof(object) ? type : type.BaseType;
        }

        private void MergeMap(Type type, object mapping)
        {
            Type typeToMap = GetTypeToMap(type);
            InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
                autoMapper, a => a.MergeMap<object>(null), new[] { mapping }, typeToMap);
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
            AddMapping(autoMapper.Map<T>(mappingTypes));
            return this;
        }

        public IClassMap FindMapping<T>()
        {
            return FindMapping(typeof(T));
        }

        public IClassMap FindMapping(Type type)
        {
            Func<Type, Type, bool> finder = (mappingType, expectedType) =>
            {
                if (mappingType.IsGenericType)
                {
                    // instance of a generic type (probably AutoMap<T>)
                    return mappingType.GetGenericArguments()[0] == expectedType;
                }
                else if (mappingType.BaseType.IsGenericType && mappingType.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>))
                {
                    // base type is a generic type of ClassMap<T>, so we've got a XXXMap instance
                    return mappingType.BaseType.GetGenericArguments()[0] == expectedType;
                }

                return false;
            };
 
            var mapping = _mappings.Find(t => finder(t.GetType(), type)) as IClassMap;

            if (mapping != null) return mapping;

            // standard AutoMap<T> not found for the type, so looking for one for it's base type.
            return _mappings.Find(t => finder(t.GetType(), type.BaseType)) as IClassMap;
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
