using System;

namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionRelationshipMapping : IMappingBase
    {
        Type ChildType { get; }
        TypeReference Class { get; set; }
        string NotFound { get; set; }
        string EntityName { get; set; }
    }
}