using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public class XmlNodeSorter
    {
        public static XmlNode SortClassChildren(XmlNode node)
        {
            return new XmlClasslikeNodeSorter().Sort(node);
        }
    }
}