namespace FluentNHibernate.MappingModel
{
    public interface IRelationship
    {
        IRelationship OtherSide { get; set; }
    }
}