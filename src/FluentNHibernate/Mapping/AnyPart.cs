using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
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
        private readonly AttributeStore<AnyMapping> attributes = new AttributeStore<AnyMapping>();
        private readonly Type entity;
        private readonly Member member;
        private readonly AccessStrategyBuilder<AnyPart<T>> access;
        private readonly CascadeExpression<AnyPart<T>> cascade;
        private readonly IList<string> typeColumns = new List<string>();
        private readonly IList<string> identifierColumns = new List<string>();
        private readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();
        private bool nextBool = true;

        public AnyPart(Type entity, Member member)
        {
            this.entity = entity;
            this.member = member;
            access = new AccessStrategyBuilder<AnyPart<T>>(this, value => attributes.Set(x => x.Access, value));
            cascade = new CascadeExpression<AnyPart<T>>(this, value => attributes.Set(x => x.Cascade, value));

            SetDefaultAccess();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.SetDefault(x => x.Access, resolvedAccess.ToString());
        }

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
            return IdentityType(expression.ToMember().PropertyType);
        }

        public AnyPart<T> IdentityType<TIdentity>()
        {
            return IdentityType(typeof(TIdentity));
        }

        public AnyPart<T> IdentityType(Type type)
        {
            attributes.Set(x => x.IdType, type.AssemblyQualifiedName);
            return this;
        }

        public AnyPart<T> EntityTypeColumn(string columnName)
        {
            typeColumns.Add(columnName);
            return this;
        }

        public AnyPart<T> EntityIdentifierColumn(string columnName)
        {
            identifierColumns.Add(columnName);
            return this;
        }

        public AnyPart<T> AddMetaValue<TModel>(string valueMap)
        {
            return AddMetaValue(typeof(TModel), valueMap);
        }

        public AnyPart<T> AddMetaValue(Type @class, string valueMap)
        {
            metaValues.Add(new MetaValueMapping
            {
                Class = new TypeReference(@class),
                Value = valueMap,
                ContainingEntityType = entity
            });
            return this;
        }

        public AnyPart<T> Insert()
        {
            attributes.Set(x => x.Insert, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> Update()
        {
            attributes.Set(x => x.Update, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            attributes.Set(x => x.Update, !nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> LazyLoad()
        {
            attributes.Set(x => x.Lazy, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> OptimisticLock()
        {
            attributes.Set(x => x.OptimisticLock, nextBool);
            nextBool = true;
            return this;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            var mapping = new AnyMapping(attributes.CloneInner());

            if (typeColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Type Column");
            if (identifierColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Identifier Column");
            if (!mapping.IsSpecified("IdType"))
                throw new InvalidOperationException("<any> mapping is not valid without specifying an IdType");

            mapping.ContainingEntityType = entity;

            if (!mapping.IsSpecified("Name"))
                mapping.Name = member.Name;

            if (!mapping.IsSpecified("MetaType"))
            {
                if (metaValues.Count() > 0)
                {
                    metaValues.Each(mapping.AddMetaValue);
                    mapping.MetaType = new TypeReference(typeof(string));
                }
                else
                    mapping.MetaType = new TypeReference(member.PropertyType);
            }

            foreach (var column in typeColumns)
                mapping.AddTypeColumn(new ColumnMapping { Name = column });

            foreach (var column in identifierColumns)
                mapping.AddIdentifierColumn(new ColumnMapping { Name = column });

            return mapping;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <typeparam name="TMetaType">Meta type</typeparam>
        public AnyPart<T> MetaType<TMetaType>()
        {
            attributes.Set(x => x.MetaType, new TypeReference(typeof(TMetaType)));
            return this;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <param name="metaType">Meta type</param>
        public AnyPart<T> MetaType(string metaType)
        {
            attributes.Set(x => x.MetaType, new TypeReference(metaType));
            return this;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <param name="metaType">Meta type</param>
        public AnyPart<T> MetaType(Type metaType)
        {
            attributes.Set(x => x.MetaType, new TypeReference(metaType));
            return this;
        }
    }
}
