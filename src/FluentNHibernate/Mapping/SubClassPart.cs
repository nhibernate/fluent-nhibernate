using System;
using System.Reflection;
using System.Xml;
using FluentNHibernate.MappingModel;

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
            
            foreach (var property in Properties)
                mapping.AddProperty(property.GetPropertyMapping());

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
            var propertyMapping = new PropertyMapping()
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

        public DiscriminatorPart SubClass<TChild>(object discriminatorValue, Action<SubClassPart<TChild>> action)
        {
            var subclass = new SubClassPart<TChild>(parent, discriminatorValue);

            action(subclass);

            mapping.AddSubclass(subclass.GetSubclassMapping());

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
            mapping.LazyLoad = nextBool;
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

        ISubclass ISubclass.LazyLoad()
        {
            return LazyLoad();
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
