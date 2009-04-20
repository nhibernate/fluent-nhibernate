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
using FluentNHibernate.Conventions.Defaults;
using FluentNHibernate.Conventions.Discovery;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;
using FluentNHibernate.Xml;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        protected readonly IList<IMappingProvider> _mappings;
        private readonly IList<IMappingModelVisitor> _visitors;
        public IConventionFinder ConventionFinder { get; private set; }
        private bool conventionsApplied;
        private HibernateMapping rootMapping;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            _mappings = new List<IMappingProvider>();
            _visitors = new List<IMappingModelVisitor>();
            ConventionFinder = conventionFinder;

            AddDiscoveryConventions();
        }

        public PersistenceModel()
            : this(new DefaultConventionFinder())
        {}

        private void AddDiscoveryConventions()
        {
            foreach (var foundType in from type in typeof(PersistenceModel).Assembly.GetTypes()
                                      where type.Namespace == typeof(ClassDiscoveryConvention).Namespace && !type.IsAbstract
                                      select type)
            {
                ConventionFinder.Add(foundType);
            }
        }

        private void AddDefaultConventions()
        {
            foreach (var foundType in from type in typeof(PersistenceModel).Assembly.GetTypes()
                                      where type.Namespace == typeof(TableNameConvention).Namespace && !type.IsAbstract
                                      select type)
            {
                ConventionFinder.Add(foundType);
            }
        }

        public PersistenceModel(IEnumerable<IMappingProvider> providers, IList<IMappingModelVisitor> visitors)
        {
            foreach(var mapping in providers)
                Add(mapping);
            _visitors = visitors;
        }

        protected void addMappingsFromThisAssembly()
        {
            Assembly assembly = findTheCallingAssembly();
            addMappingsFromAssembly(assembly);
        }

        public void addMappingsFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (!type.IsGenericType && typeof(IClassMap).IsAssignableFrom(type))
                    Add(type);
            }
        }

        private static Assembly findTheCallingAssembly()
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
            _mappings.Add(provider);
        }

        public void AddConvention(IMappingModelVisitor visitor)
        {
            _visitors.Add(visitor);
        }

        public void Add(Type type)
        {
            var mapping = (IMappingProvider)type.InstantiateUsingParameterlessConstructor();
            Add(mapping);
        }

        public IEnumerable<IMappingProvider> Mappings
        {
            get { return _mappings; }
        }

        public HibernateMapping BuildHibernateMapping()
        {
            ApplyConventions();

            var mapping = new HibernateMapping();
            mapping.DefaultLazy = false;

            foreach (var classMapping in _mappings)
                mapping.AddClass(classMapping.GetClassMapping());

            return mapping;
        }

        public void ApplyVisitors(HibernateMapping mapping)
        {
            foreach (var visitor in _visitors)
                mapping.AcceptVisitor(visitor);
        }

        public void ApplyConventions()
        {
            if (conventionsApplied) return;

            // we do this now so user conventions are applied first, then any defaults
            AddDefaultConventions();
            
            var conventions = ConventionFinder.Find<IEntireMappingsConvention>() ?? new List<IEntireMappingsConvention>();

            // HACK: get a list of IClassMap from a list of IMappingProviders
            var classes = new List<IClassMap>();
            
            foreach (var provider in _mappings.Where(x => x is IClassMap))
                classes.Add((IClassMap)provider);

            foreach (var convention in conventions)
            {
                if (convention.Accept(classes))
                    convention.Apply(classes);
            }

            conventionsApplied = true;
        }

        private void EnsureMappingBuilt()
        {
            if (rootMapping != null) return;

            rootMapping = BuildHibernateMapping();

            ApplyVisitors(rootMapping);
        }

        public void WriteMappingsTo(string folder)
        {
            EnsureMappingBuilt();

            var serializer = new MappingXmlSerializer();
            var document = serializer.Serialize(rootMapping);

            using (var writer = new XmlTextWriter(Path.Combine(folder, "Mappings.hbm.xml"), Encoding.Default))
            {
                document.WriteTo(writer);
            }
        }

        public virtual void Configure(Configuration cfg)
        {
            EnsureMappingBuilt();

            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(rootMapping);            

            cfg.AddDocument(document);
        }
    }

public class DiagnosticMappingVisitor : MappingVisitor
{
            private string _folder;

        public DiagnosticMappingVisitor(string folder, Configuration configuration) : base(configuration)
        {
            _folder = folder;            
        }

        public override void AddMappingDocument(XmlDocument document, Type type)
        {
            string filename = Path.Combine(_folder, type.FullName + ".hbm.xml");
            document.Save(filename);
        }
}

    public interface IMappingProvider
    {
        ClassMapping GetClassMapping();
    }
}