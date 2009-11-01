using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings
{
    class CompositeIdModelMapping : ClassMap<CompositeIdModel>
    {
        public CompositeIdModelMapping()
        {
            CompositeId()
                .KeyProperty(x => x.IdA)
                .KeyProperty(x => x.IdB);
        }
    }
}
