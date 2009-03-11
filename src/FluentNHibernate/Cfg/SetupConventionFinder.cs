using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Cfg
{
    public class SetupConventionFinder : IConventionFinder
    {
        private readonly FluentMappingsContainer container;
        private readonly IConventionFinder conventionFinder;

        public SetupConventionFinder(FluentMappingsContainer container, IConventionFinder conventionFinder)
        {
            this.container = container;
            this.conventionFinder = conventionFinder;
        }

        public FluentMappingsContainer AddAssembly(Assembly assembly)
        {
            conventionFinder.AddAssembly(assembly);
            return container;
        }

        void IConventionFinder.AddAssembly(Assembly assembly)
        {
            AddAssembly(assembly);
        }

        public FluentMappingsContainer Add<T>() where T : IConvention
        {
            conventionFinder.Add<T>();
            return container;
        }

        void IConventionFinder.Add<T>()
        {
            Add<T>();
        }

        public FluentMappingsContainer Add<T>(T instance) where T : IConvention
        {
            conventionFinder.Add(instance);
            return container;
        }

        public FluentMappingsContainer Add<T>(params T[] instances) where T : IConvention
        {
            foreach (var instance in instances)
            {
                conventionFinder.Add(instance);
            }

            return container;
        }

        void IConventionFinder.Add<T>(T instance)
        {
            Add(instance);
        }

        public FluentMappingsContainer Setup(Action<IConventionFinder> setupAction)
        {
            setupAction(this);
            return container;
        }

        public IEnumerable<T> Find<T>() where T : IConvention
        {
            return conventionFinder.Find<T>();
        }
    }
}