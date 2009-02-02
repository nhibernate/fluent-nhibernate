using Examples.FirstProject.Entities;
using FluentNHibernate.Mapping;

namespace Examples.FirstProject.Mappings
{
    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            HasManyToMany(x => x.Products)
                .Cascade.All()
                .WithTableName("StoreProduct");
            HasMany(x => x.Staff)
                .Inverse();
        }
    }
}