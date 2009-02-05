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
    public abstract class EmployeesTestBase
    {
        protected abstract ClassMap<Employee> GetClassMap();

        [Test]
        public void Employees_xml_should_be_valid_against_schema()
        {
            var model = new PersistenceModel();
            model.Add(GetClassMap());

            model.AddConvention(new NamingConvention());
            var hibernateMapping = model.BuildHibernateMapping();
            model.ApplyVisitors(hibernateMapping);
            hibernateMapping.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void Should_allow_employee_entities_to_be_saved()
        {
            var integrationHelper = new IntegrationTestHelper();
            integrationHelper.PersistenceModel.Add(GetClassMap());

            integrationHelper.Execute(session =>
            {
                var joe = new SalaryEmployee { Name = "Joe", Salary = 50000 };
                session.Save(joe);

                var jeff = new CasualEmployee { Name = "Jeff", HourlyWage = 30 };
                session.Save(jeff);
            });
        }

        protected class EmployeeTablePerHierarchyMap : ClassMap<Employee>
        {
            public EmployeeTablePerHierarchyMap()
            {
                Id(x => x.ID);
                Map(x => x.Name);
                DiscriminateSubClassesOnColumn("EmployeeType")
                    .ColumnType(DiscriminatorType.String)
                    .SubClass<SalaryEmployee>("Salary", salaryMap => salaryMap.Map(x => x.Salary))
                    .SubClass<CasualEmployee>("Casual", casualMap => casualMap.Map(x => x.HourlyWage));
            }
        }

        protected class EmployeeTablePerClassMap : ClassMap<Employee>
        {
            public EmployeeTablePerClassMap()
            {
                Id(x => x.ID);
                Map(x => x.Name);
                JoinedSubClass<SalaryEmployee>("SalaryEmployeeID", salaryMap => salaryMap.Map(x => x.Salary));
                JoinedSubClass<CasualEmployee>("CasualEmployeeID", casualMap => casualMap.Map(x => x.HourlyWage));
            }
        }
    }   

    [TestFixture]
    public class EmployeesTablePerHierarchyTester : EmployeesTestBase
    {
        protected override ClassMap<Employee> GetClassMap()
        {
            return new EmployeeTablePerHierarchyMap();
        }
    }

    [TestFixture]
    public class EmployeesTablePerClassTester : EmployeesTestBase
    {
        protected override ClassMap<Employee> GetClassMap()
        {
            return new EmployeeTablePerClassMap();
        }
    }

    
}
