using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmListWriterTester
    {
        private RhinoAutoMocker<HbmListWriter> _mocker;
        private HbmListWriter _listWriter;

        [SetUp]
        public void SetUp()
        {
            _mocker = new RhinoAutoMocker<HbmListWriter>();
            _listWriter = _mocker.ClassUnderTest;
        }

        [Test]
        public void Should_produce_valid_hbm()
        {
            var list = new ListMapping { Name = "list1", Key = new KeyMapping(), Index = new IndexMapping(), Contents = new OneToManyMapping()};

            _mocker.Get<IXmlWriter<ICollectionContentsMapping>>()
                .Expect(x => x.Write(list.Contents)).Return(new HbmOneToMany { @class = "class1" });

            _mocker.Get<IXmlWriter<KeyMapping>>()
                .Expect(x => x.Write(list.Key)).Return(new HbmKey());

            _mocker.Get<IXmlWriter<IndexMapping>>()
               .Expect(x => x.Write(list.Index)).Return(new HbmIndex());

            _listWriter.ShouldGenerateValidOutput(list);
        }

        [Test]
        public void Should_write_the_attributes()
        {
            var testHelper = new HbmTestHelper<ListMapping>();
            testHelper.Check(x => x.Name, "bag1").MapsToAttribute("name");
            testHelper.Check(x => x.IsInverse, true).MapsToAttribute("inverse");
            testHelper.Check(x => x.IsLazy, true).MapsToAttribute("lazy");

            testHelper.VerifyAll(_listWriter);
        }

        [Test]
        public void Should_write_the_contents()
        {
            var list = new ListMapping { Contents = new OneToManyMapping() };

            _mocker.Get<IXmlWriter<ICollectionContentsMapping>>()
                .Expect(x => x.Write(list.Contents))
                .Return(new HbmOneToMany());

            _listWriter.VerifyXml(list)
                .Element("one-to-many").Exists();
        }

        [Test]
        public void Should_write_the_key()
        {
            var list = new ListMapping { Key = new KeyMapping() };

            _mocker.Get<IXmlWriter<KeyMapping>>()
                .Expect(x => x.Write(list.Key))
                .Return(new HbmKey());

            _listWriter.VerifyXml(list)
                .Element("key").Exists();
        }

        [Test]
        public void Should_write_the_index()
        {
            var list = new ListMapping {Index = new IndexMapping()};

            _mocker.Get<IXmlWriter<IndexMapping>>()
                .Expect(x => x.Write(list.Index))
                .Return(new HbmIndex());

            _listWriter.VerifyXml(list)
                .Element("index").Exists();
        }

        
    }
    
}