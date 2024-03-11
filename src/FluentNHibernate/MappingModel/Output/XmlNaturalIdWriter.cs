﻿using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlNaturalIdWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<NaturalIdMapping>
{
    private XmlDocument document;

    public XmlDocument Write(NaturalIdMapping mappingModel)
    {
        document = null;
        mappingModel.AcceptVisitor(this);
        return document;
    }

    public override void ProcessNaturalId(NaturalIdMapping mapping)
    {
        document = new XmlDocument();

        var element = document.AddElement("natural-id");

        if (mapping.IsSpecified("Mutable"))
            element.WithAtt("mutable", mapping.Mutable);
    }

    public override void Visit(PropertyMapping mapping)
    {
        var writer = serviceLocator.GetWriter<PropertyMapping>();
        var xml = writer.Write(mapping);

        document.ImportAndAppendChild(xml);
    }

    public override void Visit(ManyToOneMapping mapping)
    {
        var writer = serviceLocator.GetWriter<ManyToOneMapping>();
        var xml = writer.Write(mapping);

        document.ImportAndAppendChild(xml);
    }
}
