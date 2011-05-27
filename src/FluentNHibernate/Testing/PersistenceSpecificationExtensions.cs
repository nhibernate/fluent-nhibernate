using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;
using System.Collections;
using FluentNHibernate.Utils.Reflection;

namespace FluentNHibernate.Testing
{
    public static class PersistenceSpecificationExtensions
    {
        public static PersistenceSpecification<T> CheckProperty<T>(this PersistenceSpecification<T> spec,
                                                                   Expression<Func<T, object>> expression, object propertyValue)
        {
            return spec.CheckProperty(expression, propertyValue, (IEqualityComparer)null);
        }

        public static PersistenceSpecification<T> CheckProperty<T>(this PersistenceSpecification<T> spec,
                                                                    Expression<Func<T, object>> expression, object propertyValue,
                                                                    IEqualityComparer propertyComparer)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            return spec.RegisterCheckedProperty(new Property<T, object>(property, propertyValue), propertyComparer);
        }

        public static PersistenceSpecification<T> CheckProperty<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                 Expression<Func<T, Array>> expression,
                                                                                 IEnumerable<TListElement> propertyValue)
        {
            return spec.CheckProperty(expression, propertyValue, null);
        }

        public static PersistenceSpecification<T> CheckProperty<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                  Expression<Func<T, Array>> expression,
                                                                                  IEnumerable<TListElement> propertyValue,
                                                                                  IEqualityComparer elementComparer)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            return spec.RegisterCheckedProperty(new List<T, TListElement>(property, propertyValue), elementComparer);
        }

        public static PersistenceSpecification<T> CheckProperty<T, TProperty>(this PersistenceSpecification<T> spec,
                                                                               Expression<Func<T, TProperty>> expression,
                                                                               TProperty propertyValue,
                                                                               Action<T, TProperty> propertySetter)
        {
            return spec.CheckProperty(expression, propertyValue, null, propertySetter);
        }
        
        public static PersistenceSpecification<T> CheckProperty<T, TProperty>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, TProperty>> expression,
                                                                              TProperty propertyValue,
                                                                              IEqualityComparer propertyComparer,
                                                                              Action<T, TProperty> propertySetter)
        {
            Accessor propertyInfoFromExpression = ReflectionHelper.GetAccessor(expression);

            var property = new Property<T, TProperty>(propertyInfoFromExpression, propertyValue);
            property.ValueSetter = (target, propertyInfo, value) => propertySetter(target, value);

            return spec.RegisterCheckedProperty(property, propertyComparer);
        }

        public static PersistenceSpecification<T> CheckReference<T>(this PersistenceSpecification<T> spec,
                                                                     Expression<Func<T, object>> expression,
                                                                     object propertyValue)
        {
            return spec.CheckReference(expression, propertyValue, (IEqualityComparer)null);
        }

        public static PersistenceSpecification<T> CheckReference<T>(this PersistenceSpecification<T> spec,
                                                                     Expression<Func<T, object>> expression,
                                                                     object propertyValue,
                                                                     IEqualityComparer propertyComparer)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            return spec.RegisterCheckedProperty(new ReferenceProperty<T, object>(property, propertyValue), propertyComparer);
        }

        public static PersistenceSpecification<T> CheckReference<T, TReference>(this PersistenceSpecification<T> spec,
                                                                   Expression<Func<T, object>> expression,
                                                                   TReference propertyValue,
                                                                   params Func<TReference, object>[] propertiesToCompare)
        {
            // Because of the params keyword, the compiler will select this overload
            // instead of the one above, even when no funcs are supplied in the method call.
            if (propertiesToCompare == null || propertiesToCompare.Length == 0)
                return spec.CheckReference(expression, propertyValue, (IEqualityComparer)null);

            return spec.CheckReference(expression, propertyValue, new FuncEqualityComparer<TReference>(propertiesToCompare));
        }

        public static PersistenceSpecification<T> CheckReference<T, TProperty>(this PersistenceSpecification<T> spec,
                                                                                Expression<Func<T, TProperty>> expression,
                                                                                TProperty propertyValue,
                                                                                Action<T, TProperty> propertySetter)
        {
            return spec.CheckReference(expression, propertyValue, null, propertySetter);
        }
        
        public static PersistenceSpecification<T> CheckReference<T, TProperty>(this PersistenceSpecification<T> spec,
                                                                               Expression<Func<T, TProperty>> expression,
                                                                               TProperty propertyValue,
                                                                               IEqualityComparer propertyComparer,
                                                                               Action<T, TProperty> propertySetter)
        {
            Accessor propertyInfoFromExpression = ReflectionHelper.GetAccessor(expression);

            var property = new ReferenceProperty<T, TProperty>(propertyInfoFromExpression, propertyValue);
            property.ValueSetter = (target, propertyInfo, value) => propertySetter(target, value);

            return spec.RegisterCheckedProperty(property, propertyComparer);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue)
        
        {
            return spec.CheckList(expression, propertyValue, (IEqualityComparer)null);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue,
                                                                              IEqualityComparer elementComparer)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            return spec.RegisterCheckedProperty(new ReferenceList<T, TListElement>(property, propertyValue), elementComparer);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                            Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                            IEnumerable<TListElement> propertyValue,
                                                                            params Func<TListElement, object>[] propertiesToCompare)
        {
            // Because of the params keyword, the compiler can select this overload
            // instead of the one above, even when no funcs are supplied in the method call.
            if (propertiesToCompare == null || propertiesToCompare.Length == 0)
                return spec.CheckList(expression, propertyValue, (IEqualityComparer)null);

            return spec.CheckList(expression, propertyValue, new FuncEqualityComparer<TListElement>(propertiesToCompare));
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue,
                                                                              Action<T, TListElement> listItemSetter)
        {
            return spec.CheckList(expression, propertyValue, null, listItemSetter);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue,
                                                                              IEqualityComparer elementComparer,
                                                                              Action<T, TListElement> listItemSetter)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            var list = new ReferenceList<T, TListElement>(property, propertyValue);
            list.ValueSetter = (target, propertyInfo, value) =>
            {
                foreach(var item in value)
                {
                    listItemSetter(target, item);
                }
            };

            return spec.RegisterCheckedProperty(list, elementComparer);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue,
                                                                              Action<T, IEnumerable<TListElement>> listSetter)
        {
            return spec.CheckList(expression, propertyValue, null, listSetter);
        }

        public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                              Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                              IEnumerable<TListElement> propertyValue,
                                                                              IEqualityComparer elementComparer,
                                                                              Action<T, IEnumerable<TListElement>> listSetter)

        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            var list = new ReferenceList<T, TListElement>(property, propertyValue);
            list.ValueSetter = (target, propertyInfo, value) => listSetter(target, value);

            return spec.RegisterCheckedProperty(list, elementComparer);
        }

        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                      Expression<Func<T, object>> expression,
                                                                                      IEnumerable<TListElement> propertyValue)
        {
            return spec.CheckComponentList(expression, propertyValue, null);
        }

        /// <summary>
        /// Checks a list of components for validity.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TListElement">Type of list element</typeparam>
        /// <param name="spec">Persistence specification</param>
        /// <param name="expression">Property</param>
        /// <param name="propertyValue">Value to save</param>
        /// <param name="elementComparer">Equality comparer</param>
        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                      Expression<Func<T, object>> expression,
                                                                                      IEnumerable<TListElement> propertyValue,
                                                                                      IEqualityComparer elementComparer)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            return spec.RegisterCheckedProperty(new List<T, TListElement>(property, propertyValue), elementComparer);
        }

        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                       Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                                       IEnumerable<TListElement> propertyValue,
                                                                                       Action<T, TListElement> listItemSetter)
        {
            return spec.CheckComponentList(expression, propertyValue, null, listItemSetter);
        }

        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                       Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                                       IEnumerable<TListElement> propertyValue,
                                                                                       IEqualityComparer elementComparer,
                                                                                       Action<T, TListElement> listItemSetter)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            var list = new List<T, TListElement>(property, propertyValue);
            list.ValueSetter = (target, propertyInfo, value) => {
                foreach(var item in value) {
                    listItemSetter(target, item);
                }
            };

            return spec.RegisterCheckedProperty(list, elementComparer);
        }

        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                     Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                                     IEnumerable<TListElement> propertyValue,
                                                                                     Action<T, IEnumerable<TListElement>> listSetter)
        {
            return spec.CheckComponentList(expression, propertyValue, null, listSetter);
        }

        public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
                                                                                      Expression<Func<T, IEnumerable<TListElement>>> expression,
                                                                                      IEnumerable<TListElement> propertyValue,
                                                                                      IEqualityComparer elementComparer,
                                                                                      Action<T, IEnumerable<TListElement>> listSetter)
        {
            Accessor property = ReflectionHelper.GetAccessor(expression);

            var list = new List<T, TListElement>(property, propertyValue);
            list.ValueSetter = (target, propertyInfo, value) => listSetter(target, value);

            return spec.RegisterCheckedProperty(list, elementComparer);
        }

        [Obsolete("CheckEnumerable has been replaced with CheckList")]
        public static PersistenceSpecification<T> CheckEnumerable<T, TItem>(this PersistenceSpecification<T> spec,
                                                                            Expression<Func<T, IEnumerable<TItem>>> expression,
                                                                            Action<T, TItem> addAction,
                                                                            IEnumerable<TItem> itemsToAdd)
        {
            return spec.CheckList(expression, itemsToAdd, addAction);
        }

        private class FuncEqualityComparer<T> : EqualityComparer<T>
        {
            readonly IEnumerable<Func<T, object>> comparisons;

            public FuncEqualityComparer(IEnumerable<Func<T, object>> comparisons)
            {
                this.comparisons = comparisons;
            }

            public override bool Equals(T x, T y)
            {
                return comparisons.All(func => object.Equals(func(x), func(y)));
            }

            public override int GetHashCode(T obj)
            {
                throw new NotSupportedException();
            }
        }
    }
}
