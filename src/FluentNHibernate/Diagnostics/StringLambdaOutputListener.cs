using System;

namespace FluentNHibernate.Diagnostics;

public class StringLambdaOutputListener(Action<string> raiseMessage) : IDiagnosticListener
{
    IDiagnosticResultsFormatter outputFormatter = new DefaultOutputFormatter();

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
