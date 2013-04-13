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
                mapping.Element("//property[@name='Rate']/column").HasAttribute("name", "Rate");
                mapping.Element("//property[@name='SecondRate']/column").HasAttribute("name", "SecondRate");
            });
        }

        [Test]
        public void AutoMapInheritedIdProperty()
        {
            Model<SecondLevel>();

            Test<SecondLevel>(mapping => mapping.Element("//id").HasAttribute("name", "Id"));
        }

        [Test]
        public void AutoMapManyToOne()
        {
            Model<FourthLevel>();

            Test<FourthLevel>(mapping => mapping.Element("//many-to-one").HasAttribute("name", "One").
                Element("//many-to-one/column").HasAttribute("name", "One_id"));
        }

        [Test]
        public void AutoMapManyToMany()
        {
            Model<FourthLevel>();

            Test<FourthLevel>(mapping => 
            { 
                mapping.Element("//many-to-many/column").HasAttribute("name", "ManyToMany_id");
            });
        }

        [Test]
        public void AutoMapEnum()
        {
            Model<FourthLevel>(model => model.Override<FourthLevel>(map => map.Map(x => x.publisherType)));

            Test<FourthLevel > (mapping => mapping.Element("//property[@name='publisherType']").Exists());
        }

        [Test]
        public void AutoMapVersion()
        {
            Model<ThirdLevel>();

            Test<ThirdLevel>(mapping =>
            {
                mapping.Element("//version").HasAttribute("name", "Version");
                mapping.Element("//version/column").HasAttribute("name", "Version");
            });
        }
    }
}
