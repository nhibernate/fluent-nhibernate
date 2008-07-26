using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShadeTree.DomainModel
{
    public interface IMappingVisitor
    {
        Conventions Conventions { get;}
        Type CurrentType { get; set; }
        void AddMappingDocument(XmlDocument document, Type type);
        void RegisterDependency(Type parentType);
    }
}
