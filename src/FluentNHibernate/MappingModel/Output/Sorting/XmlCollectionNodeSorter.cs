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
                { "meta", new SortValue { Position = First, Level = 1 } },
                { "jcs-cache", new SortValue { Position = First, Level = 2 } },
                { "cache", new SortValue { Position = First, Level = 2 } },
                { "key", new SortValue { Position = First, Level = 3 } },
                { "index", new SortValue { Position = First, Level = 4 } },
                { "list-index", new SortValue { Position = First, Level = 4 } },
                { "index-many-to-many", new SortValue { Position = First, Level = 4} },
                { "element", new SortValue { Position = Anywhere, Level = 1 } },
                { "one-to-many", new SortValue { Position = Anywhere, Level = 1 } },
                { "many-to-many", new SortValue { Position = Anywhere, Level = 1 } },
                { "composite-element", new SortValue { Position = Anywhere, Level = 1 } },
                { "many-to-any", new SortValue { Position = Anywhere, Level = 1 } },
                { "filter", new SortValue { Position = Last, Level = 1 } },
            };
        }

        protected override void SortChildren(XmlNode node)
        { }
    }
}