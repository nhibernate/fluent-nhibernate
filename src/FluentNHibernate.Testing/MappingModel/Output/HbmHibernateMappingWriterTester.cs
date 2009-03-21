using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmHibernateMappingWriterTester
    {
        [Test]
        public void Should_write_default_lazy()
        {
            var hibMap = new HibernateMapping { DefaultLazy = true };
            var writer = new HbmHibernateMappingWriter(null);
            var hbm = writer.Write(hibMap);

            hbm.defaultlazy.ShouldBeTrue();
        }

        [Test]
        public void Should_append_class_mappings()
        {
            var hibernateMap = new HibernateMapping();
            hibernateMap.AddClass(new ClassMapping { Name = "class1" });

            var classWriter = MockRepository.GenerateStub<IHbmWriter<ClassMapping>>();
            classWriter.Stub((x => x.Write(hibernateMap.Classes.First()))).Return(new HbmClass());

            var writer = new HbmHibernateMappingWriter(classWriter);
            writer.VerifyXml(hibernateMap)
                .Element("class").Exists();
        }

    }
}
