using System;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class StringLambdaOutputListenerTests
    {
        [Test]
        public void should_format_results()
        {
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);
            var formatter = Mock<IDiagnosticResultsFormatter>.Create();
            var listener = new StringLambdaOutputListener(x => { });
            listener.SetFormatter(formatter);
            listener.Receive(results);

            formatter.AssertWasCalled(x => x.Format(results));
        }

        [Test]
        public void should_raise_formatted_results()
        {
            var results = new DiagnosticResults(new ScannedSource[0], new Type[0], new Type[0], new SkippedAutomappingType[0], new Type[0], new AutomappingType[0]);
            var output = "formatted output";
            var receivedOutput = "";
            var formatter = Stub<IDiagnosticResultsFormatter>.Create(sb =>
                sb.Stub(x => x.Format(results))
                    .Return(output));
            var listener = new StringLambdaOutputListener(x => { receivedOutput = x; });
            listener.SetFormatter(formatter);
            listener.Receive(results);

            receivedOutput.ShouldEqual(output);
        }
    }
}
