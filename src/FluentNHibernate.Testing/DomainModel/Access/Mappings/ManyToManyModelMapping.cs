using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings
{
    class ManyToManyModelMapping : ClassMap<ManyToManyModel>
    {
        public ManyToManyModelMapping()
        {
            Id(x => x.Id);
            HasManyToMany(x => x.Bag).AsBag();
        }
    }
}
