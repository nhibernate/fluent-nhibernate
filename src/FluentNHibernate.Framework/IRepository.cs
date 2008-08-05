using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Framework;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using ShadeTree.Core;
using Action=System.Action;

namespace FluentNHibernate.Framework
{
    public interface IRepository
    {
        T Find<T>(long id) where T : Entity;
        void Delete<T>(T target);
        T[] Query<T>(Expression<Func<T, bool>> where);
        T FindBy<T, U>(Expression<Func<T, U>> expression, U search) where T : class;
        T FindBy<T>(Expression<Func<T, bool>> where);
        void Save<T>(T target);
    }

    public class Repository : IRepository
    {
        private readonly ISession _session;

        public Repository(ISessionSource source) : this(source.CreateSession())
        {
            
        }


        public Repository(ISession session)
        {
            _session = session;
            _session.FlushMode = FlushMode.Commit;
        }

        #region IRepository Members

        private void withinTransaction(Action action)
        {
            ITransaction transaction = _session.BeginTransaction();
            try
            {
                action();
                transaction.Commit();
            }
            catch (Exception)
            {
                // Do something here?
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public T Find<T>(long id) where T : Entity
        {
            return _session.Get<T>(id);
        }

        public void Delete<T>(T target)
        {
            withinTransaction(() => _session.Delete(target));
        }

        public T[] Query<T>(Expression<System.Func<T, bool>> where)
        {
            return _session.Linq<T>().Where(where).ToArray();
        }

        #endregion

        public void Save<T>(T target)
        {
            withinTransaction(() => _session.SaveOrUpdate(target));
        }

        public T FindBy<T, U>(Expression<System.Func<T, U>> expression, U search) where T : class
        {
            string propertyName = ReflectionHelper.GetProperty(expression).Name;
            var criteria = _session.CreateCriteria(typeof (T)).Add(Restrictions.Eq(propertyName, search));
            return criteria.UniqueResult() as T;
        }
        
        public T FindBy<T>(Expression<System.Func<T, bool>> where)
        {
            return _session.Linq<T>().First(where);
        }
    }
}