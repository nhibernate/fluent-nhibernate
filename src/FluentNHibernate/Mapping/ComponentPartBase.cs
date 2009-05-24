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
        protected readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        protected ComponentMappingBase mapping;
        private bool nextBool = true;

        public ComponentPartBase(ComponentMappingBase mapping, string propertyName)
        {
            this.mapping = mapping;
            access = new AccessStrategyBuilder<ComponentPartBase<T>>(this, value => SetAttribute("access", value));
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

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

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

        /// <summary>
        /// Set an attribute on the xml element produced by this component mapping.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void SetAttribute(string name, string value)
        {
            unmigratedAttributes.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        public override DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property);
            action(part);
            components.Add(part);

            return part;
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(property);
            action(part);
            components.Add(part);

            return part;
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

        public int LevelWithinPosition
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