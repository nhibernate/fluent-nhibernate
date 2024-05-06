using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping;

public class IndexManyToManyPart(Type entity)
{
    readonly List<string> columns = [];
    readonly AttributeStore attributes = new AttributeStore();

    public IndexManyToManyPart Column(string indexColumnName)
    {
        columns.Add(indexColumnName);
        return this;
    }

    public IndexManyToManyPart Type<TIndex>()
    {
        attributes.Set("Class", Layer.UserSupplied, new TypeReference(typeof(TIndex)));
        return this;
    }

    public IndexManyToManyPart Type(Type indexType)
    {
        attributes.Set("Class", Layer.UserSupplied, new TypeReference(indexType));
        return this;
    }

    /// <summary>
    /// Specifies an entity-name.
    /// </summary>
    /// <remarks>See https://nhibernate.info/blog/2008/10/21/entity-name-in-action-a-strongly-typed-entity.html </remarks>
    public IndexManyToManyPart EntityName(string entityName)
    {
        attributes.Set("EntityName", Layer.UserSupplied, entityName);
        return this;
    }

    [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
    public IndexManyToManyMapping GetIndexMapping()
    {
        var mapping = new IndexManyToManyMapping(attributes.Clone())
        {
            ContainingEntityType = entity
        };

        columns.Each(name =>
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, name);
            mapping.AddColumn(Layer.UserSupplied, columnMapping);
        });

        return mapping;
    }
}
