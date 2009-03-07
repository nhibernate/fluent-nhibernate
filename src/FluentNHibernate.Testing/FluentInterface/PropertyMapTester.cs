using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.MappingModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterface
{
    [TestFixture]
    public class PropertyMapTester
    {
        [Test]
        public void Can_set_the_column_name()
        {
            var model = new PropertyMapping();
            var propertyMap = new PropertyMap(model);

            propertyMap.ColumnName("Column1");
            model.ColumnName.ShouldEqual("Column1");
        }
    }
}
