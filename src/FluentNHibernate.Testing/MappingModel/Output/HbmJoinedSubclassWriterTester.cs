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
    public class HbmJoinedSubclassWriterTester
    {
        [Test]
        public void Should_produce_valid_hbm()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Name = "joinedsubclass1", Key = new KeyMapping() };
            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());
            var writer = new HbmJoinedSubclassWriter(null, null, null, keyWriter);

            writer.ShouldGenerateValidOutput(joinedSubclassMapping);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<JoinedSubclassMapping>();
            testHelper.Check(x => x.Name, "mapping1").MapsToAttribute("name");

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            testHelper.VerifyAll(writer);
        }

        [Test]
        public void Should_write_the_key()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping { Key = new KeyMapping() };

            var keyWriter = MockRepository.GenerateStub<IHbmWriter<KeyMapping>>();
            keyWriter.Expect(x => x.Write(joinedSubclassMapping.Key)).Return(new HbmKey());
            var writer = new HbmJoinedSubclassWriter(null, null, null, keyWriter);

            writer.VerifyXml(joinedSubclassMapping)
                .Element("key").Exists();
        }

        [Test]
        public void Should_write_the_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();
            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping());

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists();
        }

        [Test]
        public void Should_write_multiple_nestings_of_subclasses()
        {
            var joinedSubclassMapping = new JoinedSubclassMapping();

            joinedSubclassMapping.AddSubclass(new JoinedSubclassMapping { Name = "Child" });
            joinedSubclassMapping.Subclasses.First().AddSubclass(new JoinedSubclassMapping { Name = "Granchild" });

            var writer = new HbmJoinedSubclassWriter(null, null, null, null);
            writer.VerifyXml(joinedSubclassMapping)
                .Element("joined-subclass").Exists().HasAttribute("name", "Child")
                .Element("joined-subclass").Exists().HasAttribute("name", "Granchild");
        }
    }
}
