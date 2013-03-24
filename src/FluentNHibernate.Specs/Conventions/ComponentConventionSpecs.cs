using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public class when_specifying_component_convention
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<ComponentConvention>();
            model.Add(new EntityWithComponentMap());
            model.Add(new AddressMap());
        };

        Because of = () =>
        {
            mapping = model.BuildMappingFor<EntityWithComponent>();
        };

        It should_be_able_to_specify_column_name = () =>
        {
            mapping.Components.First()
                .Properties.Single(x => x.Name == "Count")
                .Columns.FirstOrDefault().Name.ShouldEqual("different");
        };

        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
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

    class AddressMap: ComponentMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.Line1);
            Map(x => x.Line2);
            Map(x => x.Count);
        } 
    }

    public class ComponentConvention: IComponentConvention
    {
        public void Apply(IComponentInstance instance)
        {
            if (instance.Type == typeof(Address))
            {
                instance.Properties.First(p => p.Type.GetType() == typeof(int))
                    .Column("different");
            }
        }
    }

    public class EntityWithComponentMap: ClassMap<EntityWithComponent>
    {
        public EntityWithComponentMap()
        {
            Id(x => x.Id);
            Map(x => x.Description);
            Component(x => x.Address);
        }
    }
}
