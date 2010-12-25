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
                { "meta", new SortValue { Position = First, Level = 1 } },
                { "subselect", new SortValue { Position = First, Level = 2 } },
                { "cache", new SortValue { Position = First, Level = 3 } },
                { "synchronize", new SortValue { Position = First, Level = 4 } },
                { "comment", new SortValue { Position = First, Level = 5 } },
                { "tuplizer", new SortValue { Position = First, Level = 6 } },
                { "key", new SortValue { Position = First, Level = 7 } },
                { "parent", new SortValue { Position = First, Level = 7 } },
                { "id", new SortValue { Position = First, Level = 7 } },
                { "composite-id", new SortValue { Position = First, Level = 7 } },
                { "discriminator", new SortValue { Position = First, Level = 8 } },
                { "natural-id", new SortValue { Position = First, Level = 9 } },
                { "version", new SortValue { Position = First, Level = 10 } },
                { "timestamp", new SortValue { Position = First, Level = 10 } },

                // middle section
                { "property", new SortValue { Position = Anywhere, Level = 1 } },
                { "many-to-one", new SortValue { Position = Anywhere, Level = 1 } },
                { "one-to-one", new SortValue { Position = Anywhere, Level = 1 } },
                { "component", new SortValue { Position = Anywhere, Level = 1 } },
                { "dynamic-component", new SortValue { Position = Anywhere, Level = 1 } },
                { "properties", new SortValue { Position = Anywhere, Level = 1 } },
                { "any", new SortValue { Position = Anywhere, Level = 1 } },
                { "map", new SortValue { Position = Anywhere, Level = 1 } },
                { "set", new SortValue { Position = Anywhere, Level = 1 } },
                { "list", new SortValue { Position = Anywhere, Level = 1 } },
                { "bag", new SortValue { Position = Anywhere, Level = 1 } },
                { "idbag", new SortValue { Position = Anywhere, Level = 1 } },
                { "array", new SortValue { Position = Anywhere, Level = 1 } },
                { "primitive-array", new SortValue { Position = Anywhere, Level = 1 } },

                // bottom section
                { "join", new SortValue { Position = Last, Level = 1 } },
                { "subclass", new SortValue { Position = Last, Level = 2 } },
                { "joined-subclass", new SortValue { Position = Last, Level = 3 } },
                { "union-subclass", new SortValue { Position = Last, Level = 4 } },
                { "loader", new SortValue { Position = Last, Level = 5 } },
                { "sql-insert", new SortValue { Position = Last, Level = 6 } },
                { "sql-update", new SortValue { Position = Last, Level = 7 } },
                { "sql-delete", new SortValue { Position = Last, Level = 8 } },
                { "filter", new SortValue { Position = Last, Level = 9 } },
                { "query", new SortValue { Position = Last, Level = 10 } },
                { "sql-query", new SortValue { Position = Last, Level = 11 } },
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