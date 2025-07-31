using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;

namespace FluentNHibernate.Cfg;

public class SetupConventionFinder<TReturn>(TReturn container, IConventionFinder conventionFinder) : IConventionFinder
{
    ConventionsCollection IConventionFinder.Conventions => conventionFinder.Conventions;

    public TReturn AddSource(ITypeSource source)
    {
        conventionFinder.AddSource(source);
        return container;
    }

    void IConventionFinder.AddSource(ITypeSource source)
    {
        AddSource(source);
    }

    public TReturn AddAssembly(Assembly assembly)
    {
        conventionFinder.AddAssembly(assembly);
        return container;
    }

    public TReturn AddFromAssemblyOf<T>()
    {
        conventionFinder.AddFromAssemblyOf<T>();
        return container;
    }

    void IConventionFinder.AddFromAssemblyOf<T>()
    {
        AddFromAssemblyOf<T>();
    }

    void IConventionFinder.AddAssembly(Assembly assembly)
    {
        AddAssembly(assembly);
    }

    public TReturn Add<T>() where T : IConvention
    {
        conventionFinder.Add<T>();
        return container;
    }

    void IConventionFinder.Add<T>()
    {
        Add<T>();
    }

    public void Add(Type type, object instance)
    {
        conventionFinder.Add(type, instance);
    }

    public TReturn Add<T>(T instance) where T : IConvention
    {
        conventionFinder.Add(instance);
        return container;
    }

    void IConventionFinder.Add(Type type)
    {
        Add(type);
    }

    public TReturn Add(Type type)
    {
        conventionFinder.Add(type);
        return container;
    }

    void IConventionFinder.Add<T>(T instance)
    {
        Add(instance);
    }

    public TReturn Add(params IConvention[] instances)
    {
        foreach (var instance in instances)
        {
            conventionFinder.Add(instance.GetType(), instance);
        }

        return container;
    }

    public TReturn Setup(Action<IConventionFinder> setupAction)
    {
        setupAction(this);
        return container;
    }

    public IEnumerable<T> Find<T>() where T : IConvention
    {
        return conventionFinder.Find<T>();
    }

    void IConventionFinder.SetLogger(IDiagnosticLogger logger)
    {
        conventionFinder.SetLogger(logger);
    }

    void IConventionFinder.Merge(IConventionFinder other)
    {
        conventionFinder.Merge(other);
    }
}
