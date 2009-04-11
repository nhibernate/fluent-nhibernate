using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.Testing.DomainModel;

namespace FluentNHibernate.Testing.FluentInterface
{
    [TestFixture]
    public class DiscriminatorPartTester
    {
        [Test]
        public void Should_add_subclass_to_class_mapping()
        {
            var classMapping = new ClassMapping(typeof(Employee));
            classMapping.Discriminator = new DiscriminatorMapping();
            var part = new DiscriminatorPartOld(classMapping.Discriminator);

            part.SubClass<SalaryEmployee>("Salary", salaryMap => salaryMap.Map(x => x.Salary));

            var subclass = (SubclassMapping)classMapping.Subclasses.FirstOrDefault();
            subclass.ShouldNotBeNull();

            subclass.DiscriminatorValue.ShouldEqual("Salary");
            subclass.Properties.ShouldHaveCount(1);
        }
    }
}