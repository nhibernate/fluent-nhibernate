﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests;

[TestFixture]
public class SubclassMapWithUserTypeTests
{
    [Test]
    public void ShouldTakeIntoAccountAllColumnsOfAUserType()
    {
        var model = new PersistenceModel();

        model.Add(new MediaMap());
        model.Add(new ImageMap());

        var mappings = model.BuildMappings();
        var sb = new StringBuilder();
        model.WriteMappingsTo(new StringWriter(sb));

        var subclassMapping = mappings.SelectMany(m => m.Classes)
            .FirstOrDefault(cm => cm.Subclasses.Any()).Subclasses.FirstOrDefault();

        subclassMapping.Properties.SingleOrDefault(x => x.Name == "Contexts");
    }
}

public class Media
{
    public virtual Guid Id { get; protected set; }
    public virtual bool IsDeprecated { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual DateTime Modified { get; set; }

}

public class MediaMap : ClassMap<Media>
{
    public MediaMap()
    {
        Id(x => x.Id).Column("[Guid]").GeneratedBy.GuidComb();
        Map(x => x.IsDeprecated).Not.Nullable();
        Map(x => x.Created).Not.Nullable();
        Map(x => x.Modified).Not.Nullable();
    }
}

public class Image : Media
{
    private IList<string> contexts = new List<string>();

    public virtual string Title { get; set; }
    public virtual string Description { get; set; }
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public virtual IList<string> Contexts => contexts;
}

public class ImageMap : SubclassMap<Image>
{
    public ImageMap()
    {
        Table("[ImageInfo]");
        KeyColumn("[Guid]");
        Map(x => x.Title).Length(100).Not.Nullable();
        Map(x => x.Description).Length(500).Nullable();
        Map(x => x.Width).Not.Nullable();
        Map(x => x.Height).Not.Nullable();
        Map(x => x.Contexts)
            .Access.CamelCaseField()
            .CustomType<ImageContextsUserType>()
            .Columns.Add(new[] { "IsIcon", "IsPromo", "IsWallpaper", "IsPlaceholder" })
            .Not.Nullable();
    }
}

public class ImageContextsUserType : IUserType
{
    public new bool Equals(object x, object y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode(object x)
    {
        throw new NotImplementedException();
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        IList<string> contexts = new List<string>();

        if ((bool)NHibernateUtil.Boolean.NullSafeGet(rs, names[0], session)) contexts.Add("Icon");
        if ((bool)NHibernateUtil.Boolean.NullSafeGet(rs, names[1], session)) contexts.Add("Promo");
        if ((bool)NHibernateUtil.Boolean.NullSafeGet(rs, names[2], session)) contexts.Add("Wallpaper");
        if ((bool)NHibernateUtil.Boolean.NullSafeGet(rs, names[3], session)) contexts.Add("Placeholder");

        return contexts;
    }

    public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
        IList<string> contexts = value as IList<string>;

        if (contexts is not null)
        {
            NHibernateUtil.Boolean.NullSafeSet(cmd, contexts.Contains("Icon"), index, session);
            NHibernateUtil.Boolean.NullSafeSet(cmd, contexts.Contains("Promo"), index + 1, session);
            NHibernateUtil.Boolean.NullSafeSet(cmd, contexts.Contains("Wallpaper"), index + 2, session);
            NHibernateUtil.Boolean.NullSafeSet(cmd, contexts.Contains("Placeholder"), index + 3, session);
        }
    }

    public object DeepCopy(object value)
    {
        throw new NotImplementedException();
    }

    public object Replace(object original, object target, object owner)
    {
        throw new NotImplementedException();
    }

    public object Assemble(object cached, object owner)
    {
        throw new NotImplementedException();
    }

    public object Disassemble(object value)
    {
        throw new NotImplementedException();
    }

    public Type ReturnedType => typeof(IList<string>);

    public bool IsMutable => throw new NotImplementedException();

    public SqlType[] SqlTypes
    {
        get
        {
            return new[] {
                NHibernateUtil.Boolean.SqlType, NHibernateUtil.Boolean.SqlType,
                NHibernateUtil.Boolean.SqlType, NHibernateUtil.Boolean.SqlType };
        }
    }
}
