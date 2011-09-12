using System;
using System.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values
{
    public abstract class Property<T>
    {
        public IEqualityComparer EntityEqualityComparer { get; set; }
        public abstract void SetValue(T target);
        public abstract void CheckValue(object target);

        public virtual void HasRegistered(PersistenceSpecification<T> specification)
        {}
    }

    public class Property<T, TProperty> : Property<T>
    {
        private static readonly Action<T, Accessor, TProperty> DefaultValueSetter = (target, propertyAccessor, value) => propertyAccessor.SetValue (target, value);
        private readonly Accessor _propertyAccessor;
        private readonly TProperty _value;
        private Action<T, Accessor, TProperty> _valueSetter;

        public Property(Accessor property, TProperty value)
        {
            _propertyAccessor = property;
            _value = value;
        }

        public virtual Action<T, Accessor, TProperty> ValueSetter
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

        protected Accessor PropertyAccessor
        {
            get { return _propertyAccessor; }
        }

        protected TProperty Value
        {
            get { return _value; }
        }

        public override void SetValue(T target)
        {
            try
            {
                ValueSetter(target, PropertyAccessor, Value);
            }
            catch (Exception e)
            {
                string message = "Error while trying to set property " + _propertyAccessor.Name;
                throw new ApplicationException(message, e);
            }
        }

        public override void CheckValue(object target)
        {
            object actual = PropertyAccessor.GetValue(target);

            bool areEqual;
            
            if (EntityEqualityComparer != null)
                areEqual = EntityEqualityComparer.Equals(Value, actual);
            else if (Value == null)
                areEqual = actual == null;
            else
                areEqual = Value.Equals(actual);

            if (!areEqual)
            {
                throw new ApplicationException(GetInequalityComparisonMessage(actual));
            }
        }

        private string GetInequalityComparisonMessage(object actual)
        {
            string message;

            string actualToPrint = actual != null ? actual.ToString() : "(null)";
            string actualTypeToPrint = PropertyAccessor.PropertyType.FullName;

            string valueToPrint = Value != null ? Value.ToString() : "(null)";
            string valueTypeToPrint = Value != null ? Value.GetType().FullName : "(null)";

            if (actualToPrint != valueToPrint && actualTypeToPrint != valueTypeToPrint)
            {
                message =
                    String.Format(
                        "For property '{0}' expected '{1}' of type '{2}' but got '{3}' of type '{4}'",
                        PropertyAccessor.Name,
                        valueToPrint,
                        valueTypeToPrint,
                        actualToPrint,
                        actualTypeToPrint);
            }
            else if (actualToPrint != valueToPrint)
            {
                message =
                    String.Format(
                        "For property '{0}' of type '{1}' expected '{2}' but got '{3}'",
                        PropertyAccessor.Name,
                        actualTypeToPrint,
                        valueToPrint,
                        actualToPrint);
            }
            else if (actualTypeToPrint != valueTypeToPrint)
            {
                message =
                    String.Format(
                        "For property '{0}' expected type '{1}' but got '{2}'",
                        PropertyAccessor.Name,
                        valueTypeToPrint,
                        actualTypeToPrint);
            }
            else if (actualTypeToPrint != actualToPrint)
            {
                message =
                    String.Format(
                        "For property '{0}' expected same element, but got different element with the same value '{1}' of type '{2}'."
                        + Environment.NewLine + "Tip: use a CustomEqualityComparer when creating the PersistenceSpecification object.",
                        PropertyAccessor.Name,
                        actualToPrint,
                        actualTypeToPrint);
            }
            else
            {
                message =
                    String.Format(
                        "For property '{0}' expected same element, but got different element of type '{1}'."
                        + Environment.NewLine + "Tip: override ToString() on the type to find out the difference.",
                        PropertyAccessor.Name,
                        actualTypeToPrint);
            }

            return message;
        }
    }
}