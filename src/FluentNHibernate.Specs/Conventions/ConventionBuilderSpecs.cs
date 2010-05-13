using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public class when_a_convention_builder_is_used_for_properties
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new TwoPropertyEntityMap());
            model.Conventions.Add(
                ConventionBuilder.Property.When(
                    z => z.Expect(c => c.Name == "TargetProperty"),
                    z => z.CustomSqlType("EXAMPLE")));
        };

        Because of = () =>
            mapping = model.BuildMappingFor<TwoPropertyEntity>();

        It shouldnt_apply_the_convention_to_any_properties_that_dont_match_the_acceptance_criteria = () =>
            mapping.Properties.Single(x => x.Name == "OtherProperty").Columns.Single().SqlType.ShouldNotEqual("EXAMPLE");

        It should_apply_the_convention_to_any_properties_that_match_the_acceptance_criteria = () =>
            mapping.Properties.Single(x => x.Name == "TargetProperty").Columns.Single().SqlType.ShouldEqual("EXAMPLE");

        static IPropertyConvention conventions;
        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }
}
