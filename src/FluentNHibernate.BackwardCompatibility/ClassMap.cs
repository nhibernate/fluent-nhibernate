using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Reflection;
using System.Collections.Generic;

namespace FluentNHibernate.BackwardCompatibility
{
    public class ClassMap<T> : IMappingProvider
    {
        private readonly ClassMapping _classMapping;
        private readonly IList<IDeferredCollectionMapping> _deferredCollections;

        public ClassMap()
        {
            _classMapping = new ClassMapping();
            _classMapping.Type = typeof (T);
            _deferredCollections = new List<IDeferredCollectionMapping>();
        }

        public ClassMapping GetClassMapping()
        {
            foreach (var collection in _deferredCollections.Select(x => x.ResolveCollectionMapping()))
                _classMapping.AddCollection(collection);

            _deferredCollections.Clear();

            return _classMapping;
        }

        public void Id(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            _classMapping.Id = new IdMapping(new ColumnMapping { PropertyInfo = info})
                                   {                                       
                                       PropertyInfo = info,
                                       Generator = IdGeneratorMapping.NativeGenerator
                                   };
        }

        public PropertyMap Map(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);
            var propertyMapping = new PropertyMapping {PropertyInfo = info};

            _classMapping.AddProperty(propertyMapping);
            return new PropertyMap(propertyMapping);
        }

        public OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            var part = new OneToManyPart<T, CHILD>(info);
            _deferredCollections.Add(part);
            return part;
        }

        public ManyToOnePart References(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            var mapping = new ManyToOneMapping {PropertyInfo = info};
            _classMapping.AddReference(mapping);
            return new ManyToOnePart(mapping);
        }
    }
}