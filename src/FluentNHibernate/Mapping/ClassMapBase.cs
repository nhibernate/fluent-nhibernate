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
        private readonly List<IMappingPart> _properties = new List<IMappingPart>();

        protected bool parentIsRequired
        {
            get { return _parentIsRequired; }
            set { _parentIsRequired = value; }
        }

        protected void AddPart(IMappingPart part)
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

        public virtual ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            return References(expression, null);
        }

        public virtual ManyToOnePart<OTHER> References<OTHER>(Expression<Func<T, OTHER>> expression, string columnName)
        {
            var property = ReflectionHelper.GetProperty(expression);
            var part = new ManyToOnePart<OTHER>(property);

            if (columnName != null)
                part.TheColumnNameIs(columnName);

            AddPart(part);

            return part;
        }

        public virtual OneToOnePart<OTHER> HasOne<OTHER>(Expression<Func<T, OTHER>> expression)
        {
            var property = ReflectionHelper.GetProperty(expression);
            var part = new OneToOnePart<OTHER>(property);
            AddPart(part);

            return part;
        }

        public virtual DiscriminatorPart<ARG, T> DiscriminateSubClassesOnColumn<ARG>(string columnName, ARG baseClassDiscriminator) 
		{
			var part = new DiscriminatorPart<ARG, T>(columnName, _properties, baseClassDiscriminator);
			AddPart(part);

			return part;
		}

        public virtual DiscriminatorPart<ARG, T> DiscriminateSubClassesOnColumn<ARG>(string columnName)
        {
            var part = new DiscriminatorPart<ARG, T>(columnName, _properties);
            AddPart(part);

            return part;
        }

        public virtual ComponentPart<C> Component<C>(Expression<Func<T, object>> expression, Action<ComponentPart<C>> action)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);

            ComponentPart<C> part = new ComponentPart<C>(property, parentIsRequired);
            AddPart(part);

            action(part);

            return part;
        }

        public virtual OneToManyPart<T, CHILD> HasMany<CHILD>(Expression<Func<T, object>> expression)
        {
            var part = ReflectionHelper.IsMethodExpression(expression)
                               ? new OneToManyPart<T, CHILD>(ReflectionHelper.GetMethod(expression))
                               : new OneToManyPart<T, CHILD>(ReflectionHelper.GetProperty(expression));

            AddPart(part);

            return part;
        }

        public virtual ManyToManyPart<T, CHILD> HasManyToMany<CHILD>(Expression<Func<T, object>> expression)
        {
            var part = ReflectionHelper.IsMethodExpression(expression)
                               ? new ManyToManyPart<T, CHILD>(ReflectionHelper.GetMethod(expression))
                               : new ManyToManyPart<T, CHILD>(ReflectionHelper.GetProperty(expression));

            AddPart(part);

            return part;
        }

        public virtual VersionPart Version(Expression<Func<T, object>> expression)
        {
            var versionPart = new VersionPart(ReflectionHelper.GetProperty(expression));
            AddPart(versionPart);
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
    }
}
