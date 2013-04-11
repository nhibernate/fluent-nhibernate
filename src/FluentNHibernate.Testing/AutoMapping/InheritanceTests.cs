using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping.TestFixtures.SuperTypes;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping
{
    [TestFixture]
    public class InheritanceTests: BaseAutoMapFixture
    {
        [Test]
        public void AutoMapSimpleProperties()
        {
            Model<ThirdLevel>();

            Test<ThirdLevel>(mapping =>
            {
                mapping.Element("//id").HasAttribute("name", "Id");
                mapping.Element("//property[@name='Rate']/column").HasAttribute("name", "Rate");
                mapping.Element("//property[@name='SecondRate']/column").HasAttribute("name", "SecondRate");
            });
        }
    }
}
