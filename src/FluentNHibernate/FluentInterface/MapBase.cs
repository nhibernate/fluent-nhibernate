using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Reflection;

namespace FluentNHibernate.FluentInterface
{
    public abstract class MapBase<T>
    {
        private readonly IHasMappedMembers _hasMappedMembers;

        protected MapBase(IHasMappedMembers hasMappedMembers)
        {
            _hasMappedMembers = hasMappedMembers;
        }

        public PropertyMap Map(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);
            var propertyMapping = new PropertyMapping { PropertyInfo = info };

            _hasMappedMembers.AddProperty(propertyMapping);
            return new PropertyMap(propertyMapping);
        }

        public ComponentMap<COMPONENT_TYPE> Component<COMPONENT_TYPE>(Expression<Func<T, COMPONENT_TYPE>> expression, Action<ComponentMap<COMPONENT_TYPE>> action)
        {
            var componentMapping = new ComponentMapping {PropertyInfo = ReflectionHelper.GetProperty(expression)};
            _hasMappedMembers.AddComponent(componentMapping);

            var map = new ComponentMap<COMPONENT_TYPE>(componentMapping);
            action(map);

            return map;
        }
    }
}
