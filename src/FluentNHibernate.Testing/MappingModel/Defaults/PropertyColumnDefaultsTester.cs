﻿using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Defaults;

[TestFixture]
public class PropertyColumnDefaultsTester
{
    [Test]
    public void ShouldHaveDefaultColumnIfNoneSpecified()
    {
        var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget)))
            .GetPropertyMapping();

        mapping.Columns.Count().ShouldEqual(1);
        mapping.Columns.Count().ShouldEqual(1);
    }

    [Test]
    public void ShouldHaveNoDefaultsIfUserSpecifiedColumn()
    {
        var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget))
                .Column("explicit"))
            .GetPropertyMapping();

        mapping.Columns.Count().ShouldEqual(1);
        mapping.Columns.Count().ShouldEqual(1);
    }

    [Test]
    public void DefaultColumnShouldInheritColumnAttributes()
    {
        var mapping = ((IPropertyMappingProvider)new PropertyPart(Prop(x => x.Name), typeof(PropertyTarget))
                .Not.Nullable())
            .GetPropertyMapping();

        mapping.Columns.First().NotNull.ShouldBeTrue();
        mapping.Columns.First().NotNull.ShouldBeTrue();
    }

    Member Prop(Expression<Func<PropertyTarget, object>> propertyAccessor)
    {
        return ReflectionHelper.GetMember(propertyAccessor);
    }
}
