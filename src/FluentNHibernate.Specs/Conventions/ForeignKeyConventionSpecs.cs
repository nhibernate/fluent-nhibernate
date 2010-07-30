using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public abstract class ForeignKeyConventionSpec
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add(new TestForeignKeyConvention());
        };

        protected static FluentNHibernate.PersistenceModel model;
        protected static ClassMapping mapping;

        class TestForeignKeyConvention : ForeignKeyConvention
        {
            protected override string GetKeyName(Member property, Type type)
            {
                return "KEY_NAME";
            }
        }
    }

    public class when_a_foreign_key_convention_is_being_applied_to_a_set_mapping : ForeignKeyConventionSpec
    {
        Establish context = () =>
            model.Add(new SetCollectionEntityMap());

        Because of = () =>
            mapping = model.BuildMappingFor<SetCollectionEntity>();

        It should_override_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("KEY_NAME");
    }

    public class when_a_foreign_key_convention_is_being_applied_to_a_set_mapping_with_an_element : ForeignKeyConventionSpec
    {
        Establish context = () =>
            model.Add(new SetElementCollectionEntityMap());

        Because of = () =>
            mapping = model.BuildMappingFor<SetElementCollectionEntity>();

        It should_override_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("KEY_NAME");
    }

    public class when_a_foreign_key_convention_is_being_applied_to_a_set_mapping_with_a_composite_element : ForeignKeyConventionSpec
    {
        Establish context = () =>
            model.Add(new SetCompositeElementCollectionEntityMap());

        Because of = () =>
            mapping = model.BuildMappingFor<SetCompositeElementCollectionEntity>();

        It should_override_the_key_column_name = () =>
            mapping.Collections.Single().Key.Columns.Single().Name.ShouldEqual("KEY_NAME");
    }
}
