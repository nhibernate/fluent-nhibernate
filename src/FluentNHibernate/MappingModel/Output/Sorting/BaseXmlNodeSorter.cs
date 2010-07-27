using System;
using System.Collections.Generic;
using System.Xml;

namespace FluentNHibernate.MappingModel.Output.Sorting
{
    public abstract class BaseXmlNodeSorter
    {
        protected const int Top = 0;
        protected const int Middle = 1;
        protected const int Bottom = 2;

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
                if (xSort.DocumentSection != ySort.DocumentSection) return xSort.DocumentSection.CompareTo(ySort.DocumentSection);
                //Sub-Position if positions are the same
                if (xSort.RankWithinSection != ySort.RankWithinSection) return xSort.RankWithinSection.CompareTo(ySort.RankWithinSection);

                //Relative Index based on the order the part was added
                return Array.IndexOf(originalSortOrder, x).CompareTo(Array.IndexOf(originalSortOrder, y));
            });

            while (node.ChildNodes.Count > 0)
                node.RemoveChild(node.ChildNodes[0]);            

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