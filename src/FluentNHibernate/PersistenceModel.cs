using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate;

public class PersistenceModel
{
    protected readonly IList<IMappingProvider> classProviders = new List<IMappingProvider>();
    protected readonly IList<IFilterDefinition> filterDefinitions = new List<IFilterDefinition>();
    protected readonly IIndeterminateSubclassMappingProviderCollection subclassProviders = new IndeterminateSubclassMappingProviderCollection();
    protected readonly IList<IExternalComponentMappingProvider> componentProviders = new List<IExternalComponentMappingProvider>();
    protected readonly IList<IComponentReferenceResolver> componentResolvers = new List<IComponentReferenceResolver>
    {
        new ComponentMapComponentReferenceResolver()
    };
    private readonly IList<IMappingModelVisitor> visitors = new List<IMappingModelVisitor>();
    public IConventionFinder Conventions { get; private set; }
    public bool MergeMappings { get; set; }
    private IEnumerable<HibernateMapping> compiledMappings;
    private ValidationVisitor validationVisitor;
    public PairBiDirectionalManyToManySidesDelegate BiDirectionalManyToManyPairer { get; set; }

    IDiagnosticMessageDispatcher diagnosticDispatcher = new DefaultDiagnosticMessageDispatcher();
    protected IDiagnosticLogger log = new NullDiagnosticsLogger();

    public PersistenceModel(IConventionFinder conventionFinder)
    {
        BiDirectionalManyToManyPairer = (c,o,w) => {};
        Conventions = conventionFinder;

        visitors.Add(new SeparateSubclassVisitor(subclassProviders));
        visitors.Add(new ComponentReferenceResolutionVisitor(componentResolvers, componentProviders));
        visitors.Add(new RelationshipPairingVisitor(BiDirectionalManyToManyPairer));
        visitors.Add(new ManyToManyTableNameVisitor());
        visitors.Add(new ConventionVisitor(Conventions));
        visitors.Add(new ComponentColumnPrefixVisitor());
        visitors.Add(new RelationshipKeyPairingVisitor());
        visitors.Add((validationVisitor = new ValidationVisitor()));
    }

    public PersistenceModel(): this(new DefaultConventionFinder())
    {}

    public void SetLogger(IDiagnosticLogger logger)
    {
        log = logger;
        Conventions.SetLogger(logger);
    }

    protected void AddMappingsFromThisAssembly()
    {
        var assembly = FindTheCallingAssembly();
        AddMappingsFromAssembly(assembly);
    }

    public void AddMappingsFromAssembly(Assembly assembly)
    {
        AddMappingsFromSource(new AssemblyTypeSource(assembly));
    }

    public void AddMappingsFromSource(ITypeSource source)
    {
        source.GetTypes()
            .Where(x => IsMappingOf<IMappingProvider>(x) ||
                        IsMappingOf<IIndeterminateSubclassMappingProvider>(x) ||
                        IsMappingOf<IExternalComponentMappingProvider>(x) ||
                        IsMappingOf<IFilterDefinition>(x))
            .Each(Add);

        log.LoadedFluentMappingsFromSource(source);
    }

    private static Assembly FindTheCallingAssembly()
    {
        StackTrace trace = new StackTrace(false);

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

    public void Add(IFilterDefinition definition)
    {
        filterDefinitions.Add(definition);
    }

    public void Add(IExternalComponentMappingProvider provider)
    {
        componentProviders.Add(provider);
    }

    public void Add(Type type)
    {
        var mapping = type.InstantiateUsingParameterlessConstructor();

        if (mapping is IMappingProvider)
        {
            log.FluentMappingDiscovered(type);
            Add((IMappingProvider)mapping);
        }
        else if (mapping is IIndeterminateSubclassMappingProvider)
        {
            log.FluentMappingDiscovered(type);
            Add((IIndeterminateSubclassMappingProvider)mapping);
        }
        else if (mapping is IFilterDefinition)
            Add((IFilterDefinition)mapping);
        else if (mapping is IExternalComponentMappingProvider)
        {
            log.FluentMappingDiscovered(type);
            Add((IExternalComponentMappingProvider)mapping);
        }
        else
            throw new InvalidOperationException("Unsupported mapping type '" + type.FullName + "'");
    }

    private bool IsMappingOf<T>(Type type)
    {
        return !type.IsGenericType && typeof(T).IsAssignableFrom(type);
    }

    public virtual IEnumerable<HibernateMapping> BuildMappings()
    {
        var hbms = new List<HibernateMapping>();

        if (MergeMappings)
            BuildSingleMapping(hbms.Add);
        else
            BuildSeparateMappings(hbms.Add);

        ApplyVisitors(hbms);

        log.Flush();

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

        foreach (var filterDefinition in filterDefinitions)
        {
            var hbm = filterDefinition.GetHibernateMapping();
            hbm.AddFilter(filterDefinition.GetFilterMapping());
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
        foreach (var filterDefinition in filterDefinitions)
        {
            hbm.AddFilter(filterDefinition.GetFilterMapping());
        }

        if (hbm.Classes.Any())
            add(hbm);
    }

    private void ApplyVisitors(IEnumerable<HibernateMapping> mappings)
    {
        foreach (var visitor in visitors)
            visitor.Visit(mappings);
    }

    private void EnsureMappingsBuilt()
    {
        if (compiledMappings is not null) return;

        compiledMappings = BuildMappings();
    }

    protected virtual string GetMappingFileName()
    {
        return "FluentMappings.hbm.xml";
    }

    private string DetermineMappingFileName(HibernateMapping mapping)
    {
        if (MergeMappings)
            return GetMappingFileName();

        if (mapping.Classes.Any())
            return mapping.Classes.First().Type.FullName + ".hbm.xml";

        return "filter-def." + mapping.Filters.First().Name + ".hbm.xml";
    }

    public void WriteMappingsTo(string folder)
    {
        WriteMappingsTo(mapping => new XmlTextWriter(Path.Combine(folder, DetermineMappingFileName(mapping)), Encoding.Default), true);
    }

    public void WriteMappingsTo(TextWriter writer)
    {
        WriteMappingsTo( _ => new XmlTextWriter(writer), false);
    }

    private void WriteMappingsTo(Func<HibernateMapping, XmlTextWriter> writerBuilder, bool shouldDispose)
    {
        EnsureMappingsBuilt();

        foreach (var mapping in compiledMappings)
        {
            var serializer = new MappingXmlSerializer();
            var document = serializer.Serialize(mapping);

            XmlTextWriter xmlWriter = null;

            try
            {
                xmlWriter = writerBuilder(mapping);
                xmlWriter.Formatting = Formatting.Indented;
                document.WriteTo(xmlWriter);
            }
            finally
            {
                if(shouldDispose && xmlWriter is not null)
                    xmlWriter.Close();
            }
        }
    }

    public virtual void Configure(Configuration cfg)
    {
        EnsureMappingsBuilt();

        foreach (var mapping in compiledMappings.Where(m => !m.Classes.Any()))
        {
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);
            cfg.AddDocument(document);
        }

        foreach (var mapping in compiledMappings.Where(m => m.Classes.Any()))
        {
            var serializer = new MappingXmlSerializer();
            XmlDocument document = serializer.Serialize(mapping);

            if (cfg.GetClassMapping(mapping.Classes.First().Type) is null)
                cfg.AddDocument(document);
        }
    }

    public bool ContainsMapping(Type type)
    {
        return classProviders.Any(x => x.GetType() == type) ||
               filterDefinitions.Any(x => x.GetType() == type) ||
               subclassProviders.Any(x => x.GetType() == type) ||
               componentProviders.Any(x => x.GetType() == type);
    }

    /// <summary>
    /// Gets or sets whether validation of mappings is performed.
    /// </summary>
    public bool ValidationEnabled
    {
        get { return validationVisitor.Enabled; }
        set { validationVisitor.Enabled = value; }
    }

    internal void ImportProviders(PersistenceModel model)
    {
        model.classProviders.Each(x =>
        {
            if (!classProviders.Contains(x))
                classProviders.Add(x);
        });

        model.subclassProviders.Each(x =>
        {
            if (!subclassProviders.Contains(x))
                subclassProviders.Add(x);
        });

        model.componentProviders.Each(x =>
        {
            if (!componentProviders.Contains(x))
                componentProviders.Add(x);
        });
    }
}

public interface IMappingProvider
{
    ClassMapping GetClassMapping();
    // HACK: In place just to keep compatibility until verdict is made
    HibernateMapping GetHibernateMapping();
    IEnumerable<Member> GetIgnoredProperties();
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

    public IEnumerable<Member> GetIgnoredProperties()
    {
        return Array.Empty<Member>();
    }
}
