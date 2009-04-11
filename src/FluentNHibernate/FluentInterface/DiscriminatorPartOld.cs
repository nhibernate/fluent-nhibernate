using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface
{
    public class DiscriminatorPartOld
    {
        private readonly DiscriminatorMapping _discriminatorMapping;

        public DiscriminatorPartOld(DiscriminatorMapping discriminatorMapping)
        {
            _discriminatorMapping = discriminatorMapping;
        }

        public DiscriminatorPartOld ColumnType(DiscriminatorType type)
        {
            _discriminatorMapping.DiscriminatorType = type;
            return this;
        }

        public DiscriminatorPartOld SubClass<TSubclassType>(object discriminatorValue, Action<SubclassPart<TSubclassType>> action)
        {
            var subclassMapping = new SubclassMapping {Type = typeof (TSubclassType), DiscriminatorValue = discriminatorValue};
            _discriminatorMapping.ParentClass.AddSubclass(subclassMapping);
            var subclassPart = new SubclassPart<TSubclassType>(subclassMapping);
            action(subclassPart);
            return this;
        }
    }
}