namespace FluentNHibernate.MappingModel.Output
{
    public interface IHbmConverter<F, H>
        where F: IMapping
    {
        H Convert(F fluentMapping);
    }
}