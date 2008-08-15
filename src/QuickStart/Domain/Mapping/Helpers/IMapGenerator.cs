using System.Xml;

namespace FluentNHibernate.QuickStart.Domain.Mapping
{
    public interface IMapGenerator
    {
        string FileName { get; }
        XmlDocument Generate();
    }
}