using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.Automapping.Fixtures.Overrides;
using FluentNHibernate.Specs.ExternalFixtures;
using FluentNHibernate.Specs.ExternalFixtures.Overrides;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping
{
    public class when_using_an_automapping_override_to_create_a_join
    {
        Establish context = () =>
            model = AutoMap.Source(new StubTypeSource(typeof(Entity)))
                .Override<Entity>(map =>
                    map.Join("join_table", m => m.Map(x => x.One)));

        Because of = () =>
            mapping = model.BuildMappingFor<Entity>();

        It should_create_the_join_mapping = () =>
            mapping.Joins.ShouldNotBeEmpty();

        It should_have_a_property_in_the_join = () =>
            mapping.Joins.Single().Properties.Select(x => x.Name).ShouldContain("One");

        It should_exclude_the_join_mapped_property_from_the_main_automapping = () =>
            mapping.Properties.Select(x => x.Name).ShouldNotContain("One");
        
        static AutoPersistenceModel model;
        static ClassMapping mapping;
    }


    public class when_using_an_automapping_override_to_specify_a_discriminators_and_join_on_subclass
    {
        private Establish context = () =>
            model = AutoMap.Source(new StubTypeSource(typeof (Parent), typeof (Child)))
                .Override<Parent>(map =>
                    map.DiscriminateSubClassesOnColumn("type"))
                .Override<Child>(map => map.Join("table", part => { }));

        private Because of = () => 
            mapping = model.BuildMappingFor<Parent>();
           


        It should_not_create_the_join_mapping = () =>
            mapping.Joins.ShouldBeEmpty();

        It should_map_the_discriminator = () =>
            mapping.Discriminator.ShouldNotBeNull();

        It should_map_subclasses_as_joined_subclasses = () =>
            mapping.Subclasses.ShouldEachConformTo(x => x.Joins.Any());

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
            mapping.Discriminator.ShouldNotBeNull();

        It should_map_subclasses_as_subclass_instead_of_joined_subclass = () =>
        {
            mapping.Subclasses.Count().ShouldEqual(1);
            mapping.Subclasses.ShouldEachConformTo(x => x.SubclassType == SubclassType.Subclass);
        };
        
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
            mapping.BatchSize.ShouldEqual(1234);

        It should_apply_override_from_the_second_assembly = () =>
            mapping.TableName.ShouldEqual("OverriddenTableName");

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
            entityMapping.EntityName.ShouldEqual("customEntityName");
            parentMapping.TableName.ShouldEqual("fancyTableName_Parent");
            bParentMapping.BatchSize.ShouldEqual(50);
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
}
