using System.Collections.Generic;

namespace FluentNHibernate.Testing.DomainModel;

public class Artist
{
    public virtual long Id { get; set; }
    public virtual string Name { get; set; }
    public virtual ISet<Album> Albums { get; set; }
    public virtual Genre Genre { get; set; }

    public Artist()
    {
        Albums = new HashSet<Album>();
    }
}

public class Genre
{
    public virtual long Id { get; set; }
    public virtual string Name { get; set; }
    public virtual IList<Artist> Artists { get; set; }

    public Genre()
    {
        Artists = new List<Artist>();
    }

    public virtual void AddArtist(Artist artist)
    {
        artist.Genre = this;
        Artists.Add(artist);
    }
}

public class Album
{
    public int ID { get; set; }
    public string Title { get; set;}
    public Artist Artist { get; set; }
    public ISet<Track> Tracks { get; set; } = new HashSet<Track>();
    public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();
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
