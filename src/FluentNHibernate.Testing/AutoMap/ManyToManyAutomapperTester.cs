using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.AutoMap.ManyToMany;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class ManyToManyAutomapperTester : BaseAutoMapFixture
    {
        [Test]
        public void CanMapManyToManyProperty()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new AutoMap<ManyToMany1>();

            var mapper = new ManyToManyAutoMapper(new AutoMappingExpressions());
            mapper.Map(autoMap, propertyInfo);

            autoMap.PropertiesMapped.ShouldHaveCount(1);
        }

        [Test]
        public void CanGetTheManyToManyPart()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new AutoMap<ManyToMany1>();

            var mapper = new ManyToManyAutoMapper(new AutoMappingExpressions());
            object manyToManyPart = mapper.GetManyToManyPart(autoMap, propertyInfo);

            manyToManyPart.ShouldBeOfType(typeof(ManyToManyPart<ManyToMany1, ManyToMany2>));
        }

        [Test]
        public void CanApplyInverse()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var mapper = new ManyToManyAutoMapper(new AutoMappingExpressions());
            var manyToManyPart = MockRepository.GenerateMock<IManyToManyPart>();

            mapper.ApplyInverse(propertyInfo, typeof(ManyToMany1), manyToManyPart);

            manyToManyPart.AssertWasCalled(x => x.Inverse());
        }

        [Test]
        public void GetsTableName()
        {
            Model<ManyToMany1>(model => model
                .Where(type => type == typeof(ManyToMany1)));

            Test<ManyToMany1>(mapping => mapping
                .Element("class/set")
                    .HasAttribute("table", "ManyToMany2ToManyToMany1"));
        }

    }
}
