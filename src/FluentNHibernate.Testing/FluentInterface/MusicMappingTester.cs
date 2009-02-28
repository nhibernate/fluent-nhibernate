using System;
using System.Linq;
using FluentNHibernate.FluentInterface;
using FluentNHibernate.Cfg;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Reflection;
using FluentNHibernate.Testing.DomainModel;
using FluentNHibernate.Testing.MappingModel;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Conventions;

namespace FluentNHibernate.Testing.FluentInterface
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
                HasManyToMany<Tag>(x => x.Tags)
                    .AsSet();
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

        private class TagMap : ClassMap<Tag>
        {
            public TagMap()
            {
                Id(x => x.ID);
                Map(x => x.Description);
            }
        }

        [Test]
        public void CanMapArtist()
        {
            var artistMap = new ArtistMap();
            ClassMapping mapping = artistMap.GetClassMapping();

            PropertyMapping prop = mapping.Properties.First();
            prop.PropertyInfo.ShouldNotBeNull();
            prop.Length.ShouldEqual(50);
            prop.IsNotNullable.ShouldBeTrue();

            ICollectionMapping col = mapping.Collections.First();
            col.PropertyInfo.ShouldNotBeNull();
            col.ShouldBeOfType(typeof(SetMapping));
            col.IsInverse.ShouldBeTrue();
        }

        [Test]
        public void CanMapAlbum()
        {
            var albumMap = new AlbumMap();
            ClassMapping mapping = albumMap.GetClassMapping();

            PropertyMapping prop = mapping.Properties.First();
            prop.PropertyInfo.ShouldNotBeNull();
            prop.Length.ShouldEqual(50);
            prop.IsNotNullable.ShouldBeTrue();

            ManyToOneMapping reference = mapping.References.First();
            reference.PropertyInfo.ShouldEqual(ReflectionHelper.GetProperty<Album>(x => x.Artist));

            ICollectionMapping tracks = mapping.Collections.First();
            tracks.PropertyInfo.ShouldNotBeNull();
            tracks.ShouldBeOfType(typeof(SetMapping));
            tracks.Contents.ShouldBeOfType(typeof(OneToManyMapping));
            tracks.IsInverse.ShouldBeTrue();

            ICollectionMapping tags = mapping.Collections.Skip(1).First();
            tags.PropertyInfo.ShouldNotBeNull();
            tags.ShouldBeOfType(typeof (SetMapping));
            tags.Contents.ShouldBeOfType(typeof(ManyToManyMapping));            
        }

        [Test]
        public void CanMapTrack()
        {
            var trackMap = new TrackMap();
            ClassMapping mapping = trackMap.GetClassMapping();

            PropertyMapping nameProp = mapping.Properties.Where(p => p.PropertyInfo.Name == "Name").FirstOrDefault();
            nameProp.ShouldNotBeNull();
            nameProp.Length.ShouldEqual(50);
            nameProp.IsNotNullable.ShouldBeTrue();

            PropertyMapping numberProp = mapping.Properties.Where(p => p.PropertyInfo.Name == "TrackNumber").FirstOrDefault();
            numberProp.ShouldNotBeNull();

            ManyToOneMapping reference = mapping.References.First();
            reference.PropertyInfo.ShouldEqual(ReflectionHelper.GetProperty<Track>(x => x.Album));
        }

        [Test]
        public void Music_xml_is_valid_against_schema()
        {
            var model = new PersistenceModel();
            model.Add(new ArtistMap());
            model.Add(new AlbumMap());
            model.Add(new TrackMap());
            model.Add(new TagMap());

            model.AddConvention(new NamingConvention());
            var hibernateMapping = model.BuildHibernateMapping();
            model.ApplyVisitors(hibernateMapping);
            hibernateMapping.ShouldBeValidAgainstSchema();
        }

        [Test]
        public void Should_allow_music_entities_to_be_saved()
        {
            var integrationHelper = new IntegrationTestHelper();
            integrationHelper.PersistenceModel.Add(new ArtistMap());
            integrationHelper.PersistenceModel.Add(new AlbumMap());
            integrationHelper.PersistenceModel.Add(new TrackMap());
            integrationHelper.PersistenceModel.Add(new TagMap());

            integrationHelper.Execute(session =>
            {
                var melodicDeath = new Tag { Description = "Melodic Death Metal" };
                session.Save(melodicDeath);

                var inflames = new Artist {Name = "In Flames"};
                session.Save(inflames);

                var whoracle = new Album {Title = "Whoracle"};
                whoracle.Tags.Add(melodicDeath);
                whoracle.Artist = inflames;
                session.Save(whoracle);

                var jotun = new Track {Name = "Jotun"};
                jotun.Album = whoracle;
                session.Save(jotun);
            });
        }

    }
}