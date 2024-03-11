using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping;

/// <summary>
/// The fluent-interface part for a external component reference. These are
/// components which have their bulk/body declared external to a class mapping
/// and are reusable.
/// </summary>
/// <typeparam name="T">Component type</typeparam>
public class ReferenceComponentPart<T>(Member property, Type containingEntityType) : IReferenceComponentMappingProvider
{
    private string columnPrefix;

    /// <summary>
    /// Sets the prefix for any columns defined within the component. To refer to the property
    /// that exposes this component use {property}.
    /// </summary>
    /// <example>
    /// // Entity using Address component
    /// public class Person
    /// {
    ///   public Address PostalAddress { get; set; }
    /// }
    /// 
    /// ColumnPrefix("{property}_") will result in any columns of Person.Address being prefixed with "PostalAddress_".
    /// </example>
    /// <param name="prefix">Prefix for column names</param>
    public void ColumnPrefix(string prefix)
    {
        columnPrefix = prefix;
    }

    IComponentMapping IComponentMappingProvider.GetComponentMapping()
    {
        return new ReferenceComponentMapping(ComponentType.Component, property, typeof(T), containingEntityType, columnPrefix);
    }

    Type IReferenceComponentMappingProvider.Type => typeof(T);
}
