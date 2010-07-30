using System;
using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public class XmlClasslikeNodeSorter : BaseXmlNodeSorter
    {
        protected override IDictionary<string, SortValue> GetSorting()
        {
            return new Dictionary<string, SortValue>
            {          
                // top section
                { "meta", new SortValue { DocumentSection = Top, RankWithinSection = 1 } },
                { "subselect", new SortValue { DocumentSection = Top, RankWithinSection = 2 } },
                { "cache", new SortValue { DocumentSection = Top, RankWithinSection = 3 } },
                { "synchronize", new SortValue { DocumentSection = Top, RankWithinSection = 4 } },
                { "comment", new SortValue { DocumentSection = Top, RankWithinSection = 5 } },
                { "tuplizer", new SortValue { DocumentSection = Top, RankWithinSection = 6 } },
                { "key", new SortValue { DocumentSection = Top, RankWithinSection = 7 } },
                { "parent", new SortValue { DocumentSection = Top, RankWithinSection = 7 } },
                { "id", new SortValue { DocumentSection = Top, RankWithinSection = 7 } },
                { "composite-id", new SortValue { DocumentSection = Top, RankWithinSection = 7 } },
                { "discriminator", new SortValue { DocumentSection = Top, RankWithinSection = 8 } },
                { "natural-id", new SortValue { DocumentSection = Top, RankWithinSection = 9 } },
                { "version", new SortValue { DocumentSection = Top, RankWithinSection = 10 } },
                { "timestamp", new SortValue { DocumentSection = Top, RankWithinSection = 10 } },

                // middle section
                { "property", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "many-to-one", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "one-to-one", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "component", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "dynamic-component", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "properties", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "any", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "map", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "set", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "list", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "bag", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "idbag", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "array", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },
                { "primitive-array", new SortValue { DocumentSection = Middle, RankWithinSection = 1 } },

                // bottom section
                { "join", new SortValue { DocumentSection = Bottom, RankWithinSection = 1 } },
                { "subclass", new SortValue { DocumentSection = Bottom, RankWithinSection = 2 } },
                { "joined-subclass", new SortValue { DocumentSection = Bottom, RankWithinSection = 3 } },
                { "union-subclass", new SortValue { DocumentSection = Bottom, RankWithinSection = 4 } },
                { "loader", new SortValue { DocumentSection = Bottom, RankWithinSection = 5 } },
                { "sql-insert", new SortValue { DocumentSection = Bottom, RankWithinSection = 6 } },
                { "sql-update", new SortValue { DocumentSection = Bottom, RankWithinSection = 7 } },
                { "sql-delete", new SortValue { DocumentSection = Bottom, RankWithinSection = 8 } },
                { "filter", new SortValue { DocumentSection = Bottom, RankWithinSection = 9 } },
                { "query", new SortValue { DocumentSection = Bottom, RankWithinSection = 10 } },
                { "sql-query", new SortValue { DocumentSection = Bottom, RankWithinSection = 11 } },
            };
        }

        protected override void SortChildren(XmlNode node)
        {
            if (node.Name == "subclass" || node.Name == "joined-subclass" || node.Name == "union-subclass" || node.Name == "component")
                Sort(node);
            else if (node.Name == "id")
                new XmlIdNodeSorter().Sort(node);
            else if (IsCollection(node.Name))
                new XmlCollectionNodeSorter().Sort(node);
        }

        private bool IsCollection(string name)
        {
            return name == "bag" || name == "set" || name == "list" || name == "map" ||
                name == "array" || name == "primitive-array";
        }
    }
}