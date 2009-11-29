using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using System.Collections;

namespace FluentNHibernate.Testing.DomainModel.Access
{
    class ParentModel
    {

        public virtual int Id { get; private set; }

        public long Version { get; private set; }
        public virtual string Property { get; private set; }
        public virtual string JoinedProperty { get; private set; }
        public virtual ComponentModel Component { get; private set; }
        public virtual OneToOneModel One { get; private set; }
        public virtual object Any { get; private set; }
        public virtual IDictionary Dynamic { get; private set; }

        public virtual IDictionary<char, ManyToOneModel> MapOne { get; private set; }
        public virtual ISet<ManyToOneModel> SetOne { get; private set; }
        public virtual IList<ManyToOneModel> ListOne { get; private set; }
        public virtual IList<ManyToOneModel> BagOne { get; private set; }

        public virtual IDictionary<char, ManyToManyModel> MapMany { get; private set; }
        public virtual ISet<ManyToManyModel> SetMany { get; private set; }
        public virtual IList<ManyToManyModel> ListMany { get; private set; }
        public virtual IList<ManyToManyModel> BagMany { get; private set; }
    }
}
