using System;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;
using Rhino.Mocks;

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
            var firstListener = Mock<IDiagnosticListener>.Create();
            var secondListener = Mock<IDiagnosticListener>.Create();
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);

            dispatcher.RegisterListener(firstListener);
            dispatcher.RegisterListener(secondListener);
            dispatcher.Publish(results);

            firstListener.AssertWasCalled(x => x.Receive(results));
            secondListener.AssertWasCalled(x => x.Receive(results));
        }
    }
}
