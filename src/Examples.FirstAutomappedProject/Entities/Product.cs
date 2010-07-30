using System.Collections.Generic;

namespace Examples.FirstAutomappedProject.Entities
{
    public class Product
    {
        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual Location Location { get; set; }
        public virtual IList<Store> StoresStockedIn { get; set; }

        public Product()
        {
            StoresStockedIn = new List<Store>();
        }
    }
}