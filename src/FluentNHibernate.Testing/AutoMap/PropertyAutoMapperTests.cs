using FluentNHibernate.FluentInterface.AutoMap;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using System.Linq;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class PropertyAutoMapperTests
    {
        [Test]
        public void ShouldMapProperty()
        {
            var classMap = new ClassMapping() {Type = typeof (PropertyClass)};
            var mapper = new PropertyAutoMapper();
            mapper.Map(classMap);

            var propertyEnumerator = classMap.Properties.GetEnumerator();
            propertyEnumerator.MoveNext();
            Assert.AreEqual(propertyEnumerator.Current.Name, "Id");

            propertyEnumerator.MoveNext();
            Assert.AreEqual(propertyEnumerator.Current.Name, "Name");
        }

        [Test]
        public void ShouldIgnoreNonSystemProperties()
        {
            var classMap = new ClassMapping() {Type = typeof (PropertyClass)};
            var mapper = new PropertyAutoMapper();
            mapper.Map(classMap);

            var hasntMappedSomeClass = classMap.Properties.Count(q => q.Name == "SomeClass") == 0;
            Assert.IsTrue(hasntMappedSomeClass);
        }

        public class PropertyClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public SomeClass SomeClass { get; set; }
        }

        public class SomeClass
        {
        }
    }
}