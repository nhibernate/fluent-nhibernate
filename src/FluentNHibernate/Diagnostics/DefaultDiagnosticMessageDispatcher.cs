using System;
using System.Collections.Generic;

namespace FluentNHibernate.Diagnostics
{
    public class DefaultDiagnosticMessageDispatcher : IDiagnosticMessageDispatcher
    {
        readonly List<IDiagnosticListener> listeners = new List<IDiagnosticListener>();

        public void RegisterListener(IDiagnosticListener listener)
        {
            listeners.Add(listener);
        }

        public void Publish(DiagnosticResults results)
        {
            foreach (var listener in listeners)
                listener.Receive(results);
        }
    }
}