using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class DiscriminatorPart : IDiscriminatorPart
    {
        private readonly IClassMap classMap;
        private readonly DiscriminatorMapping mapping;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private bool nextBool = true;

        public DiscriminatorPart(IClassMap classMap, ClassMapping parentMapping, string columnName)
            : this(new DiscriminatorMapping(parentMapping) { ColumnName = columnName })
        {
            this.classMap = classMap;
        }

        public DiscriminatorPart(DiscriminatorMapping mapping)
        {
            this.mapping = mapping;
        }

        public DiscriminatorMapping GetDiscriminatorMapping()
        {
            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        public DiscriminatorPart SubClass<TSubClass>(object discriminatorValue, Action<SubClassPart<TSubClass>> action)
        {
            var subclass = new SubClassPart<TSubClass>(this, discriminatorValue);

            action(subclass);

            mapping.ParentClass.AddSubclass(subclass.GetSubclassMapping());
            classMap.AddSubclass(subclass); // HACK for conventions

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

        public DiscriminatorPart WithLengthOf(int length)
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

        /// <summary>
        /// Set an attribute on the xml element produced by this discriminator mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }
    }
}