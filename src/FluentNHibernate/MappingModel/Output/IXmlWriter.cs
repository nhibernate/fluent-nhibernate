using System.Xml;

namespace FluentNHibernate.MappingModel.Output
{
    public interface IXmlWriter<T>
    {
        XmlDocument Write(T mappingModel);        
    }
}