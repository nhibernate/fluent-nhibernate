using System;
using FakeItEasy;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class StringLambdaOutputListenerTests
    {
        [Test]
        public void should_format_results()
        {
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);
            var formatter = A.Fake<IDiagnosticResultsFormatter>();
            var listener = new StringLambdaOutputListener(x => { });
            listener.SetFormatter(formatter);
            listener.Receive(results);

            A.CallTo(() => formatter.Format(results)).MustHaveHappened();
        }

        [Test]
        public void should_raise_formatted_results()
        {
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);
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
}
