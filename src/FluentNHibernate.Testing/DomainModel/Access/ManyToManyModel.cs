
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.Testing.DomainModel.Access
{
    class ManyToManyModel
    {
        public virtual int Id { get; private set; }
        public IList<ParentModel> Bag { get; private set; }
    }
}
