using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using FluentNHibernate.Specs.ExternalFixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Diagnostics
{
    public class when_registering_types_with_diagnostics_enabled
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = new FluentNHibernate.PersistenceModel();
            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
        {
            model.AddMappingsFromSource(new StubTypeSource(typeof(First), typeof(FirstMap), typeof(SecondMap), typeof(ChildMap), typeof(CompMap)));
            model.BuildMappings();
        };

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_register_each_ClassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(FirstMap), typeof(SecondMap));

        It should_register_each_SubclassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(ChildMap));

        It should_register_each_ComponentMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(CompMap));

        It should_return_the_source_in_the_results = () =>
            results.ScannedSources
                .Where(x => x.Phase == ScanPhase.FluentMappings)
                .Select(x => x.Identifier)
                .ShouldContainOnly("StubTypeSource");

        It should_not_register_non_fluent_mapping_types = () =>
            results.FluentMappings.ShouldNotContain(typeof(First));
        
        static FluentNHibernate.PersistenceModel model;
        static DiagnosticResults results;
    }

    public class when_registering_conventions_with_diagnostics_enabled
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = new FluentNHibernate.PersistenceModel();
            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
        {
            model.Conventions.AddSource(new StubTypeSource(typeof(ConventionA), typeof(ConventionB), typeof(NotAConvention)));
            model.BuildMappings();
        };

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_register_each_convention_type_and_return_them_in_the_results = () =>
            results.Conventions.ShouldContain(typeof(ConventionA), typeof(ConventionB));

        It should_return_the_source_in_the_results = () =>
            results.ScannedSources
                .Where(x => x.Phase == ScanPhase.Conventions)
                .Select(x => x.Identifier)
                .ShouldContainOnly("StubTypeSource");

        It should_not_register_non_convention_types = () =>
            results.Conventions.ShouldNotContain(typeof(NotAConvention));

        static FluentNHibernate.PersistenceModel model;
        static DiagnosticResults results;
    }

    public class when_automapping_with_diagnostics_enabled
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(First), typeof(Second), typeof(Third)), new TestAutomappingConfiguration());

            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(First));

        It should_have_a_reason_of_skipped_by_configuration_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by result of IAutomappingConfiguration.ShouldMap(Type)");

        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));

        static AutoPersistenceModel model;
        static DiagnosticResults results;

        class TestAutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return type != typeof(First);
            }
        }
    }

    public class when_automapping_with_diagnostics_enabled_and_excluding_by_where
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(First), typeof(Second), typeof(Third)))
                .Where(x => x != typeof(First));
            
            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(First));

        It should_have_a_reason_of_skipped_by_explicit_where_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by Where clause");
        
        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));
        
        static AutoPersistenceModel model;
        static DiagnosticResults results;
    }

    public class when_automapping_with_diagnostics_enabled_and_excluding_by_IgnoreBase
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(First), typeof(Second), typeof(Third)))
                .IgnoreBase<First>();

            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(First));

        It should_have_a_reason_of_skipped_by_IgnoreBase_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by IgnoreBase");

        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));

        static AutoPersistenceModel model;
        static DiagnosticResults results;
    }

    public class when_automapping_with_diagnostics_enabled_and_excluding_by_generic_IgnoreBase
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(Something<First>), typeof(Second), typeof(Third)))
                .IgnoreBase(typeof(Something<>));

            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(Something<First>));

        It should_have_a_reason_of_skipped_by_IgnoreBase_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by IgnoreBase");

        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));

        static AutoPersistenceModel model;
        static DiagnosticResults results;
    }

    public class when_automapping_with_diagnostics_enabled_and_excluding_by_layer_supertype
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(Abstract), typeof(Second), typeof(Third)), new TestAutomappingConfiguration());

            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(Abstract));

        It should_have_a_reason_of_skipped_by_IgnoreBase_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by IAutomappingConfiguration.AbstractClassIsLayerSupertype(Type)");

        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));

        static AutoPersistenceModel model;
        static DiagnosticResults results;

        class TestAutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool AbstractClassIsLayerSupertype(Type type)
            {
                return type == typeof(Abstract);
            }
        }
    }

    public class when_automapping_with_diagnostics_enabled_and_excluding_by_explicit_component
    {
        Establish context = () =>
        {
            var dispatcher = new DefaultDiagnosticMessageDispatcher();
            dispatcher.RegisterListener(new StubListener(x => results = x));

            model = AutoMap.Source(new StubTypeSource(typeof(Component), typeof(Second), typeof(Third)), new TestAutomappingConfiguration());

            model.SetLogger(new DefaultDiagnosticLogger(dispatcher));
        };

        Because of = () =>
            model.BuildMappings();

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_include_a_skipped_entry_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Type).ShouldContain(typeof(Component));

        It should_have_a_reason_of_skipped_by_IgnoreBase_for_each_skipped_type = () =>
            results.AutomappingSkippedTypes.Select(x => x.Reason).ShouldContain("Skipped by IAutomappingConfiguration.IsComponent(Type)");

        It should_not_include_a_skipped_entry_for_used_types = () =>
            results.AutomappingSkippedTypes.ShouldNotContain(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_candidate_list = () =>
            results.AutomappingCandidateTypes.ShouldContainOnly(typeof(Second), typeof(Third));

        It should_include_all_unskipped_types_in_the_automapped_list = () =>
            results.AutomappedTypes.Select(x => x.Type).ShouldContainOnly(typeof(Second), typeof(Third));

        static AutoPersistenceModel model;
        static DiagnosticResults results;

        class TestAutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool IsComponent(Type type)
            {
                return type == typeof(Component);
            }
        }
    }

    abstract class Abstract
    {}

    class FirstMap : ClassMap<First>
    {
        public FirstMap()
        {
            Id(x => x.Id);
        }
    }

    class First
    {
        public int Id { get; set; }
    }

    class Something<T>
    {}

    class SecondMap : ClassMap<Second>
    {
        public SecondMap()
        {
            Id(x => x.Id);
        }
    }

    class Second
    {
        public int Id { get; set; }
    }

    class Third
    {
        public int Id { get; set; }
    }

    class ChildMap : SubclassMap<Child> {}

    class Child : Second {}

    class CompMap : ComponentMap<Comp> {}
    class Comp {}
    class ConventionA : IConvention {}
    class ConventionB : IConvention {}
    class NotAConvention {}

    class StubListener : IDiagnosticListener
    {
        readonly Action<DiagnosticResults> receiver;

        public StubListener(Action<DiagnosticResults> receiver)
        {
            this.receiver = receiver;
        }

        public void Receive(DiagnosticResults results)
        {
            receiver(results);
        }
    }
}
