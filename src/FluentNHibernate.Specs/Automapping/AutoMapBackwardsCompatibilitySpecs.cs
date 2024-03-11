using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;
using FluentAssertions;

namespace FluentNHibernate.Specs.Automapping;
// just verify that the obsolete methods still work until we bin them entirely

public class when_using_automap_obsolete_where_method_for_assembly_of : AutoMapObsoleteSpec
{
    Because of = () =>
        AutoMap.AssemblyOf<EntityUsingPrivateFields>(where_clause)
            .BuildMappings();

    It should_use_the_where_clause_provided = () =>
        was_called.Should().BeTrue();
}

public class when_using_automap_obsolete_where_method_for_an_assembly : AutoMapObsoleteSpec
{
    Because of = () =>
        AutoMap.Assembly(typeof(EntityUsingPrivateFields).Assembly, where_clause)
            .BuildMappings();

    It should_use_the_where_clause_provided = () =>
        was_called.Should().BeTrue();
}

public class when_using_automap_obsolete_where_method_for_a_source : AutoMapObsoleteSpec
{
    Because of = () =>
        AutoMap.Source(new StubTypeSource(typeof(EntityUsingPrivateFields)), where_clause)
            .BuildMappings();

    It should_use_the_where_clause_provided = () =>
        was_called.Should().BeTrue();
}

public abstract class AutoMapObsoleteSpec
{
    protected static Func<Type, bool> where_clause = x => !(was_called = true);
    protected static bool was_called;
}
