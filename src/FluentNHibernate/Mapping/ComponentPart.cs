using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping;

public class ComponentPart<T> : ComponentPartBase<T, ComponentPart<T>>, IComponentMappingProvider
{
    readonly Type entity;
    readonly AttributeStore attributes;
    string columnPrefix;

    public ComponentPart(Type entity, Member property)
        : this(entity, property, new AttributeStore())
    {}

    ComponentPart(Type entity, Member property, AttributeStore attributes)
        : base(attributes, property)
    {
        this.attributes = attributes;
        this.entity = entity;
    }

    /// <summary>
    /// Sets the prefix for every column defined within the component. To refer to the name of a member that exposes
    /// the component use {property}
    /// </summary>
    /// <param name="prefix"></param>
    public void ColumnPrefix(string prefix)
    {
        columnPrefix = prefix;
    }

    /// <summary>
    /// Specify the lazy-load behaviour
    /// </summary>
    public ComponentPart<T> LazyLoad()
    {
        attributes.Set("Lazy", Layer.UserSupplied, nextBool);
        nextBool = true;
        return this;
    }
    
    /// <summary>
    /// Configures the tuplizer for this component. The tuplizer defines how to transform
    /// a Property-Value to its persistent representation, and viceversa a Column-Value
    /// to its in-memory representation, and the EntityMode defines which tuplizer is in use.
    /// </summary>
    /// <param name="mode">Tuplizer entity-mode</param>
    /// <param name="tuplizerType">Tuplizer type</param>
    public TuplizerPart Tuplizer(TuplizerMode mode, Type tuplizerType) => CreateTuplizerPart(mode, tuplizerType);

    IComponentMapping IComponentMappingProvider.GetComponentMapping()
    {
        return CreateComponentMapping();
    }

    protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
    {
        var componentMappingRoot = new ComponentMapping(ComponentType.Component, store, member)
        {
            ContainingEntityType = entity
        };
        componentMappingRoot.Set(x => x.Class, Layer.Defaults, new TypeReference(typeof(T)));
        componentMappingRoot.ColumnPrefix = columnPrefix;
        return componentMappingRoot;
    }
}
