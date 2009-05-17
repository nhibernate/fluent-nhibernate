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
        private bool conventionsApplied;
        private IEnumerable<HibernateMapping> compiledMappings;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            mappings = new List<IMappingProvider>();
            visitors = new List<IMappingModelVisitor>();
            ConventionFinder = conventionFinder;

            visitors.Add(new ConventionVisitor(ConventionFinder));
        }

        public PersistenceModel()
            : this(new DefaultConventionFinder())
        {}

        private void AddDefaultConventions()
        {
            foreach (var foundType in from type in typeof(PersistenceModel).Assembly.GetTypes()
                                      where type.Namespace == typeof(TableNameConvention).Namespace && !type.IsAbstract
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

            foreach (var classMap in mappings)
            {
                var hbm = classMap.GetHibernateMapping();

                hbm.AddClass(classMap.GetClassMapping());

                hbms.Add(hbm);
            }

            return hbms;
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

            foreach (var mapping in compiledMappings)
            {
                ApplyVisitors(mapping);
            }
        }

        public void WriteMappingsTo(string folder)
        {
            EnsureMappingsBuilt();

            foreach (var mapping in compiledMappings)
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);

                using (var writer = new XmlTextWriter(Path.Combine(folder, mapping.Classes.First().Name + ".hbm.xml"), Encoding.Default))
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
}