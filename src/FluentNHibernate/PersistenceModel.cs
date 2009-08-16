using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        protected readonly IList<IMappingProvider> classProviders = new List<IMappingProvider>();
        protected readonly IList<IIndeterminateSubclassMappingProvider> subclassProviders = new List<IIndeterminateSubclassMappingProvider>();
        private readonly IList<IMappingModelVisitor> visitors = new List<IMappingModelVisitor>();
        public IConventionFinder Conventions { get; private set; }
        public bool MergeMappings { get; set; }
        private IEnumerable<HibernateMapping> compiledMappings;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            Conventions = conventionFinder;

            visitors.Add(new SeparateSubclassVisitor(subclassProviders));
            visitors.Add(new BiDirectionalManyToManyPairingVisitor());
            visitors.Add(new ManyToManyTableNameVisitor());
            visitors.Add(new ConventionVisitor(Conventions));

            AddDefaultConventions();
        }

        public PersistenceModel()
            : this(new DefaultConventionFinder())
        {}

        private void AddDefaultConventions()
        {
            foreach (var foundType in from type in typeof(PersistenceModel).Assembly.GetTypes()
                                      where type.Namespace == "FluentNHibernate.Conventions.Defaults" && !type.IsAbstract
                                      select type)
            {
                Conventions.Add(foundType);
            }
        }

        protected void AddMappingsFromThisAssembly()
        {
            Assembly assembly = FindTheCallingAssembly();
            AddMappingsFromAssembly(assembly);
        }

        public void AddMappingsFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes().Where(x => IsClassMap(x) || IsSubclassMap(x)))
                Add(type);
        }

        private static Assembly FindTheCallingAssembly()
        {
            StackTrace trace = new StackTrace(Thread.CurrentThread, false);

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                Assembly assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != thisAssembly)
                {
                    callingAssembly = assembly;
                    break;
                }
            }
            return callingAssembly;
        }

        public void Add(IMappingProvider provider)
        {
            classProviders.Add(provider);
        }

        public void Add(IIndeterminateSubclassMappingProvider provider)
        {
            subclassProviders.Add(provider);
        }

        public void Add(Type type)
        {
            var mapping = type.InstantiateUsingParameterlessConstructor();

            if (IsClassMap(type))
                Add((IMappingProvider)mapping);
            else if (IsSubclassMap(type))
                Add((IIndeterminateSubclassMappingProvider)mapping);
            else
                throw new InvalidOperationException("Unsupported mapping type '" + type.FullName + "'");
        }

        private bool IsClassMap(Type type)
        {
            return !type.IsGenericType && typeof(IMappingProvider).IsAssignableFrom(type);
        }

        private bool IsSubclassMap(Type type)
        {
            return !type.IsGenericType && typeof(IIndeterminateSubclassMappingProvider).IsAssignableFrom(type);
        }

        public IEnumerable<HibernateMapping> BuildMappings()
        {
            var hbms = new List<HibernateMapping>();

            if (MergeMappings)
                BuildSingleMapping(hbms.Add);
            else
                BuildSeparateMappings(hbms.Add);

            ApplyVisitors(hbms);

            return hbms;
        }

        private void BuildSeparateMappings(Action<HibernateMapping> add)
        {
            foreach (var classMap in classProviders)
            {
                var hbm = classMap.GetHibernateMapping();

                hbm.AddClass(classMap.GetClassMapping());

                add(hbm);
            }
        }

        private void BuildSingleMapping(Action<HibernateMapping> add)
        {
            var hbm = new HibernateMapping();

            foreach (var classMap in classProviders)
            {
                hbm.AddClass(classMap.GetClassMapping());
            }

            if (hbm.Classes.Count() > 0)
                add(hbm);
        }

        private void ApplyVisitors(IEnumerable<HibernateMapping> mappings)
        {
            foreach (var visitor in visitors)
                foreach (var mapping in mappings)
                    mapping.AcceptVisitor(visitor);
        }

        private void EnsureMappingsBuilt()
        {
            if (compiledMappings != null) return;

            compiledMappings = BuildMappings();
        }

        protected virtual string GetMappingFileName()
        {
            return "FluentMappings.hbm.xml";
        }

        public void WriteMappingsTo(string folder)
        {
            EnsureMappingsBuilt();

            foreach (var mapping in compiledMappings)
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);
                var filename = MergeMappings ? GetMappingFileName() : mapping.Classes.First().Type.FullName + ".hbm.xml";

                using (var writer = new XmlTextWriter(Path.Combine(folder, filename), Encoding.Default))
                {
                    writer.Formatting = Formatting.Indented;
                    document.WriteTo(writer);
                }    
            }
        }

        public virtual void Configure(Configuration cfg)
        {
            EnsureMappingsBuilt();

            foreach (var mapping in compiledMappings)
            {
                var serializer = new MappingXmlSerializer();
                XmlDocument document = serializer.Serialize(mapping);

                if (cfg.GetClassMapping(mapping.Classes.First().Type) == null)
                    cfg.AddDocument(document);
            }
        }
    }

    public interface IMappingProvider
    {
        ClassMapping GetClassMapping();
        // HACK: In place just to keep compatibility until verdict is made
        HibernateMapping GetHibernateMapping();
        IEnumerable<string> GetIgnoredProperties();
    }

    public class PassThroughMappingProvider : IMappingProvider
    {
        private readonly ClassMapping mapping;

        public PassThroughMappingProvider(ClassMapping mapping)
        {
            this.mapping = mapping;
        }

        public ClassMapping GetClassMapping()
        {
            return mapping;
        }

        public HibernateMapping GetHibernateMapping()
        {
            return new HibernateMapping();
        }

        public IEnumerable<string> GetIgnoredProperties()
        {
            return new string[0];
        }
    }
}