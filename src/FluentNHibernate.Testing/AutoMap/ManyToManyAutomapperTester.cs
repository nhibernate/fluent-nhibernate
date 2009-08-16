using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.Automapping.ManyToMany;
using FluentNHibernate.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class ManyToManyAutomapperTester : BaseAutoMapFixture
    {
        [Test]
        public void CanMapManyToManyProperty()
        {
            var propertyInfo = ReflectionHelper.GetProperty<ManyToMany1>(x => x.Many1);
            var autoMap = new ClassMapping();

            var mapper = new AutoMapManyToMany(new AutoMappingExpressions());
            mapper.Map(autoMap, propertyInfo);

            autoMap.Collections.ShouldHaveCount(1);
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
