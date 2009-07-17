using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public interface INHModelWriter<T>
    {
        object Write(T mappingModel);        
    }
}