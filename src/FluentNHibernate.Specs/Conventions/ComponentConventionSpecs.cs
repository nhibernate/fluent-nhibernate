using FluentAssertions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;
using System.Linq;

namespace FluentNHibernate.Specs.Conventions;

public class when_specifying_component_convention
{
    Establish context = () =>
    {
        model = new FluentNHibernate.PersistenceModel();
        model.Conventions.Add<ComponentConvention>();
        model.Add(new AddressMap());
        model.Add(new EntityWithComponentMap());
    };

    Because of = () =>
    {
        mapping = model.BuildMappingFor<EntityWithComponent>();
    };

    It should_be_able_to_specify_column_name = () =>
    {
        var property = mapping.Components.First()
            .Properties.Single(x => x.Name == "Count");

        property.Columns.FirstOrDefault().Name.Should().Be("different");
    };

    private static FluentNHibernate.PersistenceModel model;
    private static ClassMapping mapping;
}

public class EntityWithComponent
{
    public int Id { get; set; }
    public string Description { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public int Count { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
}

internal class AddressMap : ComponentMap<Address>
{
    public AddressMap()
    {
        Map(x => x.Line1);
        Map(x => x.Line2);
        Map(x => x.Count);
    }
}

public class ComponentConvention : IComponentConvention
{
    public void Apply(IComponentInstance instance)
    {
        if (instance.Type == typeof(Address))
        {
            var type = instance.Properties.First(p => p.Type == typeof(int));
            type.Column("different");
        }
    }
}

public class EntityWithComponentMap : ClassMap<EntityWithComponent>
{
    public EntityWithComponentMap()
    {
        Id(x => x.Id);
        Map(x => x.Description);
        Component(x => x.Address);
    }
}
