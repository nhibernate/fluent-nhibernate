using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ManyToManyIntegrationTester
    {
        private class ManyToManyPersistenceModel : PersistenceModel
        {
            public override void Configure(Configuration configuration)
            {
                addMapping(new ChildObjectMap());
                addMapping(new ManyToManyTargetMap());
                base.Configure(configuration);
            }

            private class ChildObjectMap : ClassMap<ChildObject>
            {
                public ChildObjectMap()
                {
                    Id(x => x.Id);
                }
            }

            private class ManyToManyTargetMap : ClassMap<ManyToManyTarget>
            {
                public ManyToManyTargetMap()
                {
                    Id(x => x.Id);
                    HasManyToMany<ChildObject>(x => x.GetOtherChildren()).AsBag().Access.AsCamelCaseField();
                }
            }
        }

        [Test]
        public void NHibernateCanLoadOneToManyTargetMapping()
        {
            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ConfigureProperties(new Configuration());

            var model = new ManyToManyPersistenceModel();
            model.Configure(cfg);

            cfg.BuildSessionFactory();
        }
    }
}
