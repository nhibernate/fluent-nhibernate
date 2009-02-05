using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.MappingModel.Conventions;
using FluentNHibernate.Reflection;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.MappingModel;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Testing.FluentInterface
{
    [TestFixture]
    public class EmployeesTablePerHierarchyTester
    {
        private class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Id(x => x.ID);
                Map(x => x.Name);
                DiscriminateSubClassesOnColumn("EmployeeType")
                    .ColumnType(DiscriminatorType.String)
                    .SubClass<SalaryEmployee>("Salary", salaryMap => salaryMap.Map(x => x.Salary))
                    .SubClass<CasualEmployee>("Casual", casualMap => casualMap.Map(x => x.HourlyWage));
            }
        }

        [Test]
        public void Employees_xml_should_be_valid_against_schema()
        {
            var model = new PersistenceModel();
            model.Add(new EmployeeMap());

            model.AddConvention(new NamingConvention());
            var hibernateMapping = model.BuildHibernateMapping();
            model.ApplyVisitors(hibernateMapping);
            hibernateMapping.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void Should_allow_employee_entities_to_be_saved()
        {
            var integrationHelper = new IntegrationTestHelper();
            integrationHelper.PersistenceModel.Add(new EmployeeMap());

            integrationHelper.Execute(session =>
            {
                var joe = new SalaryEmployee {Name = "Joe", Salary = 50000};
                session.Save(joe);

                var jeff = new CasualEmployee {Name = "Jeff", HourlyWage = 30};
                session.Save(jeff);
            });
        }
    }
}
