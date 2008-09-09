using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class DefaultConventionTester
    {
        [Test]
        public void OnlyAppliesTypeConventionIfNoneIsSpecified()
        {            
            var classMap = new ClassMap<Site>();
            var propertyMap = classMap.Map(x => x.LastName);
            propertyMap.SetAttribute("type", "CustomType");

            var convention = new DefaultConvention();
            convention.AlterMap(propertyMap);

            Assert.AreEqual("CustomType", propertyMap.GetAttribute("type"));            
        }        

    }
}
