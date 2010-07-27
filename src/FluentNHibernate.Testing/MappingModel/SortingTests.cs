using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FluentNHibernate.MappingModel.Output.Sorting;
using NUnit.Framework;
using System.Xml;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class SortingTests
    {

        [Test]
        public void ShouldSortNodes()
        {
            var sorter = new XmlClasslikeNodeSorter();

            var xml = @"<class><property /><joined-subclass /><many-to-one /><union-subclass /><cache /><key /><one-to-one /></class>";
            var expected = @"<class><cache /><key /><property /><many-to-one /><one-to-one /><joined-subclass /><union-subclass /></class>";
            
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var node = doc.ChildNodes[0];
            sorter.Sort(node);

            node.OuterXml.ShouldEqual(expected);
        }

        [Test]
        public void ShouldPreserveOrderingOfNodesThatAreAlreadySorted()
        {
            var sorter = new XmlClasslikeNodeSorter();

            var xml = @"<class><cache /><key /><one-to-one /><property /><many-to-one /><joined-subclass /><union-subclass /></class>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var node = doc.ChildNodes[0];
            sorter.Sort(node);

            node.OuterXml.ShouldEqual(xml);            
        }

    }
}
