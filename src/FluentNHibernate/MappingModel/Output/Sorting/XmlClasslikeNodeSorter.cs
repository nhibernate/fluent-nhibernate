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
                { "cache", new SortValue { Position = First, Level = 1 } },
                { "key", new SortValue { Position = First, Level = 1 } },
                { "id", new SortValue { Position = First, Level = 2 } },
                { "composite-id", new SortValue { Position = First, Level = 2 } },
                { "discriminator", new SortValue { Position = First, Level = 3 } },
                { "version", new SortValue { Position = First, Level = 4 } },
                { "component", new SortValue { Position = Anywhere, Level = 1 } },
                { "dynamic-component", new SortValue { Position = Anywhere, Level = 1 } },
                { "one-to-one", new SortValue { Position = Anywhere, Level = 1 } },
                { "property", new SortValue { Position = Anywhere, Level = 2 } },
                { "many-to-one", new SortValue { Position = Anywhere, Level = 3 } },
                { "array", new SortValue { Position = Anywhere, Level = 3 } },
                { "bag", new SortValue { Position = Anywhere, Level = 3 } },
                { "set", new SortValue { Position = Anywhere, Level = 3 } },
                { "map", new SortValue { Position = Anywhere, Level = 3 } },
                { "list", new SortValue { Position = Anywhere, Level = 3 } },
                { "joined-subclass", new SortValue { Position = Anywhere, Level = 4 } },
                { "subclass", new SortValue { Position = Last, Level = 3 } },
                { "join", new SortValue { Position = Last, Level = 3 } },
                { "any", new SortValue { Position = Anywhere, Level = 2}}
            };
        }

        protected override void SortChildren(XmlNode node)
        {
            if (node.Name == "subclass" || node.Name == "joined-subclass")
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