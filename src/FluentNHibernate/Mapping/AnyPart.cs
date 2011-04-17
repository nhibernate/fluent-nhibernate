using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly AttributeStore attributes = new AttributeStore();
        private readonly Type entity;
        private readonly Member member;
        private readonly AccessStrategyBuilder<AnyPart<T>> access;
        private readonly CascadeExpression<AnyPart<T>> cascade;
        private readonly IList<string> typeColumns = new List<string>();
        private readonly IList<string> identifierColumns = new List<string>();
        private readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();
        private bool nextBool = true;
        bool idTypeSet;

        public AnyPart(Type entity, Member member)
        {
            this.entity = entity;
            this.member = member;
            access = new AccessStrategyBuilder<AnyPart<T>>(this, value => attributes.Set("Access", Layer.UserSupplied, value));
            cascade = new CascadeExpression<AnyPart<T>>(this, value => attributes.Set("Cascade", Layer.UserSupplied, value));

            SetDefaultAccess();
        }

        void SetDefaultAccess()
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess == Mapping.Access.Property || resolvedAccess == Mapping.Access.Unset)
                return; // property is the default so we don't need to specify it

            attributes.Set("Access", Layer.Defaults, resolvedAccess.ToString());
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
            attributes.Set("IdType", Layer.UserSupplied, type.AssemblyQualifiedName);
            idTypeSet = true;
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
            var metaValueMapping = new MetaValueMapping
            {
                ContainingEntityType = entity
            };
            metaValueMapping.Set(x => x.Class, Layer.Defaults, new TypeReference(@class));
            metaValueMapping.Set(x => x.Value, Layer.Defaults, valueMap);
            metaValues.Add(metaValueMapping);
            return this;
        }

        public AnyPart<T> Insert()
        {
            attributes.Set("Insert", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> Update()
        {
            attributes.Set("Update", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> ReadOnly()
        {
            attributes.Set("Insert", Layer.UserSupplied, !nextBool);
            attributes.Set("Update", Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> LazyLoad()
        {
            attributes.Set("Lazy", Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        public AnyPart<T> OptimisticLock()
        {
            attributes.Set("OptimisticLock", Layer.UserSupplied, nextBool);
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
            var mapping = new AnyMapping(attributes.Clone());

            if (typeColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Type Column");
            if (identifierColumns.Count() == 0)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Identifier Column");
            if (!idTypeSet)
                throw new InvalidOperationException("<any> mapping is not valid without specifying an IdType");

            mapping.ContainingEntityType = entity;

            mapping.Set(x => x.Name, Layer.Defaults, member.Name);

            if (!mapping.IsSpecified("MetaType"))
            {
                mapping.Set(x => x.MetaType, Layer.Defaults, new TypeReference(member.PropertyType));
            }

            if (metaValues.Count() > 0)
            {
                metaValues.Each(mapping.AddMetaValue);
                mapping.Set(x => x.MetaType, Layer.Defaults, new TypeReference(typeof(string)));
            }

            foreach (var column in typeColumns)
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, column);
                mapping.AddTypeColumn(Layer.UserSupplied, columnMapping);
            }


            foreach (var column in identifierColumns)
            {
                var columnMapping = new ColumnMapping();
                columnMapping.Set(x => x.Name, Layer.Defaults, column);
                mapping.AddIdentifierColumn(Layer.UserSupplied, columnMapping);
            }

            return mapping;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <typeparam name="TMetaType">Meta type</typeparam>
        public AnyPart<T> MetaType<TMetaType>()
        {
            attributes.Set("MetaType", Layer.UserSupplied, new TypeReference(typeof(TMetaType)));
            return this;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <param name="metaType">Meta type</param>
        public AnyPart<T> MetaType(string metaType)
        {
            attributes.Set("MetaType", Layer.UserSupplied, new TypeReference(metaType));
            return this;
        }

        /// <summary>
        /// Sets the meta-type value for this any mapping.
        /// </summary>
        /// <param name="metaType">Meta type</param>
        public AnyPart<T> MetaType(Type metaType)
        {
            attributes.Set("MetaType", Layer.UserSupplied, new TypeReference(metaType));
            return this;
        }
    }
}
