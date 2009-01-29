namespace FluentNHibernate.MappingModel
{
    public interface INameable
    {
        bool IsNameSpecified { get;}        
        string Name { get; set; }
    }
}