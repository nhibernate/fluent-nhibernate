using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentNHibernate.Conventions
{
    public interface IConventionFinder
    {
        void AddAssembly(Assembly assembly);
        void Add<T>() where T : IConvention;
        void Add<T>(T instance) where T : IConvention;
        IEnumerable<T> Find<T>() where T : IConvention;
    }
}