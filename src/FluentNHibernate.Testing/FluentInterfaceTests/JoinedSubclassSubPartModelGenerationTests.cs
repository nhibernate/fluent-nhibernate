using System.Linq;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class JoinedSubclassSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.Component(x => x.Component, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void DynamicComponentShouldAddToModelComponentsCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.DynamicComponent(x => x.ExtensionData, c => { }))
                .ModelShouldMatch(x => x.Components.Count().ShouldEqual(1));
        }

        [Test]
        public void MapShouldAddToModelPropertiesCollection()
        {
            JoinedSubClass<PropertyTarget>()
                .Mapping(m => m.Map(x => x.Name))
                .ModelShouldMatch(x => x.Properties.Count().ShouldEqual(1));
        }
    }
}