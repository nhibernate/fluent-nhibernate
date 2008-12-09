using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Identity
{
    [TestFixture]
    public class IdMappingTester
    {
        [Test]
        public void CanConstructValidInstance()
        {
            var idMapping = new IdMapping("ID", new IdColumnMapping("column1"), IdGeneratorMapping.NativeGenerator);            
            idMapping.ShouldBeValidAgainstSchema();
        }
    }
}