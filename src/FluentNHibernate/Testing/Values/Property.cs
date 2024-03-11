using System;
using System.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

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
    private Action<T, Accessor, TProperty> _valueSetter;

    public Property(Accessor property, TProperty value)
    {
        PropertyAccessor = property;
        Value = value;
    }

    public virtual Action<T, Accessor, TProperty> ValueSetter
    {
        get
        {
            if (_valueSetter is not null)
            {
                return _valueSetter;
            }

            return DefaultValueSetter;
        }
        set => _valueSetter = value;
    }

    protected Accessor PropertyAccessor { get; }

    protected TProperty Value { get; }

    public override void SetValue(T target)
    {
        try
        {
            ValueSetter(target, PropertyAccessor, Value);
        }
        catch (Exception e)
        {
            string message = "Error while trying to set property " + PropertyAccessor.Name;
            throw new ApplicationException(message, e);
        }
    }

    public override void CheckValue(object target)
    {
        object actual = PropertyAccessor.GetValue(target);

        bool areEqual;
            
        if (EntityEqualityComparer is not null)
            areEqual = EntityEqualityComparer.Equals(Value, actual);
        else if (Value is null)
            areEqual = actual is null;
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

        string actualToPrint = actual is not null ? actual.ToString() : "(null)";
        string actualTypeToPrint = PropertyAccessor.PropertyType.FullName;

        string valueToPrint = Value is not null ? Value.ToString() : "(null)";
        string valueTypeToPrint = Value is not null ? Value.GetType().FullName : "(null)";

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
