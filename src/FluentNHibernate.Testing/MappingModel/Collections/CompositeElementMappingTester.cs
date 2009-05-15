using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class CompositeElementMappingTester
    {
        private CompositeElementMapping compositeElementMapping;

        [TestFixtureSetUp]
        public void SetUp()
        {
            compositeElementMapping = new CompositeElementMapping();
        }

        [Test]
        public void ShouldWriteFullTests()
        {
            Assert.Inconclusive();
        }
    }
}
