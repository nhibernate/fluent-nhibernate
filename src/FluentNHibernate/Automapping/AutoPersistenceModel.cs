using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.Automapping
{
    public class AutoPersistenceModel : PersistenceModel
    {
        readonly IAutomappingConfiguration cfg;
        readonly AutoMappingExpressions expressions;
        readonly AutoMapper autoMapper;
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        Func<Type, bool> whereClause;
        readonly List<AutoMapType> mappingTypes = new List<AutoMapType>();
        bool autoMappingsCreated;
        readonly AutoMappingAlterationCollection alterations = new AutoMappingAlterationCollection();
        readonly List<InlineOverride> inlineOverrides = new List<InlineOverride>();
        readonly List<Type> ignoredTypes = new List<Type>();
        readonly List<Type> includedTypes = new List<Type>();

        public AutoPersistenceModel()
        {
            expressions = new AutoMappingExpressions();
            cfg = new ExpressionBasedAutomappingConfiguration(expressions);
            autoMapper = new AutoMapper(cfg, Conventions, inlineOverrides);

            componentResolvers.Add(new AutomappedComponentResolver(autoMapper, cfg));
        }

        public AutoPersistenceModel(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            autoMapper = new AutoMapper(cfg, Conventions, inlineOverrides);

            componentResolvers.Add(new AutomappedComponentResolver(autoMapper, cfg));
        }

        public AutoPersistenceModel AddMappingsFromAssemblyOf<T>()
        {
            return AddMappingsFromAssembly(typeof(T).Assembly);
        }

        public new AutoPersistenceModel AddMappingsFromAssembly(Assembly assembly)
        {
            AddMappingsFromSource(new AssemblyTypeSource(assembly));
            return this;
        }

        public new AutoPersistenceModel AddMappingsFromSource(ITypeSource source)
        {
            base.AddMappingsFromSource(source);
            return this;
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
            return UseOverridesFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Use auto mapping overrides defined in the assembly of T.
        /// </summary>
        /// <param name="assembly">Assembly to scan</param>
        /// <returns>AutoPersistenceModel</returns>
        public AutoPersistenceModel UseOverridesFromAssembly(Assembly assembly)
        {
            alterations.Add(new AutoMappingOverrideAlteration(assembly));
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
        /// Alter some of the configuration options that control how the automapper works.
        /// Depreciated in favour of supplying your own IAutomappingConfiguration instance to AutoMap: <see cref="AutoMap.AssemblyOf{T}(FluentNHibernate.Automapping.IAutomappingConfiguration)"/>.
        /// Cannot be used in combination with a user-defined configuration.
        /// </summary>
        [Obsolete("Depreciated in favour of supplying your own IAutomappingConfiguration instance to AutoMap: AutoMap.AssemblyOf<T>(your_configuration_instance)")]
        public AutoPersistenceModel Setup(Action<AutoMappingExpressions> expressionsAction)
        {
            if (HasUserDefinedConfiguration)
                throw new InvalidOperationException("Cannot use Setup method when using a user-defined IAutomappingConfiguration instance.");

            expressionsAction(expressions);
            return this;
        }

        /// <summary>
        /// Supply a criteria for which types will be mapped.
        /// Cannot be used in combination with a user-defined configuration.
        /// </summary>
        /// <param name="where">Where clause</param>
        public AutoPersistenceModel Where(Func<Type, bool> where)
        {
            if (HasUserDefinedConfiguration)
                throw new InvalidOperationException("Cannot use Where method when using a user-defined IAutomappingConfiguration instance.");

            whereClause = where;
            return this;
        }

        public override IEnumerable<HibernateMapping> BuildMappings()
        {
            CompileMappings();

            return base.BuildMappings();
        }

        private void CompileMappings()
        {
            if (autoMappingsCreated)
                return;

            alterations.Apply(this);

            var types = sources
                .SelectMany(x => x.GetTypes())
                .OrderBy(x => InheritanceHierarchyDepth(x));

            foreach (var type in types)
            {
                // skipped by user-defined configuration criteria
                if (!cfg.ShouldMap(type))
                {
                    log.AutomappingSkippedType(type, "Skipped by result of IAutomappingConfiguration.ShouldMap(Type)");
                    continue;
                }
                // skipped by inline where clause
                if (whereClause != null && !whereClause(type))
                {
                    log.AutomappingSkippedType(type, "Skipped by Where clause");
                    continue;
                }
                // skipped because either already mapped elsewhere, or not valid for mapping            
                if (!ShouldMap(type))
                    continue;

                mappingTypes.Add(new AutoMapType(type));
            }

            log.AutomappingCandidateTypes(mappingTypes.Select(x => x.Type));

            foreach (var type in mappingTypes)
            {
                if (type.IsMapped) continue;

                AddMapping(type.Type);
            }

            autoMappingsCreated = true;
        }

        private int InheritanceHierarchyDepth(Type type)
        {
            var depth = 0;
            var parent = type;

            while (parent != null && parent != typeof(object))
            {
                parent = parent.BaseType;
                depth++;
            }

            return depth;
        }

        public override void Configure(NHibernate.Cfg.Configuration configuration)
        {
            CompileMappings();

            base.Configure(configuration);
        }

        private void AddMapping(Type type)
        {
            Type typeToMap = GetTypeToMap(type);

            // Fixes https://github.com/jagregory/fluent-nhibernate/issues/113,
            // where 'type' would not be mapped if 'GetTypeToMap' returned the
            // base type
            if (typeToMap != type)
            {
                log.BeginAutomappingType(type);
                var derivedMapping = autoMapper.Map(type, mappingTypes);

                Add(new PassThroughMappingProvider(derivedMapping));
            }

            log.BeginAutomappingType(typeToMap);
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
            return ShouldMap(type.BaseType) && !cfg.IsConcreteBaseType(type.BaseType);
        }

        private bool ShouldMap(Type type)
        {
            if (includedTypes.Contains(type))
                return true; // inclusions take precedence over everything
            if (ignoredTypes.Contains(type))
            {
                log.AutomappingSkippedType(type, "Skipped by IgnoreBase");
                return false; // excluded
            }
            if (type.IsGenericType && ignoredTypes.Contains(type.GetGenericTypeDefinition()))
            {
                log.AutomappingSkippedType(type, "Skipped by IgnoreBase");
                return false; // generic definition is excluded
            }
            if (type.IsAbstract && cfg.AbstractClassIsLayerSupertype(type))
            {
                log.AutomappingSkippedType(type, "Skipped by IAutomappingConfiguration.AbstractClassIsLayerSupertype(Type)");
                return false; // is abstract and a layer supertype
            }
            if (cfg.IsComponent(type))
            {
                log.AutomappingSkippedType(type, "Skipped by IAutomappingConfiguration.IsComponent(Type)");
                return false; // skipped because we don't want to map components as entities
            }
            if (type == typeof(object))
                return false; // object!

            return true;
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

			if (type.BaseType != typeof(object) && !cfg.IsConcreteBaseType(type.BaseType))
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

		public AutoPersistenceModel AddFilter<TFilter>() where TFilter : IFilterDefinition
		{
    		Add(typeof(TFilter));
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
		/// Adds an IAutoMappingOverride reflectively
		/// </summary>
		/// <param name="overrideType">Override type, expected to be an IAutoMappingOverride</param>
		public void Override(Type overrideType)
		{
			Type overrideInterface = overrideType.GetInterfaces()
				.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IAutoMappingOverride<>) && x.GetGenericArguments().Length > 0)
				.FirstOrDefault();
			if (overrideInterface != null)
			{
				Type entityType = overrideInterface.GetGenericArguments().First();
				Type autoMappingType = typeof(AutoMapping<>).MakeGenericType(entityType);
				AddOverride(entityType, x =>
				{
					if (x.GetType().IsAssignableFrom(autoMappingType))
					{
						var overrideInstance = Activator.CreateInstance(overrideType);

						MethodInfo overrideHelperMethod = typeof(AutoPersistenceModel)
							.GetMethod("OverrideHelper", BindingFlags.NonPublic | BindingFlags.Instance);
						
						if (overrideHelperMethod != null)
						{
							overrideHelperMethod
								.MakeGenericMethod(entityType)
								.Invoke(this, new[] {x, overrideInstance});
						}
					}
				});
			}
		}

		//called reflectively from method above
		private void OverrideHelper<T>(AutoMapping<T> x, IAutoMappingOverride<T> mappingOverride)
		{
			mappingOverride.Override(x);
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
        /// <see cref="IAutomappingConfiguration.AbstractClassIsLayerSupertype"/> setting.
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

        bool HasUserDefinedConfiguration
        {
            get { return !(cfg is ExpressionBasedAutomappingConfiguration); }
        }
    }

    public class AutomappedComponentResolver : IComponentReferenceResolver
    {
        readonly AutoMapper mapper;
        IAutomappingConfiguration cfg;

        public AutomappedComponentResolver(AutoMapper mapper, IAutomappingConfiguration cfg)
        {
            this.mapper = mapper;
            this.cfg = cfg;
        }

        public ExternalComponentMapping Resolve(ComponentResolutionContext context, IEnumerable<IExternalComponentMappingProvider> componentProviders)
        {
            // this will only be called if there was no ComponentMap found
            var mapping = new ExternalComponentMapping(ComponentType.Component)
            {
                Member = context.ComponentMember,
                ContainingEntityType = context.EntityType,
            };
            mapping.Set(x => x.Name, Layer.Defaults, context.ComponentMember.Name);
            mapping.Set(x => x.Type, Layer.Defaults, context.ComponentType);

            if (context.ComponentMember.IsProperty && !context.ComponentMember.CanWrite)
                mapping.Set(x => x.Access, Layer.Defaults, cfg.GetAccessStrategyForReadOnlyProperty(context.ComponentMember).ToString());

            mapper.FlagAsMapped(context.ComponentType);
            mapper.MergeMap(context.ComponentType, mapping, new List<Member>());

            return mapping;
        }
    }
}
