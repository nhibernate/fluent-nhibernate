using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Reflection;

namespace FluentNHibernate.FluentInterface
{
    public abstract class ClassMapBase<T>
    {
        private readonly ClassMappingBase _classMapping;

        protected ClassMapBase(ClassMappingBase classMappingBase)
        {
            _classMapping = classMappingBase;    
        }

        public PropertyMap Map(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);
            var propertyMapping = new PropertyMapping { PropertyInfo = info };

            _classMapping.AddProperty(propertyMapping);
            return new PropertyMap(propertyMapping);
        }
    }
}
