using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Testing.Values;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing
{
	public static class PersistenceSpecificationExtensions
	{
		public static PersistenceSpecification<T> CheckProperty<T>(this PersistenceSpecification<T> spec,
		                                                           Expression<Func<T, object>> expression, object propertyValue)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			return spec.RegisterCheckedProperty(new Property<T, object>(property, propertyValue));
		}

		public static PersistenceSpecification<T> CheckProperty<T, TListElement>(this PersistenceSpecification<T> spec,
		                                                                         Expression<Func<T, Array>> expression,
		                                                                         IEnumerable<TListElement> propertyValue)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			return spec.RegisterCheckedProperty(new List<T, TListElement>(property, propertyValue));
		}

		public static PersistenceSpecification<T> CheckProperty<T, TProperty>(this PersistenceSpecification<T> spec,
		                                                                      Expression<Func<T, TProperty>> expression,
		                                                                      TProperty propertyValue,
		                                                                      Action<T, TProperty> propertySetter)
		{
			PropertyInfo propertyInfoFromExpression = ReflectionHelper.GetProperty(expression);

			var property = new Property<T, TProperty>(propertyInfoFromExpression, propertyValue);
			property.ValueSetter = (target, propertyInfo, value) => propertySetter(target, value);

			return spec.RegisterCheckedProperty(property);
		}

		public static PersistenceSpecification<T> CheckReference<T>(this PersistenceSpecification<T> spec,
		                                                            Expression<Func<T, object>> expression,
		                                                            object propertyValue)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			return spec.RegisterCheckedProperty(new ReferenceProperty<T, object>(property, propertyValue));
		}

		public static PersistenceSpecification<T> CheckReference<T, TProperty>(this PersistenceSpecification<T> spec,
		                                                                       Expression<Func<T, TProperty>> expression,
		                                                                       TProperty propertyValue,
		                                                                       Action<T, TProperty> propertySetter)
		{
			PropertyInfo propertyInfoFromExpression = ReflectionHelper.GetProperty(expression);

			var property = new ReferenceProperty<T, TProperty>(propertyInfoFromExpression, propertyValue);
			property.ValueSetter = (target, propertyInfo, value) => propertySetter(target, value);

			return spec.RegisterCheckedProperty(property);
		}

		public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
		                                                                     Expression<Func<T, IEnumerable<TListElement>>> expression,
		                                                                     IEnumerable<TListElement> propertyValue)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			return spec.RegisterCheckedProperty(new ReferenceList<T, TListElement>(property, propertyValue));
		}

		public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
		                                                                     Expression<Func<T, IEnumerable<TListElement>>> expression,
		                                                                     IEnumerable<TListElement> propertyValue,
		                                                                     Action<T, TListElement> listItemSetter)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			var list = new ReferenceList<T, TListElement>(property, propertyValue);
			list.ValueSetter = (target, propertyInfo, value) =>
			{
				foreach (var item in value)
				{
					listItemSetter(target, item);
				}
			};

			return spec.RegisterCheckedProperty(list);
		}

		public static PersistenceSpecification<T> CheckList<T, TListElement>(this PersistenceSpecification<T> spec,
		                                                                     Expression<Func<T, IEnumerable<TListElement>>> expression,
		                                                                     IEnumerable<TListElement> propertyValue,
		                                                                     Action<T, IEnumerable<TListElement>> listSetter)

		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			var list = new ReferenceList<T, TListElement>(property, propertyValue);
			list.ValueSetter = (target, propertyInfo, value) => listSetter(target, value);

			return spec.RegisterCheckedProperty(list);
		}

		/// <summary>
		/// Checks a list of components for validity.
		/// </summary>
		/// <typeparam name="TListElement">Type of list element</typeparam>
		/// <param name="expression">Property</param>
		/// <param name="propertyValue">Value to save</param>
		public static PersistenceSpecification<T> CheckComponentList<T, TListElement>(this PersistenceSpecification<T> spec,
		                                                                              Expression<Func<T, object>> expression,
		                                                                              IEnumerable<TListElement> propertyValue)
		{
			PropertyInfo property = ReflectionHelper.GetProperty(expression);

			return spec.RegisterCheckedProperty(new List<T, TListElement>(property, propertyValue));
		}

	    [Obsolete("CheckEnumerable has been replaced with CheckList")]
	    public static PersistenceSpecification<T> CheckEnumerable<T, TItem>(this PersistenceSpecification<T> spec,
                                                                            Expression<Func<T, IEnumerable<TItem>>> expression,
                                                                            Action<T, TItem> addAction,
                                                                            IEnumerable<TItem> itemsToAdd)
	    {
	        return spec.CheckList(expression, itemsToAdd, addAction);
	    }
	}
}