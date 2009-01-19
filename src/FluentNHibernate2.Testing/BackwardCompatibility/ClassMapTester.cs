using System.Linq;
using FluentNHibernate.BackwardCompatibility;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.BackwardCompatibility
{
    [TestFixture]
    public class ClassMapTester
    {
        [Test]
        public void CanSpecifyId()
        {
            var classMap = new ClassMap<Artist>();
            classMap.Id(x => x.ID);
            ClassMapping mapping = classMap.GetClassMapping();

            var id = mapping.Id as IdMapping;
            id.ShouldNotBeNull();
            id.Name.ShouldEqual("ID");            
        }

        [Test]
        public void CanMapProperty()
        {
            var classMap = new ClassMap<Artist>();
            classMap.Map(x => x.Name);
            ClassMapping mapping = classMap.GetClassMapping();

            var property = mapping.Properties.FirstOrDefault();
            property.ShouldNotBeNull();
            property.Name.ShouldEqual("Name");
        }

        [Test]
        public void CanMapCollection()
        {
            var classMap = new ClassMap<Artist>();
            classMap.HasMany<Album>(x => x.Albums);
            ClassMapping mapping = classMap.GetClassMapping();

            var collection = mapping.Collections.FirstOrDefault() as BagMapping;
            collection.ShouldNotBeNull();
            collection.Name.ShouldEqual("Albums");                             
        }

        [Test]
        public void CanMapReference()
        {
            var classMap = new ClassMap<Album>();
            classMap.References(x => x.Artist);
            ClassMapping mapping = classMap.GetClassMapping();

            var reference = mapping.References.FirstOrDefault();
            reference.ShouldNotBeNull();
            reference.Name.ShouldEqual("Artist");
        }
    }
}