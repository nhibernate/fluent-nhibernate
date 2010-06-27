using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public class when_a_property_convention_is_applied_to_a_component_mapping
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new TestClassMap());
            model.Add(new MyComponentMap());
            model.Conventions.Add(new MyPropertyConvention());                
        };

        Because of = () =>
            mapping = model.BuildMappingFor<TestClass>();

        It should_apply_the_convention_to_any_properties_that_match_the_acceptance_criteria = () =>
            mapping.Components.Single(x => x.Name == "Value").Properties.Single().Columns.Single().SqlType.ShouldEqual("money");
        
        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;

        private class TestClass
        {
            public int Id { get; set; }
            public MyComponent Value { get; set; }
        }

        private class MyComponent
        {
            public decimal Price { get; set; }
        }

        private class TestClassMap : ClassMap<TestClass>
        {
            public TestClassMap()
            {
                Id(x => x.Id);
                Component(x => x.Value);
            }
        }

        private class MyComponentMap : ComponentMap<MyComponent>
        {
            public MyComponentMap()
            {
                Map(x => x.Price);
            }
        }

        private class MyPropertyConvention : IPropertyConvention, IPropertyConventionAcceptance
        {
            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria.Expect(x => x.Type == typeof(decimal));
            }

            public void Apply(IPropertyInstance instance)
            {
                instance.CustomSqlType("money");
            }
        }
    }
}