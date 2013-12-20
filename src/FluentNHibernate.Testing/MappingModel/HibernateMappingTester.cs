using FakeItEasy;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel
{
    [TestFixture]
    public class HibernateMappingTester
    {
        [Test]
        public void CanAddClassMappings()
        {
            var hibMap = new HibernateMapping();
            var classMap1 = new ClassMapping();
            var classMap2 = new ClassMapping();

            hibMap.AddClass(classMap1);
            hibMap.AddClass(classMap2);

            hibMap.Classes.ShouldContain(classMap1);
            hibMap.Classes.ShouldContain(classMap2);
        }

        [Test]
        public void ShouldPassClassmappingsToTheVisitor()
        {
            // FakeItEasy calls ToString methods, which ends up in NullPointer
            // if Type attribute is not the AttributeStore
            var attributeStore = new AttributeStore();
            attributeStore.Set("Type", 0, typeof(object));

            var hibMap = new HibernateMapping();
            var classMap = new ClassMapping(attributeStore);
            hibMap.AddClass(classMap);

            var visitor = A.Fake<IMappingModelVisitor>();

            hibMap.AcceptVisitor(visitor);

            A.CallTo(() => visitor.Visit(classMap)).MustHaveHappened();
        }
    }
}