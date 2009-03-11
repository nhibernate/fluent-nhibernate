using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.Fixtures;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing
{
    public class when_building_a_session_source : with_fluent_configuration
    {
        private ISessionSource _sessionSource;

        public override void establish_context()
        {
            _sessionSource = build_session_source();
        }

        [Test]
        public void should_be_able_to_get_a_new_session()
        {
            _sessionSource.CreateSession();
        }

        [Test]
        public void should_be_able_to_generate_the_schema()
        {
            _sessionSource.BuildSchema();
        }
    }

    public class when_using_a_session_source_and_schema : with_fluent_configuration
    {
        private ISessionSource _sessionSource;

        public override void establish_context()
        {
            _sessionSource = build_session_source();
            _sessionSource.BuildSchema();
        }

        [Test]
        public void should_be_able_to_use_the_fluent_mappings()
        {
            new PersistenceSpecification<Record>(_sessionSource)
                .CheckProperty(x => x.Name, "Luke Skywalker")
                .CheckProperty(x => x.Age, 18)
                .CheckProperty(x => x.Location, "Tatooine")
                .VerifyTheMappings();
        }
    }

    public class with_fluent_configuration : Specification
    {
        public SessionSource build_session_source()
        {
            FluentConfiguration config = Fluently.Configure()
                .Database(() => new SQLiteConfiguration().InMemory().UseOuterJoin().ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TestPersistenceModel>());

            return new SingleConnectionSessionSourceForSQLiteInMemoryTesting(config);
        }
    }
}