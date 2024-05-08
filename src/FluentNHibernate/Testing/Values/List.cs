using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

public class List<T, TListElement>(Accessor property, IEnumerable<TListElement> value)
    : Property<T, IEnumerable<TListElement>>(property, value)
{
    Action<T, Accessor, IEnumerable<TListElement>> _valueSetter;

    public override Action<T, Accessor, IEnumerable<TListElement>> ValueSetter
    {
        get
        {
            if (_valueSetter is not null)
            {
                return _valueSetter;
            }

            return (target, propertyAccessor, value) =>
            {
                propertyAccessor.SetValue(target, CreateCollection(propertyAccessor.PropertyType));
            };
        }
        set => _valueSetter = value;
    }

    protected IEnumerable<TListElement> Expected { get; } = value;

    public override void CheckValue(object target)
    {
        var actual = PropertyAccessor.GetValue(target) as IEnumerable<TListElement>;
        AssertGenericListMatches(actual, Expected);
    }

    void AssertGenericListMatches(IEnumerable<TListElement> actualEnumerable, IEnumerable<TListElement> expectedEnumerable)
    {
        if (actualEnumerable is null)
        {
            throw new ArgumentNullException(nameof(actualEnumerable),
                "Actual and expected are not equal (actual was null).");
        }
        if (expectedEnumerable is null)
        {
            throw new ArgumentNullException(nameof(expectedEnumerable),
                "Actual and expected are not equal (expected was null).");
        }

        var actualList = actualEnumerable.ToList();
        var expectedList = expectedEnumerable.ToList();

        if (actualList.Count != expectedList.Count)
        {
            throw new ApplicationException($"Actual count ({actualList.Count}) does not equal expected count ({expectedList.Count})");
        }

        Func<object, object, bool> equalsFunc = EntityEqualityComparer is not null
            ? EntityEqualityComparer.Equals
            : Equals;

        for (var i = 0; i < actualList.Count; i++)
        {
            if (equalsFunc(actualList[i], expectedList[i]))
            {
                continue;
            }

            var message = $"Expected '{expectedList[i]}' but got '{actualList[i]}' at position {i}";
            throw new ApplicationException(message);
        }
    }

    IEnumerable<TListElement> CreateCollection(Type type)
    {
        // sorry guys - create an instance of the collection type because we can't rely
        // on the user to pass in the correct collection type (especially if they're using
        // an interface). I've tried to create the common ones, but I'm sure this won't be
        // infallible.
        if (type.IsAssignableFrom(typeof(ISet<TListElement>)))
        {
            return new HashSet<TListElement>(Expected);
        }

        if (type.IsArray)
        {
            return Expected.ToArray();
        }

        return Expected.ToList();
    }
}
