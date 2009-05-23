using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IOneToOnePart : IRelationship
    {
        CascadeExpression<IOneToOnePart> Cascade { get; }
    }

    public class OneToOnePart<TOther> : IOneToOnePart, IAccessStrategy<OneToOnePart<TOther>>
    {
        private readonly Cache<string, string> properties = new Cache<string, string>();
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<OneToOnePart<TOther>> access;
        public Type EntityType { get; private set; }

        public OneToOnePart(Type entity, PropertyInfo property) {
            EntityType = entity;
            access = new AccessStrategyBuilder<OneToOnePart<TOther>>(this, value => SetAttribute("access", value));
            this.property = property;
        }

        public FetchTypeExpression<OneToOnePart<TOther>> FetchType {
            get {
                return new FetchTypeExpression<OneToOnePart<TOther>>(this, properties);
            }
        }
        
        public OneToOnePart<TOther> WithForeignKey() {
            return WithForeignKey(string.Format("FK_{0}To{1}", property.DeclaringType.Name, property.Name));
        }

        public OneToOnePart<TOther> WithForeignKey(string foreignKeyName) {
            properties.Store("foreign-key", foreignKeyName);
            return this;
        }

        public OneToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            properties.Store("property-ref", prop.Name);

            return this;
        }

        public OneToOnePart<TOther> Constrained()
        {
            properties.Store("constrained", "true");

            return this;
        }

        public CascadeExpression<OneToOnePart<TOther>> Cascade
        {
            get {
                return new CascadeExpression<OneToOnePart<TOther>>(this);
            }
        }

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
            properties.Store("name", property.Name);
            properties.Store("class", typeof(TOther).AssemblyQualifiedName);

            classElement.AddElement("one-to-one").WithProperties(properties);
        }

        public void SetAttribute(string name, string value)
        {
            properties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public int LevelWithinPosition {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        public AccessStrategyBuilder<OneToOnePart<TOther>> Access {
            get { return access; }
        }

        #region Explicit IOneToOnePart Implementation
        CascadeExpression<IOneToOnePart> IOneToOnePart.Cascade
        {
            get { return new CascadeExpression<IOneToOnePart>(this); }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        #endregion
    }
}
