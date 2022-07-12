namespace FluentNHibernate.MappingModel.Output
{
    public interface IHbmConverter<F, H>
    {
        H Convert(F fluentMapping);
    }
}