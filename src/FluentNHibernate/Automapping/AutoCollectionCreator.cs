using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoCollectionCreator
    {
        public Collection DetermineCollectionType(Type type)
        {
            if ((type.Namespace ?? "").StartsWith("Iesi.Collections") || type.Closes(typeof(HashSet<>)))
                return Collection.Set;

            return Collection.Bag;
        }
    }
}