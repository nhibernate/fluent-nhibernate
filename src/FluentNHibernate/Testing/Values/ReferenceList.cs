using System.Collections.Generic;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values;

public class ReferenceList<T, TListElement>(Accessor property, IEnumerable<TListElement> value)
    : List<T, TListElement>(property, value)
{
    public override void HasRegistered(PersistenceSpecification<T> specification)
    {
        foreach (TListElement item in Expected)
        {
            specification.TransactionalSave(item);
        }
    }
}
