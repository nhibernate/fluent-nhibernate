using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IAnyPart<T> : IAccessStrategy<IAnyPart<T>>
    {
        /// <summary>
        /// (REQUIRED) The identity type of the any mapping
        /// </summary>
        /// <returns></returns>
        IAnyPart<T> IdentityType(Expression<Func<T, object>> expression);

        /// <summary>
        /// (REQUIRED) The identity type of the any mapping
        /// </summary>
        IAnyPart<T> IdentityType<TIdentity>();

        /// <summary>
        /// (REQUIRED) The identity type of the any mapping
        /// </summary>
        IAnyPart<T> IdentityType(Type type);

        /// <summary>
        /// (REQUIRED) Specifies the column name that will contain the type of the associated entity
        /// </summary>
        IAnyPart<T> EntityTypeColumn(string columnName);

        /// <summary>
        /// (REQUIRED) Specifies the column name that will hold the identifier
        /// </summary>
        IAnyPart<T> EntityIdentifierColumn(string columnName);

        /// <summary>
        /// This is used to map specific class types to value types stored in the EntityTypeColumn
        /// This method should only be called if the MetaType specified for this ANY mapping is a basic data type (such as string)
        /// </summary>
        /// <typeparam name="TModel">The class type to map</typeparam>
        /// <param name="valueMap">The string or character representing the value stored in the EntityTypeColumn in the table</param>
        IAnyPart<T> AddMetaValue<TModel>(string valueMap);

        /// <summary>
        /// Sets the cascade of this part. Valid options are "none", "all", and "save-update"
        /// </summary>
        CascadeExpression<IAnyPart<T>> Cascade { get; }
        IAnyPart<T> Not { get; }
        IAnyPart<T> Insert();
        IAnyPart<T> Update();
        IAnyPart<T> ReadOnly();
    }

    public interface IAnyMappingProvider
    {
        AnyMapping GetAnyMapping();
    }

    /// <summary>
    /// Represents the "Any" mapping in NHibernate. It is impossible to specify a foreign key constraint for this kind of association. For more information
    /// please reference chapter 5.2.4 in the NHibernate online documentation
    /// </summary>
    public class AnyPart<T> : IAnyPart<T>, IAnyMappingProvider
    {
        private readonly AccessStrategyBuilder<IAnyPart<T>> access;
        private readonly CascadeExpression<IAnyPart<T>> cascade;
        private readonly AnyMapping mapping = new AnyMapping();

        public AnyPart(PropertyInfo property)
        {
            access = new AccessStrategyBuilder<IAnyPart<T>>(this, value => mapping.Access = value);
            cascade = new CascadeExpression<IAnyPart<T>>(this, value => mapping.Cascade = value);
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
        public AccessStrategyBuilder<IAnyPart<T>> Access
        {
            get { return access; }
        }

        /// <summary>
        /// Cascade style (Defaults to none)
        /// </summary>
        public CascadeExpression<IAnyPart<T>> Cascade
        {
            get { return cascade; }
        }

        public IAnyPart<T> IdentityType(Expression<Func<T, object>> expression)
        {
            return IdentityType(ReflectionHelper.GetProperty(expression).PropertyType);
        }

        public IAnyPart<T> IdentityType<TIdentity>()
        {
            return IdentityType(typeof(TIdentity));
        }

        public IAnyPart<T> IdentityType(Type type)
        {
            mapping.IdType = type.AssemblyQualifiedName;
            return this;
        }

        public IAnyPart<T> EntityTypeColumn(string columnName)
        {
            mapping.AddTypeColumn(new ColumnMapping { Name = columnName });
            return this;
        }

        public IAnyPart<T> EntityIdentifierColumn(string columnName)
        {
            mapping.AddIdentifierColumn(new ColumnMapping { Name = columnName });
            return this;
        }

        public IAnyPart<T> AddMetaValue<TModel>(string valueMap)
        {
            mapping.AddMetaValue(new MetaValueMapping { Class = new TypeReference(typeof(TModel)), Value = valueMap });
            return this;
        }

        public IAnyPart<T> Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        public IAnyPart<T> Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
        }

        public IAnyPart<T> ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
            return this;
        }

        public IAnyPart<T> Not
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
