using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    public class AutoPersistenceModel : PersistenceModel
    {
        protected AutoMapper autoMapper;
        private readonly List<ITypeSource> sources = new List<ITypeSource>();
        private Func<Type, bool> shouldIncludeType;
        private readonly List<AutoMapType> mappingTypes = new List<AutoMapType>();
        private bool autoMappingsCreated;
        private readonly AutoMappingAlterationCollection alterations = new AutoMappingAlterationCollection();
        protected readonly List<InlineOverride> inlineOverrides = new List<InlineOverride>();
        private readonly List<Type> ignoredTypes = new List<Type>();
        private readonly List<Type> includedTypes = new List<Type>();

        public AutoPersistenceModel()
        {
            Expressions = new AutoMappingExpressions();
            autoMapper = new AutoMapper(Expressions, Conventions, inlineOverrides);
        }

        public AutoPersistenceModel(AutoMapper customAutomapper)
        {
            Expressions = new AutoMappingExpressions();
            autoMapper = customAutomapper;
        }

        /// <summary>
        /// Specify alterations to be used with this AutoPersisteceModel
        /// </summary>
        /// <param name="alterationDelegate">Lambda to declare alterations</param>
        /// <returns>AutoPersistenceModel</returns>
        public AutoPersistenceModel Alterations(Action<AutoMappingAlterationCollection> alterationDelegate)
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

        /// <summary>
        /// Alter convention discovery
        /// </summary>
        public new SetupConventionFinder<AutoPersistenceModel> Conventions
        {
            get { return new SetupConventionFinder<AutoPersistenceModel>(this, base.Conventions); }
        }

        /// <summary>
        /// Alter some of the configuration options that control how the automapper works
        /// </summary>
        public AutoPersistenceModel Setup(Action<AutoMappingExpressions> expressionsAction)
        {
            expressionsAction(Expressions);
            return this;
        }

        internal AutoMappingExpressions Expressions { get; private set; }

        public AutoPersistenceModel Where(Func<Type, bool> where)
        {
            this.shouldIncludeType = where;
            return this;
        }

        public void CompileMappings()
        {
            if (autoMappingsCreated)
                return;

            alterations.Apply(this);

            foreach (var type in sources.SelectMany(x => x.GetTypes()))
            {
                if (shouldIncludeType != null)
                {
                    if (!shouldIncludeType(type))
                        continue;
                }

                if (!ShouldMap(type))
                    continue;

                mappingTypes.Add(new AutoMapType(type));
            }

            foreach (var type in mappingTypes)
            {
                if (type.Type.IsClass && IsNotAnonymousMethodClass(type))
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
            CompileMappings();

            base.Configure(configuration);
        }

        private static bool IsNotAnonymousMethodClass(AutoMapType type)
        {
            return type.Type.ReflectedType == null;
        }

        private void AddMapping(Type type)
        {
            Type typeToMap = GetTypeToMap(type);
            var mapping = autoMapper.Map(typeToMap, mappingTypes);

            Add(new PassThroughMappingProvider(mapping));
        }

        private Type GetTypeToMap(Type type)
        {
            while (ShouldMapParent(type))
			{
				type = type.BaseType;
			}

			return type;
        }

        private bool ShouldMapParent(Type type)
        {
            return ShouldMap(type.BaseType) && !Expressions.IsConcreteBaseType(type.BaseType);
        }

        private bool ShouldMap(Type type)
        {
            if (includedTypes.Contains(type))
                return true; // inclusions take precedence over everything
            if (ignoredTypes.Contains(type))
                return false; // excluded
            if (type.IsGenericType && ignoredTypes.Contains(type.GetGenericTypeDefinition()))
                return false; // generic definition is excluded
            if (type.IsAbstract && Expressions.AbstractClassIsLayerSupertype(type))
                return false; // is abstract and a layer supertype
            if (Expressions.IsBaseType(type))
                return false; // excluded
            if (type == typeof(object))
                return false; // object!

            return true;
        }

        private void MergeMap(Type type, IMappingProvider mapping)
        {
            Type typeToMap = GetTypeToMap(type);
            autoMapper.MergeMap(typeToMap, mapping.GetClassMapping(), new List<string>(mapping.GetIgnoredProperties()));
        }

        public IMappingProvider FindMapping<T>()
        {
            return FindMapping(typeof(T));
        }

        public IMappingProvider FindMapping(Type type)
        {
            Func<IMappingProvider, Type, bool> finder = (provider, expectedType) =>
            {
                var mappingType = provider.GetType();
                if (mappingType.IsGenericType)
                {
                    // instance of a generic type (probably AutoMapping<T>)
                    return mappingType.GetGenericArguments()[0] == expectedType;
                }
                if (mappingType.BaseType.IsGenericType &&
                    mappingType.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>))
                {
                    // base type is a generic type of ClassMap<T>, so we've got a XXXMap instance
                    return mappingType.BaseType.GetGenericArguments()[0] == expectedType;
                }
                if (provider is PassThroughMappingProvider)
                    return provider.GetClassMapping().Type == expectedType;

                return false;
            };

            var mapping = classProviders.FirstOrDefault(t => finder(t, type));

            if (mapping != null) return mapping;

            // if we haven't found a map yet then try to find a map of the
            // base type to merge if not a concrete base type

			if (type.BaseType != typeof(object) && !Expressions.IsConcreteBaseType(type.BaseType))
			{
				return FindMapping(type.BaseType);
			}

			return null;
        }

        /// <summary>
        /// Adds all entities from a specific assembly.
        /// </summary>
        /// <param name="assembly">Assembly to load from</param>
        public AutoPersistenceModel AddEntityAssembly(Assembly assembly)
        {
            return AddTypeSource(new AssemblyTypeSource(assembly));
        }

        /// <summary>
        /// Adds all entities from the <see cref="ITypeSource"/>.
        /// </summary>
        /// <param name="source"><see cref="ITypeSource"/> to load from</param>
        public AutoPersistenceModel AddTypeSource(ITypeSource source)
        {
            sources.Add(source);
            return this;
        }

        internal void AddOverride(Type type, Action<object> action)
        {
            inlineOverrides.Add(new InlineOverride(type, action));
        }

        /// <summary>
        /// Override the mapping of a specific entity.
        /// </summary>
        /// <remarks>This may affect subclasses, depending on the alterations you do.</remarks>
        /// <typeparam name="T">Entity who's mapping to override</typeparam>
        /// <param name="populateMap">Lambda performing alterations</param>
        public AutoPersistenceModel Override<T>(Action<AutoMapping<T>> populateMap)
        {
            inlineOverrides.Add(new InlineOverride(typeof(T), x =>
            {
                if (x is AutoMapping<T>)
                    populateMap((AutoMapping<T>)x);
            }));

            return this;
        }

        /// <summary>
        /// Override all mappings.
        /// </summary>
        /// <remarks>Currently only supports ignoring properties on all entities.</remarks>
        /// <param name="alteration">Lambda performing alterations</param>
        public AutoPersistenceModel OverrideAll(Action<IPropertyIgnorer> alteration)
        {
            inlineOverrides.Add(new InlineOverride(typeof(object), x =>
            {
                if (x is IPropertyIgnorer)
                    alteration((IPropertyIgnorer)x);
            }));

            return this;
        }

        /// <summary>
        /// Ignore a base type. This removes it from any mapped inheritance hierarchies, good for non-abstract layer
        /// supertypes.
        /// </summary>
        /// <typeparam name="T">Type to ignore</typeparam>
        public AutoPersistenceModel IgnoreBase<T>()
        {
            return IgnoreBase(typeof(T));
        }

        /// <summary>
        /// Ignore a base type. This removes it from any mapped inheritance hierarchies, good for non-abstract layer
        /// supertypes.
        /// </summary>
        /// <param name="baseType">Type to ignore</param>
        public AutoPersistenceModel IgnoreBase(Type baseType)
        {
            ignoredTypes.Add(baseType);
            return this;
        }

        /// <summary>
        /// Explicitly includes a type to be used as part of a mapped inheritance hierarchy.
        /// </summary>
        /// <remarks>
        /// Abstract classes are probably what you'll be using this method with. Fluent NHibernate considers abstract
        /// classes to be layer supertypes, so doesn't automatically map them as part of an inheritance hierarchy. You
        /// can use this method to override that behavior for a specific type; otherwise you should consider using the
        /// <see cref="AutoMappingExpressions.AbstractClassIsLayerSupertype"/> setting.
        /// </remarks>
        /// <typeparam name="T">Type to include</typeparam>
        public AutoPersistenceModel IncludeBase<T>()
        {
            return IncludeBase(typeof(T));
        }

        /// <summary>
        /// Explicitly includes a type to be used as part of a mapped inheritance hierarchy.
        /// </summary>
        /// <remarks>
        /// Abstract classes are probably what you'll be using this method with. Fluent NHibernate considers abstract
        /// classes to be layer supertypes, so doesn't automatically map them as part of an inheritance hierarchy. You
        /// can use this method to override that behavior for a specific type; otherwise you should consider using the
        /// <see cref="AutoMappingExpressions.AbstractClassIsLayerSupertype"/> setting.
        /// </remarks>
        /// <param name="baseType">Type to include</param>
        public AutoPersistenceModel IncludeBase(Type baseType)
        {
            includedTypes.Add(baseType);
            return this;
        }

        protected override string GetMappingFileName()
        {
            return "AutoMappings.hbm.xml";
        }
    }
}
