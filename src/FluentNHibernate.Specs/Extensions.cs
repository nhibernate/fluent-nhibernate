﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentNHibernate.MappingModel.ClassBased;
using FluentAssertions;

namespace FluentNHibernate.Specs;

public static class Extensions
{
    public static T As<T>(this object instance)
    {
        return (T)instance;
    }

    public static void ShouldContain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
        collection.Any(predicate).Should().BeTrue();
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
        collection.Any(predicate).Should().BeFalse();
    }

    public static ClassMapping BuildMappingFor<T>(this FluentNHibernate.PersistenceModel model)
    {
        return model.BuildMappings()
            .SelectMany(x => x.Classes)
            .FirstOrDefault(x => x.Type == typeof(T));
    }

    public static void OutputXmlToConsole(this XmlDocument document)
    {
        var stringWriter = new System.IO.StringWriter();
        var xmlWriter = new XmlTextWriter(stringWriter);
        xmlWriter.Formatting = Formatting.Indented;
        document.WriteContentTo(xmlWriter);

        Console.WriteLine(string.Empty);
        Console.WriteLine(stringWriter.ToString());
        Console.WriteLine(string.Empty);
    }
}
