using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public interface IAutoClasslike : IMappingProvider
    {
        void DiscriminateSubClassesOnColumn(string column);
        IAutoClasslike JoinedSubClass(Type type, string keyColumn);
        IAutoClasslike SubClass(Type type, string discriminatorValue);
        void AlterModel(ClassMappingBase mapping);
    }
}