using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.AutoMap.ManyToMany;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class ManyToManyAutomapperTester
    {
        [Test]
        public void CanMapManyToManyProperty()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new AutoMap<ManyToMany1>();

            var mapper = new ManyToManyAutoMapper(new Conventions());
            mapper.Map(autoMap, propertyInfo);

            autoMap.PropertiesMapped.ShouldHaveCount(1);
        }

        [Test]
        public void CanGetTheManyToManyPart()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new AutoMap<ManyToMany1>();

            var mapper = new ManyToManyAutoMapper(new Conventions());
            object manyToManyPart = mapper.GetManyToManyPart(autoMap, propertyInfo);

            manyToManyPart.ShouldBeOfType(typeof(ManyToManyPart<ManyToMany1, ManyToMany2>));
        }

        [Test]
        public void CanApplyInverse()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new AutoMap<ManyToMany1>();

            var mapper = new ManyToManyAutoMapper(new Conventions());

            var manyToManyPart = MockRepository.GenerateMock<IManyToManyPart>();
            manyToManyPart.Expect(x => x.Inverse());
            manyToManyPart.Expect(x => x.WithTableName(null)).IgnoreArguments();

            mapper.ApplyInverse(propertyInfo, typeof(ManyToMany1), manyToManyPart);

            manyToManyPart.VerifyAllExpectations();
        }

    }
}