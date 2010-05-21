using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public class NaturalIdPart<T> : INaturalIdMappingProvider
    {
        private readonly AttributeStore<NaturalIdMapping> attributes = new AttributeStore<NaturalIdMapping>();
        private readonly IList<PropertyMapping> properties = new List<PropertyMapping>();
        private readonly IList<ManyToOneMapping> manyToOnes = new List<ManyToOneMapping>();
        private bool nextBool = true;

        public NaturalIdPart() { }

        /// <summary>
        /// Defines a property to be used for this natural-id.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <returns>The natural id part fluent interface</returns>
        public NaturalIdPart<T> Property(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();
            return Property(expression, member.Name);
        }

        /// <summary>
        /// Defines a property to be used for this natural-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnName">The column name in the database to use for this natural id, or null to use the property name</param>
        /// <returns>The natural id part fluent interface</returns>
        public NaturalIdPart<T> Property(Expression<Func<T, object>> expression, string columnName)
        {
            var member = expression.ToMember();
            return Property(member, columnName);
        }

        protected virtual NaturalIdPart<T> Property(Member member, string columnName)
        {
            var key = new PropertyMapping
            {
                Name = member.Name,
                Type = new TypeReference(member.PropertyType)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            properties.Add(key);

            return this;
        }

        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this natural-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <returns>The natural ID part fluent interface</returns>
        public NaturalIdPart<T> Reference(Expression<Func<T, object>> expression)
        {
            var member = expression.ToMember();
            return Reference(expression, member.Name);
        }

        /// <summary>
        /// Defines a reference to be used as a many-to-one key for this natural-id with an explicit column name.
        /// </summary>
        /// <param name="expression">A member access lambda expression for the property</param>
        /// <param name="columnName">The column name in the database to use for this key, or null to use the property name</param>
        /// <returns>The natural id part fluent interface</returns>
        public NaturalIdPart<T> Reference(Expression<Func<T, object>> expression, string columnName)
        {
            var member = expression.ToMember();
            return Reference(member, columnName);
        }

        protected virtual NaturalIdPart<T> Reference(Member member, string columnName)
        {
            var key = new ManyToOneMapping
            {
                Name = member.Name,
                Class = new TypeReference(member.PropertyType),
                ContainingEntityType = typeof(T)
            };
            key.AddColumn(new ColumnMapping { Name = columnName });

            manyToOnes.Add(key);

            return this;
        }

        /// <summary>
        /// Specifies that this id is read-only
        /// </summary>
        /// <remarks>This is the same as setting the mutable attribute to false</remarks>
        public NaturalIdPart<T> ReadOnly()
        {
            attributes.Set(x => x.Mutable, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NaturalIdPart<T> Not
        {
            get
            {
                nextBool = false;
                return this;
            }
        }

        NaturalIdMapping INaturalIdMappingProvider.GetNaturalIdMapping()
        {
            var mapping = new NaturalIdMapping(attributes.CloneInner());

            properties.Each(mapping.AddProperty);
            manyToOnes.Each(mapping.AddReference);

            return mapping;
        }
    }
}

