using System;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    public class when_applying_a_filter_to_an_entity_using_conventions
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<FilterClassConvention>();
            model.Add(new FilterTargetMap());
        };

        Because of = () =>
            mapping = model.BuildMappingFor<FilterTarget>();

        It should_add_a_filter_to_the_entity_s_mapping = () =>
            mapping.Filters.ShouldNotBeEmpty();

        It should_set_the_name_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Name.ShouldEqual(FilterClassConvention.FilterName);

        It should_set_the_condition_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Condition.ShouldEqual(FilterClassConvention.FilterCondition);

        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }

    public class when_applying_a_filter_to_a_one_to_many_using_conventions
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<FilterHasManyConvention>();
            model.Add(new FilterTargetMap());
            model.Add(new FilterChildTargetMap());
        };

        Because of = () =>
        {
            var classMapping = model.BuildMappingFor<FilterTarget>();
            mapping = classMapping.Collections.Single(x => x.Relationship is OneToManyMapping);
        };

        It should_add_a_filter_to_the_one_to_many_relationship_s_mapping = () =>
            mapping.Filters.ShouldNotBeEmpty();

        It should_set_the_name_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Name.ShouldEqual(FilterHasManyConvention.FilterName);

        It should_set_the_condition_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Name.ShouldEqual(FilterHasManyConvention.FilterName);

        static FluentNHibernate.PersistenceModel model;
        static CollectionMapping mapping;
    }

    public class when_applying_a_filter_to_a_many_to_many_using_conventions
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<FilterHasManyToManyConvention>();
            model.Add(new FilterTargetMap());
            model.Add(new FilterChildTargetMap());
        };

        Because of = () =>
        {
            var classMapping = model.BuildMappingFor<FilterTarget>();
            mapping = classMapping.Collections.Single(x => x.Relationship is ManyToManyMapping);
        };

        It should_add_a_filter_to_the_one_to_many_relationship_s_mapping = () =>
            mapping.Filters.ShouldNotBeEmpty();

        It should_set_the_name_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Name.ShouldEqual(FilterHasManyConvention.FilterName);

        It should_set_the_condition_of_the_added_filter_correctly = () =>
            mapping.Filters.Single().Name.ShouldEqual(FilterHasManyConvention.FilterName);

        static FluentNHibernate.PersistenceModel model;
        static CollectionMapping mapping;
    }
}
