using System;

namespace FluentNHibernate.Mapping.Builders
{
    public class SortBuilder<T>
    {
        readonly T parent;
        readonly Action<string> setter;

        public SortBuilder(T parent, Action<string> setter)
        {
            this.parent = parent;
            this.setter = setter;
        }

        /// <summary>
        /// Specify natural sorting.
        /// </summary>
        public T Natural()
        {
            setter("natural");
            return parent;
        }

        /// <summary>
        /// Specify no sorting.
        /// </summary>
        /// <returns></returns>
        public T Unsorted()
        {
            setter("unsorted");
            return parent;
        }

        /// <summary>
        /// Specify sorting using a custom comparer.
        /// </summary>
        /// <typeparam name="TComparer">Comparer</typeparam>
        public T WithComparer<TComparer>()
        {
            return WithComparer(typeof(TComparer));
        }

        /// <summary>
        /// Specify sorting using a custom comparer.
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public T WithComparer(Type comparer)
        {
            return WithComparer(comparer.AssemblyQualifiedName);
        }

        /// <summary>
        /// Specify sorting using a custom comparer.
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public T WithComparer(string comparer)
        {
            setter(comparer);
            return parent;
        }
    }
}