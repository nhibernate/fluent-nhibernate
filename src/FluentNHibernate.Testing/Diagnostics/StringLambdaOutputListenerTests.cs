using System;
using FakeItEasy;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics;

[TestFixture]
public class StringLambdaOutputListenerTests
{
    [Test]
    public void should_format_results()
    {
        var results = new DiagnosticResults(Array.Empty<ScannedSource>(), Array.Empty<Type>(), Array.Empty<Type>(), Array.Empty<SkippedAutomappingType>(), Array.Empty<Type>(), Array.Empty<AutomappingType>());
        var formatter = A.Fake<IDiagnosticResultsFormatter>();
        var listener = new StringLambdaOutputListener(x => { });
        listener.SetFormatter(formatter);
        listener.Receive(results);

        A.CallTo(() => formatter.Format(results)).MustHaveHappened();
    }

    [Test]
    public void should_raise_formatted_results()
    {
        var results = new DiagnosticResults(Array.Empty<ScannedSource>(), Array.Empty<Type>(), Array.Empty<Type>(), Array.Empty<SkippedAutomappingType>(), Array.Empty<Type>(), Array.Empty<AutomappingType>());
        var output = "formatted output";
        var receivedOutput = "";
        var formatter = A.Fake<IDiagnosticResultsFormatter>();
        A.CallTo(() => formatter.Format(results)).Returns(output);
        var listener = new StringLambdaOutputListener(x => { receivedOutput = x; });
        listener.SetFormatter(formatter);
        listener.Receive(results);

        receivedOutput.ShouldEqual(output);
    }
}
