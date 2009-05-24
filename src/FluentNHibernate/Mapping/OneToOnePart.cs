using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IOneToOnePart : IRelationship
    {
        CascadeExpression<IOneToOnePart> Cascade { get; }
        OneToOneMapping GetOneToOneMapping();
    }

    public class OneToOnePart<TOther> : IOneToOnePart, IAccessStrategy<OneToOnePart<TOther>>
    {
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<OneToOnePart<TOther>> access;
        private readonly OuterJoinBuilder<OneToOnePart<TOther>> outerJoin;
        private readonly FetchTypeExpression<OneToOnePart<TOther>> fetch;
        private readonly CascadeExpression<IOneToOnePart> cascade;
        private readonly OneToOneMapping mapping = new OneToOneMapping();
        private bool nextBool = true;
        public Type EntityType { get; private set; }

        public OneToOnePart(Type entity, PropertyInfo property)
        {
            outerJoin = new OuterJoinBuilder<OneToOnePart<TOther>>(this, value => mapping.OuterJoin = value);
            access = new AccessStrategyBuilder<OneToOnePart<TOther>>(this, value => mapping.Access = value);
            fetch = new FetchTypeExpression<OneToOnePart<TOther>>(this, value => mapping.Fetch = value);
            cascade = new CascadeExpression<IOneToOnePart>(this, value => mapping.Cascade = value);
            EntityType = entity;
            this.property = property;
        }

        public OneToOneMapping GetOneToOneMapping()
        {
            if (!mapping.Attributes.IsSpecified(x => x.Class))
                mapping.Class = typeof(TOther).AssemblyQualifiedName;

            if (!mapping.Attributes.IsSpecified(x => x.Name))
                mapping.Name = property.Name;

            return mapping;
        }

        public FetchTypeExpression<OneToOnePart<TOther>> Fetch
        {
            get { return fetch; }
        }

        public OneToOnePart<TOther> ForeignKey()
        {
            return ForeignKey(string.Format("FK_{0}To{1}", property.DeclaringType.Name, property.Name));
        }

        public OneToOnePart<TOther> ForeignKey(string foreignKeyName)
        {
            mapping.ForeignKey = foreignKeyName;
            return this;
        }

        public OneToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            mapping.PropertyRef = prop.Name;

            return this;
        }

        public OneToOnePart<TOther> Constrained()
        {
            mapping.Constrained = nextBool;
            nextBool = true;

            return this;
        }

        public CascadeExpression<IOneToOnePart> Cascade
        {
            get { return cascade; }
        }

        public AccessStrategyBuilder<OneToOnePart<TOther>> Access
        {
            get { return access; }
        }

        public OneToOnePart<TOther> LazyLoad()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        public OneToOnePart<TOther> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public OuterJoinBuilder<OneToOnePart<TOther>> OuterJoin
        {
            get { return outerJoin; }
        }

        #region Explicit IOneToOnePart Implementation

        CascadeExpression<IOneToOnePart> IOneToOnePart.Cascade
        {
            get { return cascade; }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        #endregion

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttribute(string name, string value)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttributes(Attributes atts)
        {
            throw new NotSupportedException("Obsolete");
        }

        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }
    }
}
