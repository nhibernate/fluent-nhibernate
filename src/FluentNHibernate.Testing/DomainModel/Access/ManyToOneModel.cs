using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Testing.DomainModel.Access
{
    class ManyToOneModel
    {
        public virtual int Id { get; private set; }
        public virtual ParentModel Parent { get; private set; }
    }
}
