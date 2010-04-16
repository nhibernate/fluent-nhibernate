using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Testing.Automapping;
using FluentNHibernate.Testing.Automapping.ManyToMany;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Steps
{
    [TestFixture]
    public class HasManyToManyStepTests : BaseAutoMapFixture
    {
        [Test]
        public void CanMapManyToManyProperty()
        {
            var Member = ReflectionHelper.GetMember<ManyToMany1>(x => x.Many1);
            var autoMap = new ClassMapping();

            var mapper = new HasManyToManyStep(new DefaultAutomappingConfiguration());
            mapper.Map(autoMap, Member);

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
