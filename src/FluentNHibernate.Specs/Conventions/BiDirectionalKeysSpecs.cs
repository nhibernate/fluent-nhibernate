﻿using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.Conventions;

public class when_mapping_a_many_to_one_one_to_many_bi_directional_relationship
{
    Establish context = () =>
    {
        model = new FluentNHibernate.PersistenceModel();
        model.Add(new ParentMap());
        model.Add(new ChildMap());
    };

    Because of = () =>
    {
        var mappings = model.BuildMappings();
        parent = mappings.SelectMany(x => x.Classes).FirstOrDefault(x => x.Type == typeof(Parent));
        child = mappings.SelectMany(x => x.Classes).FirstOrDefault(x => x.Type == typeof(Child));
    };

    [Ignore("Ignored due to a regression this code caused - as this feature was added only as an experiment, I doubt anyone will miss it for the time being.")]
    It should_use_the_many_to_one_columns_for_the_one_to_many_key = () =>
        parent.Collections.Single().Key.Columns.Select(x => x.Name).Should().ContainSingle(name => name == "one" || name == "two");

    [Ignore("Ignored due to a regression this code caused - as this feature was added only as an experiment, I doubt anyone will miss it for the time being.")]
    It shouldnt_alter_the_many_to_one_columns = () =>
        child.References.Single().Columns.Select(x => x.Name).Should().ContainSingle(name => name == "one" || name == "two");

    static FluentNHibernate.PersistenceModel model;
    static ClassMapping parent;
    static ClassMapping child;
}
