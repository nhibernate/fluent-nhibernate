using System;
using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public abstract class BaseXmlNodeSorter
    {
        protected const int First = 0;
        protected const int Anywhere = 1;
        protected const int Last = 2;

        public XmlNode Sort(XmlNode node)
        {
            var children = new List<XmlNode>();
            var sorting = GetSorting();

            foreach (XmlNode childNode in node.ChildNodes)
            {
                children.Add(childNode);

                SortChildren(childNode);
            }

            // Creates a copy of the sort order the elments were added in on the node
            var originalSortOrder = children.ToArray();

            children.Sort((x, y) =>
            {
                if (!sorting.ContainsKey(x.Name) || !sorting.ContainsKey(y.Name)) return 0;

                var xSort = sorting[x.Name];
                var ySort = sorting[y.Name];

                //General Position
                if (xSort.Position != ySort.Position) return xSort.Position.CompareTo(ySort.Position);
                //Sub-Position if positions are the same
                if (xSort.Level != ySort.Level) return xSort.Level.CompareTo(ySort.Level);

                //Relative Index based on the order the part was added
                return Array.IndexOf(originalSortOrder, x).CompareTo(Array.IndexOf(originalSortOrder, y));
            });

            for (var i = 0; i < node.ChildNodes.Count; i++)
            {
                node.RemoveChild(node.ChildNodes[i]);
            }

            foreach (var child in children)
            {
                node.AppendChild(child);
            }

            return node;
        }

        protected abstract IDictionary<string, SortValue> GetSorting();
        protected abstract void SortChildren(XmlNode node);
    }
}