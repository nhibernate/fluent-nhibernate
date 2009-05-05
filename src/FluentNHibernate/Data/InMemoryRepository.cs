using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Data
{
    public class InMemoryRepository : IRepository
    {
        private readonly Cache<Type, object> types;

        public InMemoryRepository()
        {
            types = new Cache<Type, object>(type =>
            {
                Type listType = typeof (List<>).MakeGenericType(type);
                return Activator.CreateInstance(listType);
            });
        }

        private IList<T> ListFor<T>()
        {
            return (IList<T>) types.Get(typeof (T));
        }

        public T Find<T>(long id) where T : Entity
        {
            return ListFor<T>().First(t => t.Id == id);
        }

        public void Delete<T>(T target)
        {
            ListFor<T>().Remove(target);
        }

        public T[] Query<T>(Expression<Func<T, bool>> where)
        {
            var query = from item in ListFor<T>() select item;
            return query.Where(where.Compile()).ToArray();
        }

        public T FindBy<T, TResult>(Expression<Func<T, TResult>> expression, TResult search) where T : class
        {
            PropertyInfo accessor = ReflectionHelper.GetProperty(expression);
            Func<T, bool> predicate = t =>
            {
                var actual = (TResult) accessor.GetValue(t, null);
                return search.Equals(actual);
            };

            return ListFor<T>().First(predicate);
        }

        public T FindBy<T>(Expression<Func<T, bool>> where)
        {
            var query = from item in ListFor<T>() select item;
            return query.First(where.Compile());
        }

        public void Save<T>(T target)
        {
            ListFor<T>().Add(target);
        }
    }
}