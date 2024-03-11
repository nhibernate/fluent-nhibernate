using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

public class List<T, TListElement> : Property<T, IEnumerable<TListElement>>
{
    private Action<T, Accessor, IEnumerable<TListElement>> _valueSetter;

    public List(Accessor property, IEnumerable<TListElement> value)
        : base(property, value)
    {
        Expected = value;
    }

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
                object collection;

                // sorry guys - create an instance of the collection type because we can't rely
                // on the user to pass in the correct collection type (especially if they're using
                // an interface). I've tried to create the common ones, but I'm sure this won't be
                // infallible.
                if (propertyAccessor.PropertyType.IsAssignableFrom(typeof(ISet<TListElement>)))
                {
                    collection = new HashSet<TListElement>(Expected.ToList());
                }
                else if (propertyAccessor.PropertyType.IsArray)
                {
                    collection = Array.CreateInstance(typeof(TListElement), Expected.Count());
                    Array.Copy((Array)Expected, (Array)collection, Expected.Count());
                }
                else
                {
                    collection = new List<TListElement>(Expected);
                }

                propertyAccessor.SetValue(target, collection);
            };
        }
        set => _valueSetter = value;
    }

    protected IEnumerable<TListElement> Expected { get; }

    public override void CheckValue(object target)
    {
        var actual = PropertyAccessor.GetValue(target) as IEnumerable;
        AssertGenericListMatches(actual, Expected);
    }

    private void AssertGenericListMatches(IEnumerable actualEnumerable, IEnumerable<TListElement> expectedEnumerable)
    {
        if (actualEnumerable is null)
        {
            throw new ArgumentNullException("actualEnumerable",
                "Actual and expected are not equal (actual was null).");
        }
        if (expectedEnumerable is null)
        {
            throw new ArgumentNullException("expectedEnumerable",
                "Actual and expected are not equal (expected was null).");
        }

        List<object> actualList = new List<object>();
        foreach (var item in actualEnumerable)
        {
            actualList.Add(item);
        }

        var expectedList = expectedEnumerable.ToList();

        if (actualList.Count != expectedList.Count)
        {
            throw new ApplicationException(String.Format("Actual count ({0}) does not equal expected count ({1})", actualList.Count, expectedList.Count));
        }

        var equalsFunc = (EntityEqualityComparer is not null) ? ((a, b) => EntityEqualityComparer.Equals(a, b)): new Func<object, object, bool>(Equals);

        for (var i = 0; i < actualList.Count; i++)
        {
            if (equalsFunc(actualList[i], expectedList[i]))
            {
                continue;
            }

            var message = String.Format("Expected '{0}' but got '{1}' at position {2}",
                expectedList[i],
                actualList[i],
                i);

            throw new ApplicationException(message);
        }
    }
}
