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
            mapping.Lazy = nextBool;
            nextBool = true;
            return this;
        }

        public SubClassPart<TSubclass> Proxy(Type type)
        {
            mapping.Proxy = type.AssemblyQualifiedName;
            return this;
        }

        public SubClassPart<TSubclass> Proxy<T>()
        {
            return Proxy(typeof(T));
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

        void IMappingPart.Write(XmlElement classElement, IMappingVisitor visitor)
        {
            throw new NotSupportedException("Obsolete");
        }

        PartPosition IMappingPart.PositionOnDocument
        {
            get { throw new NotSupportedException("Obsolete"); }
        }
        
        int IMappingPart.LevelWithinPosition
        {
            get { throw new NotSupportedException("Obsolete"); }
        }

        void IHasAttributes.SetAttribute(string name, string value)
        {
            throw new NotSupportedException("Obsolete");
        }

        void IHasAttributes.SetAttributes(Attributes atts)
        {
            throw new NotSupportedException("Obsolete");
        }
    }
}
