using System;
using System.Linq;
using FluentNHibernate.BackwardCompatibility;
using FluentNHibernate.Cfg;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.MappingModel;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Testing.BackwardCompatibility
{
    [TestFixture]
    public class MusicMappingTester
    {
        private class ArtistMap : ClassMap<Artist>
        {
            public ArtistMap()
            {
                Id(x => x.ID);
                Map(x => x.Name)
                    .WithLengthOf(50)
                    .CanNotBeNull();
                HasMany<Album>(x => x.Albums)
                    .AsSet()
                    .IsInverse();
            }
        }

        private class AlbumMap : ClassMap<Album>
        {
            public AlbumMap()
            {
                Id(x => x.ID);
                Map(x => x.Title)
                    .WithLengthOf(50)
                    .CanNotBeNull();
                References(x => x.Artist);
                HasMany<Track>(x => x.Tracks)
                    .AsSet()
                    .IsInverse();
            }
        }

        private class TrackMap : ClassMap<Track>
        {
            public TrackMap()
            {
                Id(x => x.ID);
                Map(x => x.Name)
                    .WithLengthOf(50)
                    .CanNotBeNull();
                Map(x => x.TrackNumber);
                References(x => x.Album)
                    .CanNotBeNull();
            }
        }

        [Test]
        public void CanMapArtist()
        {
            var artistMap = new ArtistMap();
            ClassMapping mapping = artistMap.GetClassMapping();

            PropertyMapping prop = mapping.Properties.First();
            prop.Name.ShouldEqual("Name");
            prop.Length.ShouldEqual(50);
            prop.AllowNull.ShouldBeFalse();

            ICollectionMapping col = mapping.Collections.First();
            col.Name.ShouldEqual("Albums");
            col.ShouldBeOfType(typeof(SetMapping));
            col.IsInverse.ShouldBeTrue();
        }

        [Test]
        public void CanMapAlbum()
        {
            var albumMap = new AlbumMap();
            ClassMapping mapping = albumMap.GetClassMapping();

            PropertyMapping prop = mapping.Properties.First();
            prop.Name.ShouldEqual("Title");
            prop.Length.ShouldEqual(50);
            prop.AllowNull.ShouldBeFalse();

            ManyToOneMapping reference = mapping.References.First();
            reference.Name.ShouldEqual("Artist");

            ICollectionMapping col = mapping.Collections.First();
            col.Name.ShouldEqual("Tracks");
            col.ShouldBeOfType(typeof(SetMapping));
            col.IsInverse.ShouldBeTrue();
        }

        [Test]
        public void CanMapTrack()
        {
            var trackMap = new TrackMap();
            ClassMapping mapping = trackMap.GetClassMapping();

            PropertyMapping nameProp = mapping.Properties.Where(p => p.Name == "Name").FirstOrDefault();
            nameProp.ShouldNotBeNull();
            nameProp.Length.ShouldEqual(50);
            nameProp.AllowNull.ShouldBeFalse();

            PropertyMapping numberProp = mapping.Properties.Where(p => p.Name == "TrackNumber").FirstOrDefault();
            numberProp.ShouldNotBeNull();

            ManyToOneMapping reference = mapping.References.First();
            reference.Name.ShouldEqual("Album");
        }

        [Test]
        public void Music_xml_is_valid_against_schema()
        {
            var model = new PersistenceModel();
            model.Add(new ArtistMap());
            model.Add(new AlbumMap());
            model.Add(new TrackMap());

            model.BuildHibernateMapping().ShouldBeValidAgainstSchema();
        }

        [Test]
        public void Should_allow_music_entities_to_be_saved()
        {
            var model = new PersistenceModel();
            model.Add(new ArtistMap());
            model.Add(new AlbumMap());
            model.Add(new TrackMap());

            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ShowSql()
                .ConfigureProperties(new Configuration());

            model.Configure(cfg);

            var sessionFactory = cfg.BuildSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {                
                using (var tx = session.BeginTransaction())
                {
                    SchemaExport export = new SchemaExport(cfg);
                    export.Execute(true, true, false, false, session.Connection, null);
                    tx.Commit();
                }

                using (var tx = session.BeginTransaction())
                {
                    var inflames = new Artist {Name = "In Flames"};
                    session.Save(inflames);

                    var whoracle = new Album {Title = "Whoracle"};
                    whoracle.Artist = inflames;                    
                    session.Save(whoracle);

                    var jotun = new Track {Name = "Jotun"};
                    jotun.Album = whoracle;
                    session.Save(jotun);

                    tx.Commit();
                }

            }

        }

    }
}