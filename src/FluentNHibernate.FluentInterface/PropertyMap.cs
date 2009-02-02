using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface
{
    public class PropertyMap
    {
        private readonly PropertyMapping _mapping;

        public PropertyMap(PropertyMapping mapping)
        {
            _mapping = mapping;
        }

        public PropertyMap WithLengthOf(int length)
        {
            _mapping.Length = length;
            return this;
        }

        public PropertyMap CanNotBeNull()
        {
            _mapping.IsNotNullable = true;
            return this;
        }
    }
}