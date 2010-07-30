using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public class XmlIdNodeSorter : BaseXmlNodeSorter
    {
        protected override IDictionary<string, SortValue> GetSorting()
        {
            return new Dictionary<string, SortValue>
            {
                { "meta", new SortValue { DocumentSection = Top, RankWithinSection = 1 } },
                { "column", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "generator", new SortValue { DocumentSection = Bottom, RankWithinSection = 1 } },
            };
        }

        protected override void SortChildren(XmlNode node)
        {}
    }
}