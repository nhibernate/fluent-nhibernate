namespace FluentNHibernate.MappingModel.Output
{
    public interface IHbmConverterServiceLocator
    {
        IHbmConverter<F, H> GetConverter<F, H>();
    }
}