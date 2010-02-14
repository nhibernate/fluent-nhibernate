using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.DomainModel.Access.Mappings
{
    class ParentModelMapping : ClassMap<ParentModel>
    {
        public ParentModelMapping()
        {
            Id(x => x.Id);
            Version(x => x.Version);
            Map(x => x.Property);

            Join("Joined", j =>
            {
                j.KeyColumn("Id");
                j.Map(x => x.JoinedProperty);
            });

            Component(x => x.Component, c =>
            {
                c.Map(x => x.Value);
            });

            HasOne(x => x.One);

            ReferencesAny(x => x.Any)
                .EntityTypeColumn("anytype")
                .EntityIdentifierColumn("anyid")
                .IdentityType(typeof(int));

            DynamicComponent(x => x.Dynamic, x => { });

            HasMany(x => x.MapOne).AsMap("type");
            HasMany(x => x.SetOne).AsSet();
            HasMany(x => x.ListOne).AsList();
            HasMany(x => x.BagOne).AsBag();

            HasManyToMany(x => x.MapMany).AsMap("type");
            HasManyToMany(x => x.SetMany).AsSet();
            HasManyToMany(x => x.ListMany).AsList();
            HasManyToMany(x => x.BagMany).AsBag();
        }
    }
}
