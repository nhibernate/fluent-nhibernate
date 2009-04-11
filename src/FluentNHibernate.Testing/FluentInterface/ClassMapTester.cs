using System.Linq;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Reflection;
using FluentNHibernate.Testing.DomainModel;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterface
{
    [TestFixture]
    public class ClassMapTester
    {
        [Test]
        public void CanCreateClassMap()
        {
            var classMap = new ClassMap<Artist>();
            ClassMapping mapping = classMap.GetClassMapping();

            mapping.Type.ShouldEqual(typeof(Artist));

        }

        [Test]
        public void CanSpecifyId()
        {
            var classMap = new ClassMap<Artist>();
            classMap.Id(x => x.ID);
            ClassMapping mapping = classMap.GetClassMapping();

            var id = mapping.Id as IdMapping;
            id.ShouldNotBeNull();
            id.PropertyInfo.ShouldNotBeNull();
        }

        [Test]
        public void CanMapProperty()
        {
            var classMap = new ClassMap<Artist>();
            classMap.Map(x => x.Name);
            ClassMapping mapping = classMap.GetClassMapping();

            var property = mapping.Properties.FirstOrDefault();
            property.ShouldNotBeNull();
            property.PropertyInfo.ShouldEqual(ReflectionHelper.GetProperty<Artist>(x => x.Name));
        }

        [Test]
        public void CanMapCollection()
        {
            var classMap = new ClassMap<Artist>();
            classMap.HasMany<Album>(x => x.Albums);
            ClassMapping mapping = classMap.GetClassMapping();

            var collection = mapping.Collections.FirstOrDefault() as BagMapping;
            collection.ShouldNotBeNull();
            collection.PropertyInfo.ShouldEqual(ReflectionHelper.GetProperty<Artist>(x => x.Albums));
        }

        [Test]
        public void CanMapReference()
        {
            var classMap = new ClassMap<Album>();
            classMap.References(x => x.Artist);
            ClassMapping mapping = classMap.GetClassMapping();

            var reference = mapping.References.FirstOrDefault();
            reference.ShouldNotBeNull();
            reference.PropertyInfo.ShouldEqual(ReflectionHelper.GetProperty<Album>(x => x.Artist));
        }

        [Test]
        public void CanMapTablePerHierarchy()
        {
            var classMap = new ClassMap<Employee>();
            classMap.DiscriminateSubClassesOnColumn("EmployeeType");
            ClassMapping mapping = classMap.GetClassMapping();

            var discriminator = mapping.Discriminator;
            discriminator.ShouldNotBeNull();
            discriminator.ColumnName.ShouldEqual("EmployeeType");
        }

        [Test]
        public void CanMapTablePerClass()
        {
            var classMap = new ClassMap<Employee>();
            classMap.JoinedSubClass<SalaryEmployee>("SalaryEmployeeID", salaryMap => salaryMap.Map(x => x.Salary));

            var classmapping = classMap.GetClassMapping();
            var salaryMapping = classmapping.Subclasses.FirstOrDefault() as JoinedSubclassMapping;
            salaryMapping.ShouldNotBeNull();
            salaryMapping.Type.ShouldEqual(typeof (SalaryEmployee));
            salaryMapping.Key.Column.ShouldEqual("SalaryEmployeeID");
            salaryMapping.Properties.ShouldHaveCount(1);
        }

        [Test]
        public void CanMapComponent()
        {
            var classMap = new ClassMap<SalaryEmployee>();
            classMap.Component(x => x.Salary, c =>
                {
                    c.Map(x => x.Amount);
                    c.Map(x => x.Currency);
                });

            var classmapping = classMap.GetClassMapping();
            ComponentMapping componentMapping = classmapping.Components.FirstOrDefault();
            componentMapping.ShouldNotBeNull();
            componentMapping.Properties.ShouldHaveCount(2);
        }
    }
}