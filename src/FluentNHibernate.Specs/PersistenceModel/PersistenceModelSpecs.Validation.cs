using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.Visitors;
using Machine.Specifications;

namespace FluentNHibernate.Specs.PersistenceModel
{
    public class when_the_persistence_model_is_told_to_build_the_mappings_with_a_valid_class_mapping : PersistenceModelValidationSpec
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            
            var class_map = new ClassMap<Target>();
            class_map.Id(x => x.Id);
            
            model.Add(class_map);
        };

        Because of = () =>
            exception = Catch.Exception(() => model.BuildMappings());

        It shouldnt_throw_any_validation_exceptions = () =>
            exception.ShouldBeNull();
    }

    public class when_the_persistence_model_is_told_to_build_the_mappings_with_a_class_mapping_that_doesnt_have_an_id : PersistenceModelValidationSpec
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new ClassMap<Target>());
        };

        Because of = () =>
            exception = Catch.Exception(() => model.BuildMappings());

        It should_throw_a_validation_exception = () =>
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ValidationException>();
        };

        It should_indicate_which_entity_is_missing_the_id = () =>
            exception.As<ValidationException>().RelatedEntity.ShouldEqual(typeof(Target));

        It should_explain_how_to_correct_the_error = () =>
            exception.As<ValidationException>().Resolution.ShouldEqual("Use the Id method to map your identity property. For example: Id(x => x.Id)");

        It should_provide_a_sufficently_detailed_message_in_the_exception = () =>
            exception.Message.ShouldEqual("The entity 'Target' doesn't have an Id mapped. Use the Id method to map your identity property. For example: Id(x => x.Id).");
    }

    public class when_the_persistence_model_is_told_to_build_the_mappings_with_a_many_to_many_relationship_with_inverse_specified_on_both_sides : PersistenceModelValidationSpec
    {
        Establish context = () =>
        {
            var left = new ClassMap<Left>();
            left.Id(x => x.Id);
            left.HasManyToMany(x => x.Rights)
                .Inverse();
            var right = new ClassMap<Right>();
            right.Id(x => x.Id);
            right.HasManyToMany(x => x.Lefts)
                .Inverse();

            model = new FluentNHibernate.PersistenceModel();
            model.Add(left);
            model.Add(right);
        };

        Because of = () =>
            exception = Catch.Exception(() => model.BuildMappings());

        It should_throw_a_validation_exception = () =>
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ValidationException>();
        };

        It should_indicate_which_entity_has_the_invalid_many_to_many = () =>
            exception.As<ValidationException>().RelatedEntity.ShouldEqual(typeof(Left));

        It should_explain_how_to_correct_the_error = () =>
            exception.As<ValidationException>().Resolution.ShouldEqual("Remove Inverse from one side of the relationship");

        It should_provide_a_sufficently_detailed_message_in_the_exception = () =>
            exception.Message.ShouldEqual("The relationship Left.Rights to Right.Lefts has Inverse specified on both sides. Remove Inverse from one side of the relationship.");
    }

    public class when_the_persistence_model_with_validation_disabled_is_told_to_build_the_mappings_with_a_class_mapping_that_doesnt_have_an_id : PersistenceModelValidationSpec
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Add(new ClassMap<Target>());
            model.ValidationEnabled = false;
        };

        Because of = () =>
            exception = Catch.Exception(() => model.BuildMappings());

        It shouldnt_throw_any_validation_exceptions = () =>
            exception.ShouldBeNull();
    }

    public abstract class PersistenceModelValidationSpec
    {
        protected static FluentNHibernate.PersistenceModel model;
        protected static Exception exception;

        protected class Target
        {
            public int Id { get; set; }
        }

        protected class Left
        {
            public int Id { get; set; }
            public IList<Right> Rights { get; set; }
        }

        protected class Right
        {
            public int Id { get; set; }
            public IList<Left> Lefts { get; set; }
        }
    }
}