namespace FluentNHibernate.Diagnostics
{
    public interface IDiagnosticResultsFormatter
    {
        string Format(DiagnosticResults results);
    }
}