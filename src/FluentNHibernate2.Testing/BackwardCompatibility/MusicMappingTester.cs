using System;
using System.Linq;
using FluentNHibernate.BackwardCompatibility;
using FluentNHibernate.Cfg;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.MappingModel;
using NHibernate.Cfg;
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
            var mapping = artistMap.GetClassMapping();

            var prop = mapping.Properties.First();
            prop.Name.ShouldEqual("Name");
            prop.Length.ShouldEqual(50);
            prop.AllowNull.ShouldBeFalse();

            var col = mapping.Collections.First();
            col.Name.ShouldEqual("Albums");
            col.ShouldBeOfType(typeof(SetMapping));
            col.Attributes.IsInverse.ShouldBeTrue();

            mapping.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void CanMapAlbum()
        {
            var albumMap = new AlbumMap();
            var mapping = albumMap.GetClassMapping();

            var prop = mapping.Properties.First();
            prop.Name.ShouldEqual("Title");
            prop.Length.ShouldEqual(50);
            prop.AllowNull.ShouldBeFalse();

            var reference = mapping.References.First();
            reference.Name.ShouldEqual("Artist");

            var col = mapping.Collections.First();
            col.Name.ShouldEqual("Tracks");
            col.ShouldBeOfType(typeof(SetMapping));
            col.Attributes.IsInverse.ShouldBeTrue();
        }

        [Test]
        public void CanMapTrack()
        {
            var trackMap = new TrackMap();
            var mapping = trackMap.GetClassMapping();

            var nameProp = mapping.Properties.Where(p => p.Name == "Name").FirstOrDefault();
            nameProp.ShouldNotBeNull();
            nameProp.Length.ShouldEqual(50);
            nameProp.AllowNull.ShouldBeFalse();

            var numberProp = mapping.Properties.Where(p => p.Name == "TrackNumber").FirstOrDefault();
            numberProp.ShouldNotBeNull();

            var reference = mapping.References.First();
            reference.Name.ShouldEqual("Album");
        }

        [Test]
        public void CanConfigureNHibernateWithMusicMappings()
        {
            var model = new PersistenceModel();
            model.Add(new ArtistMap());
            model.Add(new AlbumMap());
            model.Add(new TrackMap());


            var cfg = new SQLiteConfiguration()
                .InMemory()
                .ConfigureProperties(new Configuration());

            model.Configure(cfg);

            cfg.BuildSessionFactory();
        }

    }
}