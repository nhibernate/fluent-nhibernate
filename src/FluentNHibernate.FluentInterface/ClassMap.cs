using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Reflection;
using System.Collections.Generic;
using FluentNHibernate.Testing.FluentInterface;

namespace FluentNHibernate.FluentInterface
{
    public class ClassMap<T> : ClassMapBase<T>, IMappingProvider
    {
        private readonly ClassMapping _classMapping;
        private readonly IList<IDeferredCollectionMapping> _deferredCollections;

        public ClassMap()
            : this(new ClassMapping { Type = typeof(T) })
        {

        }

        protected ClassMap(ClassMapping classMapping)
            : base(classMapping)
        {
            _classMapping = classMapping;
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

            _classMapping.Id = new IdMapping(new ColumnMapping { PropertyInfo = info })
                                   {
                                       PropertyInfo = info,
                                       Generator = IdGeneratorMapping.NativeGenerator
                                   };
        }

        public OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            var part = new OneToManyPart<T, CHILD>(info);
            _deferredCollections.Add(part);
            return part;
        }

        public ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            var part = new ManyToManyPart<T, CHILD>(info);
            _deferredCollections.Add(part);
            return part;
        }

        public ManyToOnePart References(Expression<Func<T, object>> expression)
        {
            PropertyInfo info = ReflectionHelper.GetProperty(expression);

            var mapping = new ManyToOneMapping { PropertyInfo = info };
            _classMapping.AddReference(mapping);
            return new ManyToOnePart(mapping);
        }

        public DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
        {
            var mapping = new DiscriminatorMapping { ColumnName = columnName };
            _classMapping.Discriminator = mapping;
            return new DiscriminatorPart(mapping);
        }

        public JoinedSubclassPart<TSubclassType> JoinedSubClass<TSubclassType>(string keyColumn, Action<JoinedSubclassPart<TSubclassType>> action)
        {
            var mapping = new JoinedSubclassMapping { Type = typeof(TSubclassType), Key = new KeyMapping { Column = keyColumn } };
            _classMapping.AddSubclass(mapping);

            var part = new JoinedSubclassPart<TSubclassType>(mapping);
            action(part);
            return part;
        }
    }
}