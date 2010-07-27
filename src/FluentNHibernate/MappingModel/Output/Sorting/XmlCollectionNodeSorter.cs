using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public class XmlCollectionNodeSorter : BaseXmlNodeSorter
    {
        protected override IDictionary<string, SortValue> GetSorting()
        {
            return new Dictionary<string, SortValue>
            {
                { "meta", new SortValue { DocumentSection = Top, RankWithinSection = 1 } },
                { "jcs-cache", new SortValue { DocumentSection = Top, RankWithinSection = 1 } },
                { "cache", new SortValue { DocumentSection = Top, RankWithinSection = 1 } },
                { "key", new SortValue { DocumentSection = Top, RankWithinSection = 3 } },
                { "index", new SortValue { DocumentSection = Top, RankWithinSection = 4 } },
                { "list-index", new SortValue { DocumentSection = Top, RankWithinSection = 4 } },
                { "index-many-to-many", new SortValue { DocumentSection = Top, RankWithinSection = 4} },
                { "element", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "one-to-many", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "many-to-many", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "composite-element", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "many-to-any", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "filter", new SortValue { DocumentSection = Bottom, RankWithinSection = 1 } },
            };
        }

        protected override void SortChildren(XmlNode node)
        { }
    }
}