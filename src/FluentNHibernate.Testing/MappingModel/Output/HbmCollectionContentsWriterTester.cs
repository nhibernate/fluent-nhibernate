using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using Rhino.Mocks;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmCollectionContentsWriterTester
    {
        [Test]
        public void Should_write_one_to_many()
        {
            var oneToMany = new OneToManyMapping();
            var oneToManyWriter = MockRepository.GenerateStub<IXmlWriter<OneToManyMapping>>();
            oneToManyWriter.Expect(x => x.Write(oneToMany)).Return(new HbmOneToMany());
            var writer = new HbmCollectionContentsWriter(oneToManyWriter, null);

            object result = writer.Write(oneToMany);
            result.ShouldBeOfType(typeof(HbmOneToMany));
        }

        [Test]
        public void Should_write_many_to_many()
        {
            var manyToMany = new ManyToManyMapping();
            var manyTomanyWriter = MockRepository.GenerateStub<IXmlWriter<ManyToManyMapping>>();
            manyTomanyWriter.Expect(x => x.Write(manyToMany)).Return(new HbmManyToMany());

            var writer = new HbmCollectionContentsWriter(null, manyTomanyWriter);

            object result = writer.Write(manyToMany);
            result.ShouldBeOfType(typeof(HbmManyToMany));
        }

        [Test]
        public void Should_return_null_when_writing_unrecognised_object_after_having_processed_a_recognised_object()
        {
            var recognisedMapping = new OneToManyMapping();
            var oneToManyWriter = MockRepository.GenerateStub<IXmlWriter<OneToManyMapping>>();
            oneToManyWriter.Expect(x => x.Write(recognisedMapping)).Return(new HbmOneToMany());
            var writer = new HbmCollectionContentsWriter(oneToManyWriter, null);

            writer.ProcessOneToMany(recognisedMapping);

            var result = writer.Write(new UnrecognisedCollectionContents());
            result.ShouldBeNull();
        }

        private class UnrecognisedCollectionContents : ICollectionContentsMapping
        {
            public void AcceptVisitor(IMappingModelVisitor visitor)
            {

            }
        }
    }
}