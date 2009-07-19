using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart : IDiscriminatorPart
    {
        private readonly Action<Type, ISubclassMappingProvider> setter;
        private readonly DiscriminatorMapping mapping;
        private bool nextBool = true;

        public DiscriminatorPart(ClassMapping parentMapping, string columnName, Type entity, Action<Type, ISubclassMappingProvider> setter)
        {
            this.setter = setter;
            mapping = new DiscriminatorMapping(parentMapping)
            {
                Column = columnName,
                ContainingEntityType = entity
            };
        }

        public DiscriminatorMapping GetDiscriminatorMapping()
        {
            return mapping;
        }

        public DiscriminatorPart SubClass<TSubClass>(object discriminatorValue, Action<SubClassPart<TSubClass>> action)
        {
            var subclass = new SubClassPart<TSubClass>(this, discriminatorValue);

            action(subclass);
            setter(typeof(TSubClass), subclass);

            return this;
        }

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
            mapping.NotNull = !nextBool;
            nextBool = true;
            return this;
        }

        public DiscriminatorPart Length(int length)
        {
            mapping.Length = length;
            return this;
        }

        /// <summary>
        /// Force NHibernate to always select using the discriminator value, even when selecting all subclasses. This
        /// can be useful when your table contains more discriminator values than you have classes (legacy).
        /// </summary>
        /// <remarks>Sets the "force" attribute.</remarks>
        public DiscriminatorPart AlwaysSelectWithValue()
        {
            mapping.Force = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Set this discriminator as read-only. Call this if your discriminator column is also part of a mapped composite identifier.
        /// </summary>
        /// <returns>Sets the "insert" attribute.</returns>
        public DiscriminatorPart ReadOnly()
        {
            mapping.Insert = !nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// An arbitrary SQL expression that is executed when a type has to be evaluated. Allows content-based discrimination.
        /// </summary>
        /// <param name="sql">SQL expression</param>
        public DiscriminatorPart Formula(string sql)
        {
            mapping.Formula = sql;
            return this;
        }
    }
}