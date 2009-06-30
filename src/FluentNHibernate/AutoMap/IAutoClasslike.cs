using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.AutoMap
{
    public interface IAutoClasslike : IMappingProvider
    {
        IEnumerable<PropertyInfo> PropertiesMapped { get; }
        object GetMapping();
        void DiscriminateSubClassesOnColumn(string column);
        IAutoClasslike JoinedSubClass(Type type, string keyColumn);
        IAutoClasslike SubClass(Type type, string discriminatorValue);
    }
}