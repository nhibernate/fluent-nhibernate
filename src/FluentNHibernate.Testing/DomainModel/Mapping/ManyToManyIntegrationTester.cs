using FluentNHibernate.Mapping;
using NUnit.Framework;
using NHibernate.Cfg;
using static FluentNHibernate.Testing.Cfg.SQLiteFrameworkConfigurationFactory;

namespace FluentNHibernate.Testing.DomainModel.Mapping;

[TestFixture]
public class ManyToManyIntegrationTester
{
    class ManyToManyPersistenceModel : PersistenceModel
    {
        public override void Configure(Configuration configuration)
        {
            Add(new ChildObjectMap());
            Add(new ManyToManyTargetMap());
            base.Configure(configuration);
        }

        class ChildObjectMap : ClassMap<ChildObject>
        {
            public ChildObjectMap()
            {
                Id(x => x.Id);
            }
        }

        class ManyToManyTargetMap : ClassMap<ManyToManyTarget>
        {
            public ManyToManyTargetMap()
            {
                Id(x => x.Id);
                HasManyToMany(x => x.GetOtherChildren()).AsBag().Access.CamelCaseField();
            }
        }
    }

    [Test]
    public void NHibernateCanLoadOneToManyTargetMapping()
    {

        var cfg = CreateStandardInMemoryConfiguration()
            .ConfigureProperties(new Configuration());

        var model = new ManyToManyPersistenceModel();
        model.Configure(cfg);

        cfg.BuildSessionFactory();
    }
}
