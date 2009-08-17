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
        /// Setup the auto mapper
        /// </summary>
        /// <param name="expressionsAction"></param>
        /// <returns></returns>
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
                    if (!shouldIncludeType.Invoke(type))
                        continue;
                }

                if (Expressions.IsBaseType(type) || type == typeof(object) || type.IsAbstract)
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
			while (!Expressions.IsBaseType(type.BaseType) &&
                !Expressions.IsConcreteBaseType(type.BaseType) &&
                type.BaseType != typeof(object) &&
                !type.BaseType.IsAbstract)
			{
				type = type.BaseType;
			}

			return type;
        }

        private void MergeMap(Type type, IMappingProvider mapping)
        {
            Type typeToMap = GetTypeToMap(type);
            autoMapper.MergeMap(typeToMap, mapping.GetClassMapping(), new List<string>(mapping.GetIgnoredProperties()));
        }


        //public AutoPersistenceModel AutoMap<T>()
        //{
        //    Add(new PassThroughMappingProvider(autoMapper.Map(typeof(T), mappingTypes)));
        //    return this;
        //}

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

        public AutoPersistenceModel AddEntityAssembly(Assembly assembly)
        {
            return AddTypeSource(new AssemblyTypeSource(assembly));
        }

        public AutoPersistenceModel AddTypeSource(ITypeSource source)
        {
            sources.Add(source);
            return this;
        }

        internal void AddOverride(Type type, Action<object> action)
        {
            inlineOverrides.Add(new InlineOverride(type, action));
        }

        public AutoPersistenceModel ForTypesThatDeriveFrom<T>(Action<AutoMapping<T>> populateMap)
        {
            inlineOverrides.Add(new InlineOverride(typeof(T), x =>
            {
                if (x is AutoMapping<T>)
                    populateMap((AutoMapping<T>)x);
            }));

            return this;
        }

        public AutoPersistenceModel ForAllTypes(Action<IPropertyIgnorer> alteration)
        {
            inlineOverrides.Add(new InlineOverride(typeof(object), x =>
            {
                if (x is IPropertyIgnorer)
                    alteration((IPropertyIgnorer)x);
            }));

            return this;
        }

        protected override string GetMappingFileName()
        {
            return "AutoMappings.hbm.xml";
        }
    }
}
