namespace FluentNHibernate.Diagnostics
{
    public interface IDiagnosticListener
    {
        void Receive(DiagnosticResults results);
    }
}