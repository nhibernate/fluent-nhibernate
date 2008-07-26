using System;
using System.Linq.Expressions;
using ShadeTree.Core;
using StructureMap;

namespace ShadeTree.DomainModel.Fixtures
{
    public delegate T FinderDelegate<T, R>(R repository, string key);

    public static class DomainObjectFinder
    {
        private static readonly Cache<Type, object> _finders = new Cache<Type, object>();

        public static void ClearAllFinders()
        {
            _finders.ClearAll();
        }

        public static T Find<T>(string key)
        {
            var finder = (Func<string, T>) _finders.Get(typeof (T));
            return finder(key);
        }

        public static TypeFinderExpression<T> Type<T>() where T : class
        {
            return new TypeFinderExpression<T>(_finders);
        }
    }

    public class TypeFinderExpression<T> where T : class
    {
        private readonly Cache<Type, object> _finders;

        internal TypeFinderExpression(Cache<Type, object> finders)
        {
            _finders = finders;
        }

        public void IsFoundBy(Func<string, T> func)
        {
            _finders.Store(typeof (T), func);
        }

        public void IsFoundBy(Func<string, Expression<Func<T, bool>>> func)
        {
            Func<string, T> wrapped = s =>
            {
                var repository = ObjectFactory.GetInstance<IRepository>();
                var expression = func(s);
                return repository.FindBy(expression);
            };

            IsFoundBy(wrapped);
        }

        public void IsFoundByProperty(Expression<Func<T, string>> expression)
        {
            Func<string, T> function = key =>
            {
                var repository = ObjectFactory.GetInstance<IRepository>();
                return repository.FindBy(expression, key);
            };

            IsFoundBy(function);
        }


    }
}