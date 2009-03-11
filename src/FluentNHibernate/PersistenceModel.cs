using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        protected List<IClassMap> _mappings = new List<IClassMap>();
        public IConventionFinder ConventionFinder { get; private set; }
        private bool conventionsApplied;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            ConventionFinder = conventionFinder;
            ConventionFinder.AddAssembly(typeof(PersistenceModel).Assembly);
        }

        public PersistenceModel()
            : this(new DefaultConventionFinder())
        {}

        public void ForEach<T>(Action<T> action) where T : class
        {
            foreach (var mapping in _mappings)
            {
                T t = mapping as T;
                if (t != null) action(t);
            }
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
                {
                    var mapping = (IClassMap)type.InstantiateUsingParameterlessConstructor();
                    AddMapping(mapping);
                }
            }
        }

        public void AddMapping(IClassMap mapping)
        {
            _mappings.Add(mapping);
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

        public virtual void Configure(Configuration configuration)
        {
            var visitor = new MappingVisitor(configuration);

            ApplyMappings(visitor);
        }

        public void WriteMappingsTo(string folder)
        {
            var visitor = new DiagnosticMappingVisitor(folder, null);

            ApplyMappings(visitor);
        }

        public void ApplyConventions()
        {
            if (conventionsApplied) return;

            var conventions = ConventionFinder.Find<IAssemblyConvention>() ?? new List<IAssemblyConvention>();

            foreach (var convention in conventions)
            {
                if (convention.Accept(_mappings))
                    convention.Apply(_mappings);
            }

            conventionsApplied = true;
        }

        private void ApplyMappings(IMappingVisitor visitor)
        {
            ApplyConventions();

            _mappings.ForEach(mapping => mapping.ApplyMappings(visitor));
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
}