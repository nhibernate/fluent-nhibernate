using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings
{
    class ManyToOneModelMapping : ClassMap<ManyToOneModel>
    {
        public ManyToOneModelMapping()
        {
            Id(x => x.Id);
            References(x => x.Parent);
        }
    }
}
