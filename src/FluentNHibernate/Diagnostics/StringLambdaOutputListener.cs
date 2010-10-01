using System;

namespace FluentNHibernate.Diagnostics
{
    public class StringLambdaOutputListener : IDiagnosticListener
    {
        readonly Action<string> raiseMessage;
        IDiagnosticResultsFormatter outputFormatter = new DefaultOutputFormatter();

        public StringLambdaOutputListener(Action<string> raiseMessage)
        {
            this.raiseMessage = raiseMessage;
        }

        public void Receive(DiagnosticResults results)
        {
            var output = outputFormatter.Format(results);

            raiseMessage(output);
        }

        public void SetFormatter(IDiagnosticResultsFormatter formatter)
        {
            outputFormatter = formatter;
        }
    }
}