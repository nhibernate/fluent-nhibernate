namespace FluentNHibernate.MappingModel.Output
{
    public interface IHbmConverterServiceLocator
    {
        IHbmConverter<T, H> GetConverter<T, H>();
    }
}