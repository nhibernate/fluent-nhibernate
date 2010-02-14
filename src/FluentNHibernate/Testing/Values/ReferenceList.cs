using System.Collections.Generic;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Testing.Values
{
    public class ReferenceList<T, TListElement> : List<T, TListElement>
    {
        public ReferenceList(Accessor property, IEnumerable<TListElement> value)
            : base(property, value)
        {}

        public override void HasRegistered(PersistenceSpecification<T> specification)
        {
            foreach (TListElement item in Expected)
            {
                specification.TransactionalSave(item);
            }
        }
    }
}