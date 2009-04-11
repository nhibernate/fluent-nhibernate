using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Conventions;
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
                    var joe = new SalaryEmployee {Name = "Joe", Salary = new Money(50000, "AUD")};
                    session.Save(joe);

                    var jeff = new CasualEmployee {Name = "Jeff", HourlyWage = new Money(30, "AUD")};
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
                    //.ColumnType(DiscriminatorType.String)
                    .SubClass<SalaryEmployee>("Salary", salaryMap => salaryMap.Component(x => x.Salary, c =>
                        {
                            c.Map(x => x.Amount).ColumnName("Salary");
                            c.Map(x => x.Currency).ColumnName("SalaryCurrency");
                        }))
                    .SubClass<CasualEmployee>("Casual", casualMap => casualMap.Component(x => x.HourlyWage, c =>
                        {
                            c.Map(x => x.Amount).ColumnName("HourlyWage");
                            c.Map(x => x.Currency).ColumnName("HourlyWageCurrency");
                        }));
            }
        }

        protected class EmployeeTablePerClassMap : ClassMap<Employee>
        {
            public EmployeeTablePerClassMap()
            {
                Id(x => x.ID);
                Map(x => x.Name);
                JoinedSubClass<SalaryEmployee>("SalaryEmployeeID", salaryMap => salaryMap.Component(x => x.Salary, c =>
                    {
                        c.Map(x => x.Amount).ColumnName("Salary");
                        c.Map(x => x.Currency).ColumnName("SalaryCurrency");
                    }));
                JoinedSubClass<CasualEmployee>("CasualEmployeeID", casualMap => casualMap.Component(x => x.HourlyWage, c =>
                   {
                       c.Map(x => x.Amount).ColumnName("HourlyWage");
                       c.Map(x => x.Currency).ColumnName("HourlyWageCurrency");
                   }));
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