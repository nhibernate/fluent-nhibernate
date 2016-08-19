using System;
using FakeItEasy;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class DiagnosticMessageDispatcherTests
    {
        IDiagnosticMessageDispatcher dispatcher;

        [SetUp]
        public void CreateDispatcher()
        {
            dispatcher = new DefaultDiagnosticMessageDispatcher();
        }

        [Test]
        public void should_publish_results_to_all_listeners()
        {
            var firstListener = A.Fake<IDiagnosticListener>();
            var secondListener = A.Fake<IDiagnosticListener>();
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);

            dispatcher.RegisterListener(firstListener);
            dispatcher.RegisterListener(secondListener);
            dispatcher.Publish(results);

            A.CallTo(() => firstListener.Receive(results)).MustHaveHappened();
            A.CallTo(() => secondListener.Receive(results)).MustHaveHappened();
        }
    }
}
