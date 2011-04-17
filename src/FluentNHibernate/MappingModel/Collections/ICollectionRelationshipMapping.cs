using System;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionRelationshipMapping : IMapping
    {
        Type ChildType { get; }
        TypeReference Class { get; }
        string NotFound { get; }
        string EntityName { get; }
    }
}