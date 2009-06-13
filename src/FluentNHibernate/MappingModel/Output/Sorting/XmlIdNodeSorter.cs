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
                { "meta", new SortValue { Position = First, Level = 1 } },
                { "column", new SortValue { Position = Anywhere, Level = 1 } },
                { "generator", new SortValue { Position = Last, Level = 1 } },
            };
        }

        protected override void SortChildren(XmlNode node)
        {}
    }
}