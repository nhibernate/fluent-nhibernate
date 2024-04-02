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
        var actual = PropertyAccessor.GetValue(target) as IEnumerable;
        AssertGenericListMatches(actual, Expected);
    }

    void AssertGenericListMatches(IEnumerable actualEnumerable, IEnumerable<TListElement> expectedEnumerable)
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

        var actualList = actualEnumerable.Cast<object>().ToList();

        var expectedList = expectedEnumerable.ToList();

        if (actualList.Count != expectedList.Count)
        {
            throw new ApplicationException(String.Format("Actual count ({0}) does not equal expected count ({1})", actualList.Count, expectedList.Count));
        }

        var equalsFunc = (EntityEqualityComparer is not null)
            ? new Func<object, object, bool>((a, b) => EntityEqualityComparer.Equals(a, b))
            : new Func<object, object, bool>(Equals);


        var result = actualList.FirstOrDefault(item => actualList.Count(x => equalsFunc(item, x)) != expectedList.Count(x => equalsFunc(item, x)));
        if (result is not null)
        {
            throw new ApplicationException(String.Format("Actual count of item {0} ({1}) does not equal expected item count ({2})",result, actualList.Count(x => x == result), expectedList.Count(x => (object)x == result)));
        }
    }
}
