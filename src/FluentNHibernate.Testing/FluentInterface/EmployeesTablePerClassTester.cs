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
    public class EmployeesTablePerClassTester
    {
        private class EmployeeMap : ClassMap<Employee>
        {
            public EmployeeMap()
            {
                Id(x => x.ID);
                Map(x => x.Name);
                JoinedSubClass<SalaryEmployee>("SalaryEmployeeID", salaryMap => salaryMap.Map(x => x.Salary));
                JoinedSubClass<CasualEmployee>("CasualEmployeeID", casualMap => casualMap.Map(x => x.HourlyWage));
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

    }
}
