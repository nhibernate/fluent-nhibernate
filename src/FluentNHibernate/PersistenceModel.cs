using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public class PersistenceModel
    {
        protected List<IClassMap> _mappings = new List<IClassMap>();
        private readonly IConventionFinder conventionFinder;
        private bool conventionsApplied;
        private ConventionOverrides conventionOverrides;

        public PersistenceModel(IConventionFinder conventionFinder)
        {
            this.conventionFinder = conventionFinder;
            this.conventionFinder.AddAssembly(typeof(PersistenceModel).Assembly);
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

        public ConventionOverrides Conventions
        {
            get
            {
                if (conventionOverrides == null)
                    conventionOverrides = CreateConventions(conventionFinder);

                return conventionOverrides;
            }
        }

        protected virtual ConventionOverrides CreateConventions(IConventionFinder finder)
        {
            return new ConventionOverrides(finder);
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
            var visitor = new MappingVisitor(Conventions, configuration);

            ApplyMappings(visitor);
        }

        public void WriteMappingsTo(string folder)
        {
            var visitor = new DiagnosticMappingVisitor(folder, Conventions, null);

            ApplyMappings(visitor);
        }

        public void ApplyConventions()
        {
            if (conventionsApplied) return;

            var conventions = conventionFinder.Find<IAssemblyConvention>() ?? new List<IAssemblyConvention>();

            foreach (var convention in conventions)
            {
                if (convention.Accept(_mappings))
                    convention.Apply(_mappings, Conventions);
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

        public DiagnosticMappingVisitor(string folder, ConventionOverrides conventions, Configuration configuration) : base(conventions, configuration)
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