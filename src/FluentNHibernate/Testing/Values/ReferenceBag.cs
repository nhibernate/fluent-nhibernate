using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

public class ReferenceBag<T, TListElement>(Accessor property, IEnumerable<TListElement> value)
    : ReferenceList<T, TListElement>(property, value)
{
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

        var result = actualList.FirstOrDefault(item => actualList.Count(x => equalsFunc(item, x)) != expectedList.Count(x => equalsFunc(item, x)));
        if (result is not null)
        {
            throw new ApplicationException($"Actual count of item {result} ({actualList.Count(x => ReferenceEquals(x, result))}) does not equal expected item count ({expectedList.Count(x => ReferenceEquals(x, result))})");
        }
    }
}
