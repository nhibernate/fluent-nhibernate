using System.Linq;
using FluentNHibernate.FluentInterface.AutoMap;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class IdAutoMapperTests
    {
        [Test]
        public void ShouldAddIdToClass()
        {
            var classMap = new ClassMapping() { Type = typeof(IdClass)};
            var mapper = new IdAutoMapper();
            mapper.Map(classMap);

            Assert.IsNotNull(classMap.Id);
        }

        [Test]
        public void ShouldSetIdAsProperty()
        {
            var classMap = new ClassMapping() { Type = typeof(IdClass) };
            var mapper = new IdAutoMapper();
            mapper.Map(classMap);

            var id = classMap.Id as IdMapping;
            Assert.AreEqual(id.PropertyInfo.Name, "Id");
        }

        [Test]
        public void ShouldSetColumnNameToMatchPropertyName()
        {
            var classMap = new ClassMapping() { Type = typeof(IdClass) };
            var mapper = new IdAutoMapper();
            mapper.Map(classMap);

            var id = classMap.Id as IdMapping;
            var column = id.Columns.FirstOrDefault();

            Assert.AreEqual(column.Name, "Id");
        }


        public class IdClass
        {
            public int Id { get; set; }
        }
    }


}