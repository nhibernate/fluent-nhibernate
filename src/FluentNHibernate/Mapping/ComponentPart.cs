using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using ShadeTree.Validation;

namespace FluentNHibernate.Mapping
{
    public class ComponentPart<T> : ClassMapBase<T>, IMappingPart
    {
        private readonly PropertyInfo _property;

        public ComponentPart(PropertyInfo property, bool parentIsRequired)
        {
            _property = property;
            this.parentIsRequired = parentIsRequired && RequiredAttribute.IsRequired(_property) && parentIsRequired;
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement element = classElement.AddElement("component")
                .WithAtt("name", _property.Name)
                .WithAtt("insert", "true")
                .WithAtt("update", "true");

            writeTheParts(element, visitor);
        }

        public int Level
        {
            get { return 3; }
        }
    }
}
