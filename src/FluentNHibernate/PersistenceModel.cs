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
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Xml;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        protected readonly IList<IMappingProvider> mappings;
        private readonly IList<IMappingModelVisitor> visitors;
        public IConventionFinder ConventionFinder { get; private set; }
        public bool MergeMappings { get; set; }
        private bool conventionsApplied;
        private IEnumerable<HibernateMapping> compiledMappings;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            mappings = new List<IMappingProvider>();
            visitors = new List<IMappingModelVisitor>();
            ConventionFinder = conventionFinder;

            visitors.Add(new ConventionVisitor(ConventionFinder));

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
                ConventionFinder.Add(foundType);
            }
        }

        protected void AddMappingsFromThisAssembly()
        {
            Assembly assembly = FindTheCallingAssembly();
            AddMappingsFromAssembly(assembly);
        }

        public void AddMappingsFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (!type.IsGenericType && typeof(IClassMap).IsAssignableFrom(type))
                    Add(type);
            }
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
            mappings.Add(provider);
        }

        public void Add(Type type)
        {
            var mapping = (IMappingProvider)type.InstantiateUsingParameterlessConstructor();
            Add(mapping);
        }

        public IEnumerable<IMappingProvider> Mappings
        {
            get { return mappings; }
        }

        public IEnumerable<HibernateMapping> BuildMappings()
        {
            var hbms = new List<HibernateMapping>();

            if (MergeMappings)
                BuildSingleMapping(hbms.Add);
            else
                BuildSeparateMappings(hbms.Add);

            foreach (var mapping in hbms)
                ApplyVisitors(mapping);

            return hbms;
        }

        private void BuildSeparateMappings(Action<HibernateMapping> add)
        {
            foreach (var classMap in mappings)
            {
                var hbm = classMap.GetHibernateMapping();

                hbm.AddClass(classMap.GetClassMapping());

                add(hbm);
            }
        }

        private void BuildSingleMapping(Action<HibernateMapping> add)
        {
            var hbm = new HibernateMapping();

            foreach (var classMap in mappings)
            {
                hbm.AddClass(classMap.GetClassMapping());
            }

            if (hbm.Classes.Count() > 0)
                add(hbm);
        }

        public void ApplyVisitors(HibernateMapping mapping)
        {
            foreach (var visitor in visitors)
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

    public class DiagnosticMappingVisitor : MappingVisitor
    {
        private readonly string folder;

        public DiagnosticMappingVisitor(string folder, Configuration configuration) : base(configuration)
        {
            this.folder = folder;            
        }

        public override void AddMappingDocument(XmlDocument document, Type type)
        {
            string filename = Path.Combine(folder, type.FullName + ".hbm.xml");
            document.Save(filename);
        }
    }

    public interface IMappingProvider
    {
        ClassMapping GetClassMapping();
        // HACK: In place just to keep compatibility until verdict is made
        HibernateMapping GetHibernateMapping();
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
    }
}