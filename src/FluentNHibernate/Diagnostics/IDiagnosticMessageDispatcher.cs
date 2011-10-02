namespace FluentNHibernate.Diagnostics
{
    public interface IDiagnosticMessageDispatcher
    {
        void RegisterListener(IDiagnosticListener listener);
        void Publish(DiagnosticResults results);
    }
}