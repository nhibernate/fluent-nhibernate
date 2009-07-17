using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IOneToOneMappingProvider
    {
        OneToOneMapping GetOneToOneMapping();
    }

    public class OneToOnePart<TOther> : IOneToOneMappingProvider, IAccessStrategy<OneToOnePart<TOther>>
    {
        private readonly Type entity;
        private readonly PropertyInfo property;
        private readonly AccessStrategyBuilder<OneToOnePart<TOther>> access;
        private readonly OuterJoinBuilder<OneToOnePart<TOther>> outerJoin;
        private readonly FetchTypeExpression<OneToOnePart<TOther>> fetch;
        private readonly CascadeExpression<OneToOnePart<TOther>> cascade;
        private readonly OneToOneMapping mapping = new OneToOneMapping();
        private bool nextBool = true;

        public OneToOnePart(Type entity, PropertyInfo property)
        {
            outerJoin = new OuterJoinBuilder<OneToOnePart<TOther>>(this, value => mapping.OuterJoin = value);
            access = new AccessStrategyBuilder<OneToOnePart<TOther>>(this, value => mapping.Access = value);
            fetch = new FetchTypeExpression<OneToOnePart<TOther>>(this, value => mapping.Fetch = value);
            cascade = new CascadeExpression<OneToOnePart<TOther>>(this, value => mapping.Cascade = value);
            this.entity = entity;
            this.property = property;
        }

        OneToOneMapping IOneToOneMappingProvider.GetOneToOneMapping()
        {
            mapping.ContainingEntityType = entity;

            if (!mapping.IsSpecified(x => x.Class))
                mapping.SetDefaultValue(x => x.Class, new TypeReference(typeof(TOther)));

            if (!mapping.IsSpecified(x => x.Name))
                mapping.SetDefaultValue(x => x.Name, property.Name);

            return mapping;
        }

        public OneToOnePart<TOther> Class<T>()
        {
            return Class(typeof(T));
        }

        public OneToOnePart<TOther> Class(Type type)
        {
            mapping.Class = new TypeReference(type);
            return this;
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

            return PropertyRef(prop.Name);
        }

        public OneToOnePart<TOther> PropertyRef(string propertyName)
        {
            mapping.PropertyRef = propertyName;

            return this;
        }

        public OneToOnePart<TOther> Constrained()
        {
            mapping.Constrained = nextBool;
            nextBool = true;

            return this;
        }

        public CascadeExpression<OneToOnePart<TOther>> Cascade
        {
            get { return cascade; }
        }

        public AccessStrategyBuilder<OneToOnePart<TOther>> Access
        {
            get { return access; }
        }

        public OneToOnePart<TOther> LazyLoad()
        {
            mapping.Lazy = nextBool ? Laziness.True : Laziness.False;
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
    }
}
