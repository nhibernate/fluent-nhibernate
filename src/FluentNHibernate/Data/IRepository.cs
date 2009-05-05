using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace FluentNHibernate.Data
{
    public interface IRepository
    {
        T Find<T>(long id) where T : Entity;
        void Delete<T>(T target);
        T[] Query<T>(Expression<Func<T, bool>> where);
        T FindBy<T, TResult>(Expression<Func<T, TResult>> expression, TResult search) where T : class;
        T FindBy<T>(Expression<Func<T, bool>> where);
        void Save<T>(T target);
    }

    public class Repository : IRepository
    {
        private readonly ISession session;

        public Repository(ISessionSource source) : this(source.CreateSession())
        {
            
        }


        public Repository(ISession session)
        {
            this.session = session;
            this.session.FlushMode = FlushMode.Commit;
        }

        #region IRepository Members

        private void WithinTransaction(Action action)
        {
            ITransaction transaction = session.BeginTransaction();
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
            return session.Get<T>(id);
        }

        public void Delete<T>(T target)
        {
            WithinTransaction(() => session.Delete(target));
        }

        public T[] Query<T>(Expression<System.Func<T, bool>> where)
        {
            return session.Linq<T>().Where(where).ToArray();
        }

        #endregion

        public void Save<T>(T target)
        {
            WithinTransaction(() => session.SaveOrUpdate(target));
        }

        public T FindBy<T, TResult>(Expression<Func<T, TResult>> expression, TResult search) where T : class
        {
            string propertyName = ReflectionHelper.GetProperty(expression).Name;
            var criteria = session.CreateCriteria(typeof (T)).Add(Restrictions.Eq(propertyName, search));
            return criteria.UniqueResult() as T;
        }
        
        public T FindBy<T>(Expression<Func<T, bool>> where)
        {
            return session.Linq<T>().First(where);
        }
    }
}