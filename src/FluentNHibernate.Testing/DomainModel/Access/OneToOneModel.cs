using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Testing.DomainModel.Access
{
    class OneToOneModel
    {
        public ParentModel Parent { get; private set; }
    }
}
