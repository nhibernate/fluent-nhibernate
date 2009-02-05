using System;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface
{
    public class DiscriminatorPart
    {
        private readonly DiscriminatorMapping _discriminatorMapping;

        public DiscriminatorPart(DiscriminatorMapping discriminatorMapping)
        {
            _discriminatorMapping = discriminatorMapping;
        }

        public DiscriminatorPart ColumnType(DiscriminatorType type)
        {
            _discriminatorMapping.DiscriminatorType = type;
            return this;
        }

        public DiscriminatorPart SubClass<TSubclassType>(object discriminatorValue, Action<SubclassPart<TSubclassType>> action)
        {
            var subclassMapping = new SubclassMapping {Type = typeof (TSubclassType), DiscriminatorValue = discriminatorValue};
            _discriminatorMapping.ParentClass.AddSubclass(subclassMapping);
            var subclassPart = new SubclassPart<TSubclassType>(subclassMapping);
            action(subclassPart);
            return this;
        }
    }
}