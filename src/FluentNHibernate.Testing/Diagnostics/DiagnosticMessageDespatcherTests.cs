using System;
using FakeItEasy;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics;

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
        var results = new DiagnosticResults(Array.Empty<ScannedSource>(), Array.Empty<Type>(), Array.Empty<Type>(), Array.Empty<SkippedAutomappingType>(), Array.Empty<Type>(), Array.Empty<AutomappingType>());

        dispatcher.RegisterListener(firstListener);
        dispatcher.RegisterListener(secondListener);
        dispatcher.Publish(results);

        A.CallTo(() => firstListener.Receive(results)).MustHaveHappened();
        A.CallTo(() => secondListener.Receive(results)).MustHaveHappened();
    }
}
