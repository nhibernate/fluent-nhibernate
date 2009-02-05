using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Testing.DomainModel
{
    public abstract class Employee
    {
        public int ID { get; private set; }
        public string Name { get; set; }
    }

    public class SalaryEmployee : Employee
    {
        public decimal Salary { get; set; }
    }

    public class CasualEmployee : Employee
    {
        public decimal HourlyWage { get; set; }
    }
}
