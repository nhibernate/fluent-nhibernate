using System;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart : IDiscriminatorMappingProvider
    {
        private readonly Type entity;
        private readonly Action<Type, ISubclassMappingProvider> setter;
        private readonly TypeReference discriminatorValueType;
        private readonly AttributeStore<DiscriminatorMapping> attributes = new AttributeStore<DiscriminatorMapping>();
        private bool nextBool = true;

        public DiscriminatorPart(string columnName, Type entity, Action<Type, ISubclassMappingProvider> setter, TypeReference discriminatorValueType)
        {
            this.entity = entity;
            this.setter = setter;
            this.discriminatorValueType = discriminatorValueType;

            attributes.SetDefault(x => x.Column, columnName);
        }

        DiscriminatorMapping IDiscriminatorMappingProvider.GetDiscriminatorMapping()
        {
            var mapping = new DiscriminatorMapping(attributes.CloneInner())
            {
                ContainingEntityType = entity,
                Type = discriminatorValueType
            };

            return mapping;
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public DiscriminatorPart SubClass<TSubClass>(object discriminatorValue, Action<SubClassPart<TSubClass>> action)
        {
            var subclass = new SubClassPart<TSubClass>(this, discriminatorValue);

            action(subclass);
            setter(typeof(TSubClass), subclass);

            return this;
        }

        [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
        public DiscriminatorPart SubClass<TSubClass>(Action<SubClassPart<TSubClass>> action)
        {
            return SubClass(null, action);
        }

        public DiscriminatorPart Not
        {
             get
             {
                 nextBool = !nextBool;
                 return this;
             }
        }

        public DiscriminatorPart Nullable()
        {
            attributes.Set(x => x.NotNull, !nextBool);
            nextBool = true;
            return this;
        }

        public DiscriminatorPart Length(int length)
        {
            attributes.Set(x => x.Length, length);
            return this;
        }

        /// <summary>
        /// Force NHibernate to always select using the discriminator value, even when selecting all subclasses. This
        /// can be useful when your table contains more discriminator values than you have classes (legacy).
        /// </summary>
        /// <remarks>Sets the "force" attribute.</remarks>
        public DiscriminatorPart AlwaysSelectWithValue()
        {
            attributes.Set(x => x.Force, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Set this discriminator as read-only. Call this if your discriminator column is also part of a mapped composite identifier.
        /// </summary>
        /// <returns>Sets the "insert" attribute.</returns>
        public DiscriminatorPart ReadOnly()
        {
            attributes.Set(x => x.Insert, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// An arbitrary SQL expression that is executed when a type has to be evaluated. Allows content-based discrimination.
        /// </summary>
        /// <param name="sql">SQL expression</param>
        public DiscriminatorPart Formula(string sql)
        {
            attributes.Set(x => x.Formula, sql);
            return this;
        }
    }
}