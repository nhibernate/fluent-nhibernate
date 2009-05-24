namespace FluentNHibernate.MappingModel.Collections
{
    public interface ICollectionRelationshipMapping : IMappingBase
    {
        string Class { get; }
        string NotFound { get; }
    }
}