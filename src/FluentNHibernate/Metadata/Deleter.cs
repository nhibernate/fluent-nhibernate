using System;
using System.Collections.Generic;
using System.Diagnostics;
using NHibernate;

namespace FluentNHibernate.Metadata
{
    public class Deleter : IDeleter
    {
        private readonly ISession _session;
        private readonly List<Type> _types = new List<Type>();
        private readonly List<Type> _latches = new List<Type>();

        public Deleter(ISession session)
        {
            _session = session;
        }

        #region IDeleter Members

        public void DeleteAll(Type type)
        {
            try
            {
                if (_types.Contains(type)) return;
                _session.Delete("from " + type.Name);
                _session.Flush();
                _types.Add(type);
            }
            catch (Exception e)
            {
                string message = string.Format("Error deleting {0}", type.FullName);
                Debug.WriteLine(message);
                Debug.WriteLine(e.ToString());
                throw;
            }
        }

        public void FollowDependencies(TypeDependency dependency)
        {
            if (!_latches.Contains(dependency.Type))
            {
                _latches.Add(dependency.Type);
                dependency.ForEach(d => d.DeleteAll(this));
            }
        }

        #endregion
    }
}