using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ShadeTree.Core;
using System.Linq;

namespace ShadeTree.DomainModel
{
    public class InMemoryRepository : IRepository
    {
        private readonly Cache<Type, object> _types;

        public InMemoryRepository()
        {
            _types = new Cache<Type, object>(type =>
            {
                Type listType = typeof (List<>).MakeGenericType(type);
                return Activator.CreateInstance(listType);
            });
        }

        private IList<T> listFor<T>()
        {
            return (IList<T>) _types.Get(typeof (T));
        }

        public T Find<T>(long id) where T : Entity
        {
            return listFor<T>().First(t => t.Id == id);
        }

        public void Delete<T>(T target)
        {
            listFor<T>().Remove(target);
        }

        public T[] Query<T>(Expression<Func<T, bool>> where)
        {
            var query = from item in listFor<T>() select item;
            return query.Where(where.Compile()).ToArray();
        }

        public T FindBy<T, U>(Expression<Func<T, U>> expression, U search) where T : class
        {
            PropertyInfo accessor = ReflectionHelper.GetProperty(expression);
            Func<T, bool> predicate = t =>
            {
                U actual = (U) accessor.GetValue(t, null);
                return search.Equals(actual);
            };

            return listFor<T>().First(predicate);
        }

        public T FindBy<T>(Expression<Func<T, bool>> where)
        {
            var query = from item in listFor<T>() select item;
            return query.First(where.Compile());
        }

        public void Save<T>(T target)
        {
            listFor<T>().Add(target);
        }
    }
}