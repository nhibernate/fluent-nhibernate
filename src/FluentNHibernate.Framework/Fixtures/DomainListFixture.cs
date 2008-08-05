using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using fit;

namespace FluentNHibernate.Framework.Fixtures
{
    public class DomainListFixture<T> : ColumnFixture where T : new()
    {
        private readonly List<T> _instances = new List<T>();
        protected T subject;
        private readonly Action<T[]> _doneAction = array => { };


        public DomainListFixture()
        {
            try
            {
                DomainFixtureWatcher.LoadingListOfType<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw;
            }
        }


        public DomainListFixture(Action<T[]> doneAction)
        {
            _doneAction = doneAction;
        }

        public T[] Instances
        {
            get { return _instances.ToArray(); }
        }

        public override void Reset()
        {
            subject = new T();
        }

        public override void Execute()
        {
            _instances.Add(subject);
            DomainFixtureWatcher.Added(subject);
        }


        public override void DoRows(Parse rows)
        {
            base.DoRows(rows);
            _doneAction(_instances.ToArray());
        }
    }
}