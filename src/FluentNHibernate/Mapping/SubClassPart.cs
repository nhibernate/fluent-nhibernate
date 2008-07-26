using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShadeTree.DomainModel.Mapping
{
    public class SubClassPart<T> : ClassMapBase<T>, IMappingPart
    {
        private readonly string _discriminatorValue;

        public SubClassPart(string discriminatorValue)
        {
            _discriminatorValue = discriminatorValue;
        }


        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            XmlElement subclassElement = classElement.AddElement("subclass")
                .WithAtt("discriminator-value", _discriminatorValue)
                .WithAtt("name", typeof(T).Name);

            writeTheParts(subclassElement, visitor);

        }

        public int Level
        {
            get { return 3; }
        }
    }
}
