using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Automapping
{
    public class AutoCompositeIdentityPart<T> : CompositeIdentityPart<T>
    {
        private readonly IList<string> mappedProperties;

        public AutoCompositeIdentityPart(IList<string> mappedProperties)
        {
            this.mappedProperties = mappedProperties;
        }

        protected override CompositeIdentityPart<T> KeyProperty(PropertyInfo property, string columnName)
        {
            mappedProperties.Add(property.Name);

            return base.KeyProperty(property, columnName);
        }

        protected override CompositeIdentityPart<T> KeyReference(PropertyInfo property, string columnName)
        {
            mappedProperties.Add(property.Name);

            return base.KeyReference(property, columnName);
        }
    }
}