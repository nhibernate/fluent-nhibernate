using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel
{
    [TestFixture]
    public class InverseOneToManyTester
    {
        [SetUp]
        public void SetUp()
        {
            var properties = new SQLiteConfiguration()
                .UseOuterJoin()
                .ShowSql()
                .InMemory()
                .ToProperties();

            _source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, new MusicPersistenceModel());
            _source.BuildSchema();
        }

        private ISessionSource _source;

        [Test]
        public void Should_handle_inverse_collections()
        {
            var artists = new List<Artist>
            {
                new Artist {Name = "Artist1"},
                new Artist {Name = "Artist2"}
            };

            new PersistenceSpecification<Genre>(_source)
                .CheckProperty(g => g.Id, 1L)
                .CheckProperty(g => g.Name, "Genre")
                .CheckComponentList(g => g.Artists, artists, (g, a) => g.AddArtist(a))
                .VerifyTheMappings();
        }
    }

    public class MusicPersistenceModel : PersistenceModel
    {
        public MusicPersistenceModel()
        {
            Add(typeof(ArtistMap));
            Add(typeof(GenreMap));
        }
    }

    public class ArtistMap : ClassMap<Artist>
    {
        public ArtistMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.Name);

            References(a => a.Genre)
                .Column("GenreID")
                .Not.Nullable();
        }
    }

    public class GenreMap : ClassMap<Genre>
    {
        public GenreMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.Name);

            HasMany(g => g.Artists)
                .KeyColumn("GenreID")
                .Cascade.All()
                .Inverse();
        }
    }
}