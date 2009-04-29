using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmDiscriminatorWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var discriminator = new DiscriminatorMapping();

            var writer = new HbmDiscriminatorWriter(null);
            writer.ShouldGenerateValidOutput(discriminator);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<DiscriminatorMapping>();
            testHelper.Check(x => x.ColumnName, "col1").MapsToAttribute("column");
            testHelper.Check(x => x.DiscriminatorType, DiscriminatorType.Char).MapsToAttribute("type", DiscriminatorType.Char.ToString());
            testHelper.Check(x => x.Force, true).MapsToAttribute("force");
            testHelper.Check(x => x.Formula, "some formula").MapsToAttribute("formula");
            testHelper.Check(x => x.Insert, false).MapsToAttribute("insert");
            testHelper.Check(x => x.IsNotNullable, false).MapsToAttribute("not-null");
            testHelper.Check(x => x.Length, 50).MapsToAttribute("length");

            var writer = new HbmDiscriminatorWriter(null);
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void Should_write_the_column()
        {
            var discriminator = new DiscriminatorMapping { Column = new ColumnMapping() };

            var columnWriter = MockRepository.GenerateStub<IXmlWriter<ColumnMapping>>();
            columnWriter.Expect(x => x.Write(discriminator.Column)).Return(new HbmColumn());

            var writer = new HbmDiscriminatorWriter(columnWriter);
            writer.VerifyXml(discriminator)
                .Element("column").Exists();
        }
        
    }
}
