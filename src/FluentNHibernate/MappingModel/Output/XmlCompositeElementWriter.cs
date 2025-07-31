﻿using System.Xml;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output;

public class XmlCompositeElementWriter(IXmlWriterServiceLocator serviceLocator)
    : NullMappingModelVisitor, IXmlWriter<CompositeElementMapping>
{
    protected XmlDocument document;

    public XmlDocument Write(CompositeElementMapping compositeElement)
    {
        document = null;
        compositeElement.AcceptVisitor(this);
        return document;
    }

    public override void ProcessCompositeElement(CompositeElementMapping mapping)
    {
        document = new XmlDocument();

        var name = mapping is NestedCompositeElementMapping ? "nested-composite-element" : "composite-element";
        var element = document.AddElement(name);

        if (mapping.IsSpecified("Class"))
            element.WithAtt("class", mapping.Class);

        if (mapping is NestedCompositeElementMapping nested)
        {
            element.WithAtt("name", nested.Name);

            if (mapping.IsSpecified("Access")) 
                element.WithAtt("access", nested.Access);
        }
    }

    public override void Visit(CompositeElementMapping compositeElementMapping)
    {
        var writer = serviceLocator.GetWriter<CompositeElementMapping>();
        var xml = writer.Write(compositeElementMapping);

        document.ImportAndAppendChild(xml);
    }

    public override void Visit(PropertyMapping propertyMapping)
    {
        var writer = serviceLocator.GetWriter<PropertyMapping>();
        var xml = writer.Write(propertyMapping);

        document.ImportAndAppendChild(xml);
    }

    public override void Visit(ManyToOneMapping mapping)
    {
        var writer = serviceLocator.GetWriter<ManyToOneMapping>();
        var xml = writer.Write(mapping);

        document.ImportAndAppendChild(xml);
    }

    public override void Visit(ParentMapping mapping)
    {
        var writer = serviceLocator.GetWriter<ParentMapping>();
        var xml = writer.Write(mapping);

        document.ImportAndAppendChild(xml);
    }
}
