using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.Automapping.Fixtures.Overrides;
using FluentNHibernate.Specs.ExternalFixtures;
using FluentNHibernate.Specs.ExternalFixtures.Overrides;
using Machine.Specifications;
using FluentAssertions;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Specs.Automapping;

public class when_using_an_automapping_override_to_create_a_join
{
    Establish context = () =>
        model = AutoMap.Source(new StubTypeSource(typeof(Entity)))
            .Override<Entity>(map =>
                map.Join("join_table", m => m.Map(x => x.One)));

    Because of = () =>
        mapping = model.BuildMappingFor<Entity>();

    It should_create_the_join_mapping = () =>
        mapping.Joins.Should().NotBeEmpty();

    It should_have_a_property_in_the_join = () =>
        mapping.Joins.Single().Properties.Select(x => x.Name).Should().Contain("One");

    It should_exclude_the_join_mapped_property_from_the_main_automapping = () =>
        mapping.Properties.Select(x => x.Name).Should().NotContain("One");

    static AutoPersistenceModel model;
    static ClassMapping mapping;
}

public class when_using_an_automapping_override_to_specify_a_discriminators_and_join_on_subclass
{
    Establish context = () =>
        model = AutoMap.Source(new StubTypeSource(typeof (Parent), typeof (Child)))
            .Override<Parent>(map =>
                map.DiscriminateSubClassesOnColumn("type"))
            .Override<Child>(map => map.Join("table", part => { }));

    Because of = () => 
        mapping = model.BuildMappingFor<Parent>();

    It should_not_create_the_join_mapping = () =>
        mapping.Joins.Should().BeEmpty();

    It should_map_the_discriminator = () =>
        mapping.Discriminator.Should().NotBeNull();

    It should_map_subclasses_as_joined_subclasses = () =>
        mapping.Subclasses.Should().OnlyContain(x => x.Joins.Any());

    static AutoPersistenceModel model;
    static ClassMapping mapping;
}

public class when_using_an_automapping_override_to_specify_a_discriminator
{
    Establish context = () =>
        model = AutoMap.Source(new StubTypeSource(typeof(Parent), typeof(Child)))
            .Override<Parent>(map =>
                map.DiscriminateSubClassesOnColumn("discriminator"));

    Because of = () =>
        mapping = model.BuildMappingFor<Parent>();

    It should_map_the_discriminator = () =>
        mapping.Discriminator.Should().NotBeNull();

    It should_map_subclasses_as_subclass_instead_of_joined_subclass = () =>
    {
        mapping.Subclasses.Count().Should().Be(1);
        mapping.Subclasses.Should().OnlyContain(subclass => subclass.SubclassType == SubclassType.Subclass);
    };

    static AutoPersistenceModel model;
    static ClassMapping mapping;
}

public class when_using_an_automapping_override_to_specify_a_different_id
{
    Establish context = () =>
        model = AutoMap.Source(new StubTypeSource(typeof(EntityWithDifferentId)))
            .Override<EntityWithDifferentId>(map =>
                map.Id(x => x.DestinationId));

    Because of = () =>
        mapping = model.BuildMappingFor<EntityWithDifferentId>();

    It should_map_the_id = () =>
        mapping.Id.Should().NotBeNull();

    It should_map_id_as_id_mapping = () =>
        mapping.Id.Should().BeOfType<IdMapping>();

    It should_map_id_as_different_id = () =>
        ((IdMapping)mapping.Id).Name.Should().Be("DestinationId");

    static AutoPersistenceModel model;
    static ClassMapping mapping;
}

[Subject(typeof(IAutoMappingOverride<>))]
public class when_using_multiple_overrides_from_different_assemblies
{
    Establish context = () =>
        model = AutoMap.Source(new StubTypeSource(typeof(Entity)))
            .UseOverridesFromAssemblyOf<EntityBatchSizeOverride>()
            .UseOverridesFromAssemblyOf<EntityTableOverride>();

    Because of = () =>
        mapping = model.BuildMappingFor<Entity>();

    It should_apply_override_from_the_first_assembly = () =>
        mapping.BatchSize.Should().Be(1234);

    It should_apply_override_from_the_second_assembly = () =>
        mapping.TableName.Should().Be("OverriddenTableName");

    static AutoPersistenceModel model;
    static ClassMapping mapping;
}

[Subject(typeof(IAutoMappingOverride<>))]
public class when_multiple_overrides_present_in_one_class
{
    Establish context = () =>
    {
        model = AutoMap.Source(new StubTypeSource(typeof(Entity), typeof(Parent), typeof(B_Parent)));
        model.Override(typeof(MultipleOverrides));
    };

    Because of = () =>
    {
        entityMapping = model.BuildMappingFor<Entity>();
        parentMapping = model.BuildMappingFor<Parent>();
        bParentMapping = model.BuildMappingFor<B_Parent>();
    };

    It should_apply_overrides_to_every_class_for_which_such_were_provided = () =>
    {
        entityMapping.EntityName.Should().Be("customEntityName");
        parentMapping.TableName.Should().Be("fancyTableName_Parent");
        bParentMapping.BatchSize.Should().Be(50);
    };


    static AutoPersistenceModel model;
    static ClassMapping entityMapping;
    static ClassMapping parentMapping;
    static ClassMapping bParentMapping;
}

public class MultipleOverrides: IAutoMappingOverride<Entity>, IAutoMappingOverride<Parent>, IAutoMappingOverride<B_Parent>
{
    public void Override(AutoMapping<Entity> mapping)
    {
        mapping.EntityName("customEntityName");
    }

    public void Override(AutoMapping<Parent> mapping)
    {
        mapping.Table("fancyTableName_Parent");
    }

    public void Override(AutoMapping<B_Parent> mapping)
    {
        mapping.BatchSize(50);
    }
}
