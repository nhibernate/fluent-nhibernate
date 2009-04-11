using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Reflection;

namespace FluentNHibernate.FluentInterface
{
    public class ComponentMap<T> : MapBase<T>
    {
        private readonly ComponentMapping _componentMapping;

        public ComponentMap(ComponentMapping componentMapping) : base (componentMapping)
        {
            _componentMapping = componentMapping;
        }
    }
}