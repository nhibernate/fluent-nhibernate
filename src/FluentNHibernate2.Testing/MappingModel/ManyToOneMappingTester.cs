using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class ManyToOneMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var mapping = new ManyToOneMapping("Parent");
            mapping.ShouldBeValidAgainstSchema();
        }
    }
}
