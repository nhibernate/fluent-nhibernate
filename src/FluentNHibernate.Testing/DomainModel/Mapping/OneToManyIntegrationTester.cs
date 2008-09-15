using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class OneToManyIntegrationTester
    {
        private class OneToManyPersistenceModel : PersistenceModel
        {
            public override void Configure(NHibernate.Cfg.Configuration configuration)
            {
                addMapping(new ChildObjectMap());
                addMapping(new OneToManyTargetMap());
                base.Configure(configuration);
            }

            private class ChildObjectMap : FluentNHibernate.Mapping.ClassMap<ChildObject>
            {
                public ChildObjectMap()
                {
                    Id(x => x.Id);
                }
            }

            private class OneToManyTargetMap : ClassMap<OneToManyTarget>
            {
                public OneToManyTargetMap()
                {
                    Id(x => x.Id);
                    HasMany<ChildObject>(x => x.ListOfChildren).AsList();
                    HasMany<ChildObject>(x => x.BagOfChildren).AsBag();
                    HasMany<ChildObject>(x => x.SetOfChildren).AsSet();
                    HasMany<ChildObject>(x => x.MapOfChildren).AsMap( x => x.Name);
                    HasMany<ChildObject>(x => x.ArrayOfChildren).AsArray(x => x.Position);
                }
            }
        }

        [Test]
        public void NHibernateCanLoadOneToManyTargetMapping()
        {
            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ConfigureProperties(new Configuration());

            var model = new OneToManyPersistenceModel();
            model.Configure(cfg);

            cfg.BuildSessionFactory();
        }
    }
}
