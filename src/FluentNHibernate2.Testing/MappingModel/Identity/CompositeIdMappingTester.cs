using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Identity
{
    [TestFixture]
    public class CompositeIdMappingTester
    {
        [Test]
        [Explicit("CompositeId is to be implemented later")]
        public void CanConstructValidInstance()
        {            
            var mapping = new CompositeIdMapping();
            mapping.ShouldBeValidAgainstSchema();
        }
    }
}