using System.Collections.Generic;

namespace Examples.FirstProject.Entities
{
    public class Store
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual IList<Product> Products { get; set; }
        public virtual IList<Employee> Staff { get; set; }

        public Store()
        {
            Products = new List<Product>();
            Staff = new List<Employee>();
        }

        public virtual void AddProduct(Product product)
        {
            product.StoresStockedIn.Add(this);
            Products.Add(product);
        }

        public virtual void AddProducts(params Product[] products)
        {
            foreach (var product in products)
            {
                AddProduct(product);
            }
        }

        public virtual void AddEmployee(Employee employee)
        {
            employee.Store = this;
            Staff.Add(employee);
        }

        public virtual void AddEmployees(params Employee[] employees)
        {
            foreach (var employee in employees)
            {
                AddEmployee(employee);
            }
        }
    }
}