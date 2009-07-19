using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ComponentPartBase<T> : ClasslikeMapBase<T>, IComponentMappingProvider
    {
        private readonly string propertyName;
        private readonly AccessStrategyBuilder<ComponentPartBase<T>> access;
        protected ComponentMappingBase mapping;
        private bool nextBool = true;

        protected ComponentPartBase(ComponentMappingBase mapping, string propertyName)
        {
            this.mapping = mapping;
            access = new AccessStrategyBuilder<ComponentPartBase<T>>(this, value => mapping.Access = value);
            this.propertyName = propertyName;
        }

        IComponentMapping IComponentMappingProvider.GetComponentMapping()
        {
            mapping.Name = propertyName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            foreach (var oneToOne in oneToOnes)
                mapping.AddOneToOne(oneToOne.GetOneToOneMapping());

            foreach (var collection in collections)
                mapping.AddCollection(collection.GetCollectionMapping());

            foreach (var reference in references)
                mapping.AddReference(reference.GetManyToOneMapping());

            foreach (var any in anys)
                mapping.AddAny(any.GetAnyMapping());

            return mapping;
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<ComponentPartBase<T>> Access
        {
            get { return access; }
        }

        public ComponentPartBase<T> ParentReference(Expression<Func<T, object>> exp)
        {
            return ParentReference(ReflectionHelper.GetProperty(exp));
        }

        private ComponentPartBase<T> ParentReference(PropertyInfo property)
        {
            mapping.Parent = new ParentMapping
            {
                Name = property.Name,
                ContainingEntityType = typeof(T)
            };

            return this;
        }

        public ComponentPartBase<T> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public ComponentPartBase<T> ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;

            return this;
        }

        public ComponentPartBase<T> Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        public ComponentPartBase<T> Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
         }

    }
}