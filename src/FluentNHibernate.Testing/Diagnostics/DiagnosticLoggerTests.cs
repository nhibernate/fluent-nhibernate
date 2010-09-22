using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class DiagnosticLoggerTests
    {
        [Test]
        public void should_not_flush_messages_to_despatcher_when_no_messages_logged()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.Flush();

            despatcher.AssertWasNotCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything));
        }

        [Test]
        public void should_flush_messages_to_despatcher()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.FluentMappingDiscovered(typeof(SomeClassMap));
            logger.Flush();

            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything));
        }

        [Test]
        public void should_add_class_maps_to_result()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.FluentMappingDiscovered(typeof(SomeClassMap));
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.FluentMappings.ShouldContain(typeof(SomeClassMap));
        }

        [Test]
        public void should_add_conventions_to_result()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.ConventionDiscovered(typeof(SomeConvention));
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.Conventions.ShouldContain(typeof(SomeConvention));
        }

        [Test]
        public void should_add_scanned_fluent_mapping_sources_to_result()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.LoadedFluentMappingsFromSource(new StubTypeSource());
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.ScannedSources
                .Where(x => x.Phase == ScanPhase.FluentMappings)
                .Select(x => x.Identifier)
                .ShouldContain("StubTypeSource");
        }

        [Test]
        public void should_add_scanned_convention_sources_to_result()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.LoadedConventionsFromSource(new StubTypeSource());
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.ScannedSources
                .Where(x => x.Phase == ScanPhase.Conventions)
                .Select(x => x.Identifier)
                .ShouldContain("StubTypeSource");
        }

        [Test]
        public void should_add_skipped_automap_types_to_result()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.AutomappingSkippedType(typeof(object), "reason");
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

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
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.AutomappingCandidateTypes(new[] { typeof(object) });
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.AutomappingCandidateTypes
                .ShouldContain(typeof(object));
        }

        [Test]
        public void should_add_automapping_type_with_begin()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var logger = new DefaultDiagnosticLogger(despatcher);

            logger.BeginAutomappingType(typeof(object));
            logger.Flush();

            DiagnosticResults result = null;
            despatcher.AssertWasCalled(x => x.Publish(Arg<DiagnosticResults>.Is.Anything),
                c => c.Callback<DiagnosticResults>(x =>
                {
                    result = x;
                    return true;
                }));

            result.AutomappedTypes
                .Select(x => x.Type)
                .ShouldContain(typeof(object));
        }

        class SomeClassMap : ClassMap<SomeClass> { }
        class SomeClass {}
        class SomeConvention : IConvention {}
    }
}
