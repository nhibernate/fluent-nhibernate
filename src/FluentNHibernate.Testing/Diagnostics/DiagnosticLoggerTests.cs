using System.Linq;
using FakeItEasy;
using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class DiagnosticLoggerTests
    {
        [Test]
        public void should_not_flush_messages_to_dispatcher_when_no_messages_logged()
        {
            var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

            logger.Flush();

            A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._)).MustNotHaveHappened();
        }

        [Test]
        public void should_flush_messages_to_dispatcher()
        {
            var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

            logger.FluentMappingDiscovered(typeof(SomeClassMap));
            logger.Flush();

            A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._)).MustHaveHappened();
        }

        [Test]
        public void should_add_class_maps_to_result()
        {
            var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;

			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.FluentMappingDiscovered(typeof(SomeClassMap));
            logger.Flush();

            result.FluentMappings.ShouldContain(typeof(SomeClassMap));
        }

        [Test]
        public void should_add_conventions_to_result()
        {
            var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.ConventionDiscovered(typeof(SomeConvention));
			logger.Flush();

            result.Conventions.ShouldContain(typeof(SomeConvention));
        }

        [Test]
        public void should_add_scanned_fluent_mapping_sources_to_result()
        {
			var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.LoadedFluentMappingsFromSource(new StubTypeSource());
            logger.Flush();

            result.ScannedSources
                .Where(x => x.Phase == ScanPhase.FluentMappings)
                .Select(x => x.Identifier)
                .ShouldContain("StubTypeSource");
        }

        [Test]
        public void should_add_scanned_convention_sources_to_result()
        {
			var dispatcher = A.Fake<IDiagnosticMessageDispatcher> ();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.LoadedConventionsFromSource(new StubTypeSource());
            logger.Flush();

            result.ScannedSources
                .Where(x => x.Phase == ScanPhase.Conventions)
                .Select(x => x.Identifier)
                .ShouldContain("StubTypeSource");
        }

        [Test]
        public void should_add_skipped_automap_types_to_result()
        {
			var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.AutomappingSkippedType(typeof(object), "reason");
            logger.Flush();

            result.AutomappingSkippedTypes
                .ShouldContain(new SkippedAutomappingType
                {
                    Type = typeof(object),
                    Reason = "reason"
                });
        }

        [Test]
        public void should_add_automapping_candidates()
        {
			var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
			var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.AutomappingCandidateTypes(new[] { typeof(object) });
            logger.Flush();

            result.AutomappingCandidateTypes
                .ShouldContain(typeof(object));
        }

        [Test]
        public void should_add_automapping_type_with_begin()
        {
			var dispatcher = A.Fake<IDiagnosticMessageDispatcher>();
            var logger = new DefaultDiagnosticLogger(dispatcher);

			DiagnosticResults result = null;
			A.CallTo(() => dispatcher.Publish(A<DiagnosticResults>._))
				.Invokes(a => { result = (DiagnosticResults)a.Arguments.First(); });

            logger.BeginAutomappingType(typeof(object));
            logger.Flush();

            result.AutomappedTypes
                .Select(x => x.Type)
                .ShouldContain(typeof(object));
        }

        class SomeClassMap : ClassMap<SomeClass> { }
        class SomeClass { }
        class SomeConvention : IConvention { }
    }
}
