using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping;

public class CollectionIdPart : ICollectionIdMappingProvider
{
    readonly Type entity;
    readonly AttributeStore attributes = new();
    
    /// <summary>
    /// Specify the generator
    /// </summary>
    /// <example>
    /// .AsIdBag&lt;int&gt;(x => x.Column("Id").GeneratedBy.Identity())
    /// </example>
    public IdentityGenerationStrategyBuilder<CollectionIdPart> GeneratedBy { get; }

    public CollectionIdPart(Type entityType, Type idColumnType)
    {
        attributes.Set("Type", Layer.UserSupplied, new TypeReference(idColumnType));
        entity = entityType;
        GeneratedBy = new IdentityGenerationStrategyBuilder<CollectionIdPart>(this, idColumnType, entityType);
        SetDefaultGenerator(idColumnType);
    }
    
    /// <summary>
    /// Specifies the id column length
    /// </summary>
    /// <param name="length">Column length</param>
    public CollectionIdPart Length(int length)
    {
        attributes.Set("Length", Layer.UserSupplied, length);
        return this;
    }
    
    /// <summary>
    /// Specifies the column name for the collection id.
    /// </summary>
    /// <param name="idColumnName">Column name</param>
    public CollectionIdPart Column(string idColumnName)
    {
        attributes.Set("Column", Layer.UserSupplied, idColumnName);
        return this;
    }
    
    void SetDefaultGenerator(Type idColumnType)
    {
        var generatorMapping = new GeneratorMapping();
        new GeneratorBuilder(generatorMapping, idColumnType, Layer.UserSupplied).SetDefault();
        attributes.Set("Generator", Layer.Defaults, generatorMapping);
    }

    CollectionIdMapping ICollectionIdMappingProvider.GetCollectionIdMapping()
    {
        var mapping = new CollectionIdMapping(attributes.Clone())
        {
            ContainingEntityType = entity
        };
        
        mapping.Set(x => x.Column, Layer.Defaults, "Id");
        if (GeneratedBy.IsDirty)
            mapping.Set(x => x.Generator, Layer.UserSupplied, GeneratedBy.GetGeneratorMapping());
        
        return mapping;
    }
}
