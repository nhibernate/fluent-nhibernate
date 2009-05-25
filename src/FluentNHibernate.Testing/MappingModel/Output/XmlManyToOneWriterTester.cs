using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlManyToOneWriterTester
    {
        private XmlManyToOneWriter writer;

        [Test]
        public void ShouldWriteAccessAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Access, "access").MapsToAttribute("access");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteCascadeAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Cascade, "all").MapsToAttribute("cascade");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteClassAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Class, "type").MapsToAttribute("class");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteFetchAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Fetch, "select").MapsToAttribute("fetch");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteForeignKeyAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.ForeignKey, "fk").MapsToAttribute("foreign-key");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteInsertAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Insert, true).MapsToAttribute("insert");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteLazyAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Lazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNameAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Name, "nm").MapsToAttribute("name");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteNotFoundAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.NotFound, "nf").MapsToAttribute("not-found");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteOuterJoinAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.OuterJoin, "outer-join").MapsToAttribute("outer-join");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWritePropertyRefAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.PropertyRef, "pr").MapsToAttribute("property-ref");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteUpdateAttribute()
        {
            writer = new XmlManyToOneWriter();
            var testHelper = new XmlWriterTestHelper<ManyToOneMapping>();
            testHelper.Check(x => x.Update, true).MapsToAttribute("update");

            testHelper.VerifyAll(writer);
        }

    }
}