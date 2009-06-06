namespace FluentNHibernate.MappingModel.Output
{
    public interface IXmlWriterServiceLocator
    {
        IXmlWriter<T> GetWriter<T>();
    }
}