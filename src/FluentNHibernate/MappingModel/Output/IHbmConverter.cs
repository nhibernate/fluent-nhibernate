namespace FluentNHibernate.MappingModel.Output
{
    public interface IHbmConverter<T, H>
    {
        H Convert(T mappingModel);        
    }
}