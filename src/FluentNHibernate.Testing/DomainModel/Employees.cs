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
        public Money Salary { get; set; }
    }

    public class CasualEmployee : Employee
    {
        public Money HourlyWage { get; set; }
    }

    public class Money
    {
        private Money()
        { }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
    }


}
