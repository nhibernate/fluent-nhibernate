using Examples.FirstProject.Entities;
using FluentNHibernate.Mapping;

namespace Examples.FirstProject.Mappings
{
    public class LocationMap : ComponentMap<Location>
    {
        public LocationMap()
        {
            Map(x => x.Aisle);
            Map(x => x.Shelf);
        }
    }
}