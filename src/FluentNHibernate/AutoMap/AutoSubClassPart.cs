using System;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoSubClassPart<T> : AutoMap<T>, ISubclass
    {
        private readonly object discriminatorValue;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public AutoSubClassPart(object discriminatorValue)
        {
            this.discriminatorValue = discriminatorValue;
        }

        public override void SetAttribute(string name, string value)
        {
            attributes.Store(name, value);
        }

        public override void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            var subclassElement = classElement.AddElement("subclass")
                .WithAtt("name", typeof(T).AssemblyQualifiedName)
                .WithAtt("discriminator-value", discriminatorValue == null ? null : discriminatorValue.ToString());
            subclassElement.WithProperties(attributes);

            WriteTheParts(subclassElement, visitor);
        }

        public int LevelWithinPosition
        {
            get { return 4; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        void ISubclass.Proxy(Type type)
        {
            throw new NotImplementedException();
        }

        void ISubclass.Proxy<T1>()
        {
            throw new NotImplementedException();
        }

        void ISubclass.SelectBeforeUpdate()
        {
            throw new NotImplementedException();
        }

        void ISubclass.Abstract()
        {
            throw new NotImplementedException();
        }

        ISubclass ISubclass.Not
        {
            get { throw new NotImplementedException(); }
        }

        SubclassMapping ISubclass.GetSubclassMapping()
        {
            throw new NotImplementedException();
        }
    }
}