using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Testing.DomainModel
{
    public class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ISet<Album> Albums { get; set; }

        public Artist()
        {
            Albums = new HashedSet<Album>();
        }
    }

    public class Album
    {
        public int ID { get; set; }
        public string Title { get; set;}
        public Artist Artist { get; set; }
        public ISet<Track> Tracks { get; set; }
        public ISet<Tag> Tags { get; set; }

        public Album()
        {
            Tracks = new HashedSet<Track>();
            Tags = new HashedSet<Tag>();
        }
    }

    public class Track
    {
        public int ID { get; set; }
        public Album Album { get; set; }
        public string Name { get; set; }
        public int TrackNumber { get; set; }
    }

    public class Tag
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
}