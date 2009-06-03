using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public abstract class ComponentPartBase<T> : ClasslikeMapBase<T>, IComponentBase, IAccessStrategy<ComponentPartBase<T>> 
    {
        protected string propertyName;
        protected AccessStrategyBuilder<ComponentPartBase<T>> access;
        protected ComponentMappingBase mapping;
        private bool nextBool = true;

        protected ComponentPartBase(ComponentMappingBase mapping, string propertyName)
        {
            this.mapping = mapping;
            access = new AccessStrategyBuilder<ComponentPartBase<T>>(this, value => mapping.Access = value);
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Set the access and naming strategy for this component.
        /// </summary>
        public AccessStrategyBuilder<ComponentPartBase<T>> Access
        {
            get { return access; }
        }

        ComponentMappingBase IComponentBase.GetComponentMapping()
        {
            mapping.Name = propertyName;

            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());

            return mapping;
        }

        IComponentBase IComponentBase.Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        IComponentBase IComponentBase.ReadOnly()
        {
            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;

            return this;
        }

        IComponentBase IComponentBase.Insert()
        {
            mapping.Insert = nextBool;
            nextBool = true;
            return this;
        }

        IComponentBase IComponentBase.Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
            return this;
        }

        public IComponentBase WithParentReference(Expression<Func<T, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        private IComponentBase WithParentReference(PropertyInfo property)
        {
            mapping.Parent = new ParentMapping
            {
                Name = property.Name
            };

            return this;
        }

        IComponentBase IComponentBase.WithParentReference<TExplicit>(Expression<Func<TExplicit, object>> exp)
        {
            return WithParentReference(ReflectionHelper.GetProperty(exp));
        }

        void IHasAttributes.SetAttribute(string name, string value)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttributes(Attributes atts)
        {
            throw new NotSupportedException("Obsolete");
        }

        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        public PartPosition PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }
    }
}