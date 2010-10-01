namespace FluentNHibernate.Diagnostics
{
    public interface IDiagnosticMessageDespatcher
    {
        void RegisterListener(IDiagnosticListener listener);
        void Publish(DiagnosticResults results);
    }
}