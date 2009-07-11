using System;
using System.Collections;
using System.Reflection;

namespace FluentNHibernate.Testing.Values
{
	public abstract class Property<T>
	{
		public abstract void SetValue(T target);
		public abstract void CheckValue(object target);

		public virtual void HasRegistered(PersistenceSpecification<T> specification)
		{}

		public IEqualityComparer EntityEqualityComparer { get; set; }
	}

	public class Property<T, TProperty> : Property<T>
	{
		private static readonly Action<T, PropertyInfo, TProperty> DefaultValueSetter = (target, propertyInfo, value) => propertyInfo.SetValue(target, value, null);
		private readonly PropertyInfo _propertyInfo;
		private readonly TProperty _value;
		private Action<T, PropertyInfo, TProperty> _valueSetter;

		public Property(PropertyInfo property, TProperty value)
		{
			_propertyInfo = property;
			_value = value;
		}

		public virtual Action<T, PropertyInfo, TProperty> ValueSetter
		{
			get
			{
				if (_valueSetter != null)
				{
					return _valueSetter;
				}

				return DefaultValueSetter;
			}
			set { _valueSetter = value; }
		}

		protected PropertyInfo PropertyInfo
		{
			get { return _propertyInfo; }
		}

		protected TProperty Value
		{
			get { return _value; }
		}

		public override void SetValue(T target)
		{
			try
			{
				ValueSetter(target, PropertyInfo, Value);
			}
			catch (Exception e)
			{
				string message = "Error while trying to set property " + _propertyInfo.Name;
				throw new ApplicationException(message, e);
			}
		}

		public override void CheckValue(object target)
		{
			object actual = _propertyInfo.GetValue(target, null);

			bool areEqual;
			if (EntityEqualityComparer != null)
			{
				areEqual = EntityEqualityComparer.Equals(_value, actual);
			}
			else
			{
				areEqual = _value.Equals(actual);
			}

			if (!areEqual)
			{
				string message =
					String.Format(
						"Expected '{0}' but got '{1}' for Property '{2}'",
						_value,
						actual, _propertyInfo.Name);

				throw new ApplicationException(message);
			}
		}
	}
}