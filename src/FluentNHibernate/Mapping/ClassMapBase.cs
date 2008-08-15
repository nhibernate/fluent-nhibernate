using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace FluentNHibernate.Mapping
{
    public class ClassMapBase<T>
    {
        private bool _parentIsRequired = true;
        
        protected readonly List<IMappingPart> _properties = new List<IMappingPart>();


        protected bool parentIsRequired
        {
            get { return _parentIsRequired; }
            set { _parentIsRequired = value; }
        }

        protected void addPart(IMappingPart part)
        {
            _properties.Add(part);
        }

        public virtual PropertyMap Map(Expression<Func<T, object>> expression)
        {
            return Map(expression, null);
        }

        public virtual PropertyMap Map(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            var map = new PropertyMap(property, parentIsRequired, typeof(T));

            if (columnName != null)
                map.TheColumnNameIs(columnName);

            _properties.Add(map);

            return map;
        }

        public virtual ManyToOnePart References(Expression<Func<T, object>> expression)
        {
            return References(expression, null);
        }

        public virtual ManyToOnePart References(Expression<Func<T, object>> expression, string columnName)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            ManyToOnePart part = new ManyToOnePart(property, columnName);
            addPart(part);

            return part;
        }

        public virtual DiscriminatorPart<ARG, T> DiscriminateSubClassesOnColumn<ARG>(string columnName, ARG baseClassDiscriminator) 
		{
			var part = new DiscriminatorPart<ARG, T>(columnName, _properties, baseClassDiscriminator);
			addPart(part);

			return part;
		}

        public virtual DiscriminatorPart<ARG, T> DiscriminateSubClassesOnColumn<ARG>(string columnName)
        {
            var part = new DiscriminatorPart<ARG, T>(columnName, _properties);
            addPart(part);

            return part;
        }

        public virtual ComponentPart<C> Component<C>(Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);

            ComponentPart<C> part = new ComponentPart<C>(property, parentIsRequired);
            addPart(part);

            action(part);

            return part;
        }

        public virtual OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            OneToManyPart<T, CHILD> part = new OneToManyPart<T, CHILD>(property);

            addPart(part);

            return part;
        }

        public virtual ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(Expression<Func<T, object>> expression)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            ManyToManyPart<T, CHILD> part = new ManyToManyPart<T, CHILD>(property);

            addPart(part);

            return part;
        }

        public virtual VersionPart Version(Expression<Func<T, object>> expression)
        {
            var versionPart = new VersionPart(ReflectionHelper.GetProperty(expression));
            addPart(versionPart);
            return versionPart;
        }

        protected void writeTheParts(XmlElement classElement, IMappingVisitor visitor)
        {
            _properties.Sort(new MappingPartComparer());
            foreach (IMappingPart part in _properties)
            {
                part.Write(classElement, visitor);
            }
        }

        internal class MappingPartComparer : IComparer<IMappingPart>
        {
            public int Compare(IMappingPart x, IMappingPart y)
            {
                return x.Level.CompareTo(y.Level);
            }
        }
    }
}
