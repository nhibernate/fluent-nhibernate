using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// Represents the "Any" mapping in NHibernate. It is impossible to specify a foreign key constraint for this kind of association. For more information
    /// please reference chapter 5.2.4 in the NHibernate online documentation
    /// </summary>
    public class AnyPart<T> : IAnyMappingProvider
    {
        private readonly Type entity;
        private readonly AccessStrategyBuilder<AnyPart<T>> access;
        private readonly CascadeExpression<AnyPart<T>> cascade;
        private readonly AnyMapping mapping = new AnyMapping();

        public AnyPart(Type entity, PropertyInfo property)
        {
            this.entity = entity;
            access = new AccessStrategyBuilder<AnyPart<T>>(this, value => mapping.Access = value);
            cascade = new CascadeExpression<AnyPart<T>>(this, value => mapping.Cascade = value);
            AnyProperty = property;
        }

        /// <summary>
        /// The property on your class that contains the mapped object
        /// </summary>
        public PropertyInfo AnyProperty { get; private set; }
        private bool nextBool = true;

        /// <summary>
        /// Defines how NHibernate will access the object for persisting/hydrating (Defaults to Property)
        /// </summary>
        public AccessStrategyBuilder<AnyPart<T>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Cascade style (Defaults to none)
        /// </summary>
        public CascadeExpression<AnyPart<T>> Cascade
        {
            get { return cascade; }
        }

        public AnyPart<T> IdentityType(Expression<Func<T, object>> expression)
        {
            return IdentityType(ReflectionHelper.GetProperty(expression).PropertyType);
        }

        public AnyPart<T> IdentityType<TIdentity>()
        {
            return IdentityType(typeof(TIdentity));
        }

        public AnyPart<T> IdentityType(Type type)
        {
            mapping.IdType = type.AssemblyQualifiedName;
            return this;
        }

        public AnyPart<T> EntityTypeColumn(string columnName)
        {
            mapping.AddTypeColumn(new ColumnMapping { Name = columnName });
            return this;
        }

        public AnyPart<T> EntityIdentifierColumn(string columnName)
        {
            mapping.AddIdentifierColumn(new ColumnMapping { Name = columnName });
            return this;
        }

        public AnyPart<T> AddMetaValue<TModel>(string valueMap)
        {
            mapping.AddMetaValue(new MetaValueMapping { Class = new TypeReference(typeof(TModel)), Value = valueMap });
            return this;
        }

        public AnyPart<T> Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        public AnyPart<T> Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
        }

        public AnyPart<T> ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public AnyPart<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        AnyMapping IAnyMappingProvider.GetAnyMapping()
        {
            if (mapping.TypeColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Type Column");
            if (mapping.IdentifierColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Identifier Column");
            if (!mapping.IsSpecified(x => x.IdType))
                throw new InvalidOperationException("<any> mapping is not valid without specifying an IdType");

            mapping.ContainingEntityType = entity;

            if (!mapping.IsSpecified(x => x.Name))
                mapping.Name = AnyProperty.Name;

            if (!mapping.IsSpecified(x => x.MetaType))
            {
                if (mapping.MetaValues.Count() > 0)
                    mapping.MetaType = new TypeReference(typeof(string));
                else
                    mapping.MetaType = new TypeReference(AnyProperty.PropertyType);
            }


            return mapping;
        }
    }
}
