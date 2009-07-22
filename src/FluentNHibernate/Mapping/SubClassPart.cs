using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping
{
    public class SubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclass
    {
        private readonly DiscriminatorPart parent;
        private readonly SubclassMapping mapping;
        private readonly Cache<string, string> unmigratedAttributes = new Cache<string, string>();
        private bool nextBool = true;

        public SubClassPart(DiscriminatorPart parent, object discriminatorValue)
            : this(new SubclassMapping())
        {
            this.parent = parent;

            if (discriminatorValue != null)
                mapping.DiscriminatorValue = discriminatorValue;
        }

        public SubClassPart(SubclassMapping mapping)
        {
            this.mapping = mapping;
        }

        public SubclassMapping GetSubclassMapping()
        {
            mapping.Type = typeof(TSubclass);
            
            foreach (var property in properties)
                mapping.AddProperty(property.GetPropertyMapping());

            foreach (var component in components)
                mapping.AddComponent(component.GetComponentMapping());
            
            foreach(var subclass in Subclasses)
            {
                mapping.AddSubclass(subclass.GetSubclassMapping());
            }

            foreach (var part in Parts)
                mapping.AddUnmigratedPart(part);

            unmigratedAttributes.ForEachPair(mapping.AddUnmigratedAttribute);

            return mapping;
        }

        /// <summary>
        /// Set an attribute on the xml element produced by this sub-class mapping.
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

        protected override PropertyMap Map(PropertyInfo property, string columnName)
        {
            var propertyMapping = new PropertyMapping
            {
                Name = property.Name,
                PropertyInfo = property
            };

            var propertyMap = new PropertyMap(propertyMapping);

            if (!string.IsNullOrEmpty(columnName))
                propertyMap.ColumnName(columnName);

            properties.Add(propertyMap); // new

            return propertyMap;
        }

        public override DynamicComponentPart<IDictionary> DynamicComponent(PropertyInfo property, Action<DynamicComponentPart<IDictionary>> action)
        {
            var part = new DynamicComponentPart<IDictionary>(property);
            components.Add(part);
            action(part);

            return part;
        }

        protected override ComponentPart<TComponent> Component<TComponent>(PropertyInfo property, Action<ComponentPart<TComponent>> action)
        {
            var part = new ComponentPart<TComponent>(property);
            action(part);
            components.Add(part);

            return part;
        }

        public DiscriminatorPart SubClass<TChild>(object discriminatorValue, Action<SubClassPart<TChild>> action)
        {
            var subclass = new SubClassPart<TChild>(parent, discriminatorValue);

            action(subclass);

            AddSubclass(subclass);

            return parent;
        }

        public DiscriminatorPart SubClass<TChild>(Action<SubClassPart<TChild>> action)
        {
            return SubClass(null, action);
        }

        /// <summary>
        /// Sets whether this subclass is lazy loaded
        /// </summary>
        /// <returns></returns>
        public SubClassPart<TSubclass> LazyLoad()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> Proxy(Type type)
        {
            mapping.Proxy = type;
            return this;
        }

        public SubClassPart<TSubclass> Proxy<T>()
        {
            mapping.Proxy = typeof(T);
            return this;
        }

        public SubClassPart<TSubclass> DynamicUpdate()
        {
            mapping.DynamicUpdate = nextBool;
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> DynamicInsert()
        {
            mapping.DynamicInsert = nextBool;
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> SelectBeforeUpdate()
        {
            mapping.SelectBeforeUpdate = nextBool;
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> Abstract()
        {
            mapping.Abstract = nextBool;
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public SubClassPart<TSubclass> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        void ISubclass.Proxy(Type type)
        {
            Proxy(type);
        }

        void ISubclass.Proxy<T>()
        {
            Proxy<T>();
        }

        void ISubclass.LazyLoad()
        {
            LazyLoad();
        }

        void ISubclass.DynamicUpdate()
        {
            DynamicUpdate();
        }

        void ISubclass.DynamicInsert()
        {
            DynamicInsert();
        }

        void ISubclass.SelectBeforeUpdate()
        {
            SelectBeforeUpdate();
        }

        void ISubclass.Abstract()
        {
            Abstract();
        }

        ISubclass ISubclass.Not
        {
            get { return Not; }
        }

        #region Implementation of IMappingPart

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates a constant, general position on the document the part should be written to
        /// </summary>
        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Indicates a constant sub-position within a similar grouping of positions the element will be written in
        /// </summary>
        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
