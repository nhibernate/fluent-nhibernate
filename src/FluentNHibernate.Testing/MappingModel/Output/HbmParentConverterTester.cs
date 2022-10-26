using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class HbmParentConverterTester
    {
        private IHbmConverter<ParentMapping, HbmParent> converter;

        [SetUp]
        public void GetConverterFromContainer()
        {
            var container = new HbmConverterContainer();
            converter = container.Resolve<IHbmConverter<ParentMapping, HbmParent>>();
        }

        [Test]
        public void ShouldConvertNameIfPopulated()
        {
            var parentMapping = new ParentMapping();
            parentMapping.Set(fluent => fluent.Name, Layer.Conventions, "testName");
            var convertedHbmParent = converter.Convert(parentMapping);
            convertedHbmParent.name.ShouldEqual(parentMapping.Name);
        }

        [Test]
        public void ShouldNotConvertNameIfNotPopulated()
        {
            var parentMapping = new ParentMapping();
            // Don't set anything on the original mapping
            var convertedHbmParent = converter.Convert(parentMapping);
            var blankHbmParent = new HbmParent();
            convertedHbmParent.name.ShouldEqual(blankHbmParent.name);
        }

        [Test]
        public void ShouldConvertAccessIfPopulated()
        {
            var parentMapping = new ParentMapping();
            parentMapping.Set(fluent => fluent.Access, Layer.Conventions, "acc");
            var convertedHbmParent = converter.Convert(parentMapping);
            convertedHbmParent.access.ShouldEqual(parentMapping.Access);
        }

        [Test]
        public void ShouldNotConvertAccessIfNotPopulated()
        {
            var parentMapping = new ParentMapping();
            // Don't set anything on the original mapping
            var convertedHbmParent = converter.Convert(parentMapping);
            var blankHbmParent = new HbmParent();
            convertedHbmParent.access.ShouldEqual(blankHbmParent.access);
        }
    }
}