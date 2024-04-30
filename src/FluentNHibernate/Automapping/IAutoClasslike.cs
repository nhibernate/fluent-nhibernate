#if USE_NULLABLE
#nullable enable
#endif
using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping;

public interface IAutoClasslike : IMappingProvider
{
    void DiscriminateSubClassesOnColumn(string column);
    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    IAutoClasslike JoinedSubClass(Type type, string keyColumn);
    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    IAutoClasslike SubClass(Type type, string discriminatorValue);
    void AlterModel(ClassMappingBase mapping);
}
