using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Mapping
{
    public interface IManyToOnePart : IRelationship
    {
        CascadeExpression<IManyToOnePart> Cascade { get; }
        string GetColumnName();
        PropertyInfo Property { get; }
        IManyToOnePart ColumnName(string columnName);
        INotFoundExpression NotFound { get; }
    }

    public class ManyToOnePart<OTHER> : IManyToOnePart, IAccessStrategy<ManyToOnePart<OTHER>>
    {
		private readonly Cache<string, string> _properties = new Cache<string, string>();
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private string columnName;
        private readonly AccessStrategyBuilder<ManyToOnePart<OTHER>> access;
        private readonly IList<string> _columns = new List<string>();
        private bool nextBool = true;

        public ManyToOnePart(Type entity, PropertyInfo property) 
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<ManyToOnePart<OTHER>>(this);

            Property = property;
        }

		public FetchTypeExpression<ManyToOnePart<OTHER>> FetchType
		{
			get
			{
				return new FetchTypeExpression<ManyToOnePart<OTHER>>(this, _properties);
			}
		}

        public NotFoundExpression<ManyToOnePart<OTHER>> NotFound
        {
            get
            {
                return new NotFoundExpression<ManyToOnePart<OTHER>>( this, _properties );
            }
        }

        INotFoundExpression IManyToOnePart.NotFound
        {
            get { return NotFound; }
        }

        public ManyToOnePart<OTHER> PropertyRef(Expression<Func<OTHER, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            _properties.Store("property-ref", prop.Name);

            return this;
        }

        public ManyToOnePart<OTHER> Unique()
        {
            _properties.Store("unique", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public ManyToOnePart<OTHER> UniqueKey(string keyName)
        {
            _properties.Store("unique-key", keyName);
            return this;
        }

        public ManyToOnePart<OTHER> LazyLoad()
        {
            _properties.Store("lazy", nextBool ? "proxy" : "false");
            nextBool = true;
            return this;
        }
		
		public ManyToOnePart<OTHER> WithForeignKey()
		{
			return WithForeignKey(string.Format("FK_{0}To{1}", Property.DeclaringType.Name, Property.Name));
		}
		
		public ManyToOnePart<OTHER> WithForeignKey(string foreignKeyName)
		{
			_properties.Store("foreign-key", foreignKeyName);
			return this;
		}

        public ManyToOnePart<OTHER> WithColumns(params string[] columns)
        {
            foreach (var column in columns)
            {
                _columns.Add(column);
            }

            return this;
        }

        public ManyToOnePart<OTHER> WithColumns(params Expression<Func<OTHER, object>>[] columns)
        {
            foreach (var expression in columns)
            {
                var property = ReflectionHelper.GetProperty(expression);

                WithColumns(property.Name);
            }

            return this;
        }
		
		public CascadeExpression<ManyToOnePart<OTHER>> Cascade
		{
			get
			{
				return new CascadeExpression<ManyToOnePart<OTHER>>(this);
			}
		}

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
        	_properties.Store("name", Property.Name);

            if (_columns.Count == 0)
                _properties.Store("column", columnName);

            var manyToOneElement = classElement.AddElement("many-to-one").WithProperties(_properties);

            if (_columns.Count > 0)
            {
                foreach (var column in _columns)
                {
                    manyToOneElement.AddElement("column")
                        .WithAtt("name", column);
                }
            }

        }

        public void SetAttribute(string name, string value)
        {
			_properties.Store(name, value);
        }

        public void SetAttributes(Attributes atts)
        {
            foreach (var key in atts.Keys)
            {
                SetAttribute(key, atts[key]);
            }
        }

        IManyToOnePart IManyToOnePart.ColumnName(string name)
        {
            return ColumnName(name);
        }

        public ManyToOnePart<OTHER> ColumnName(string name)
        {
            columnName = name;

            return this;
        }

        public string GetColumnName()
        {
            return columnName;
        }

        public int LevelWithinPosition
        {
            get { return 1; }
        }

        public PartPosition PositionOnDocument
        {
            get { return PartPosition.Anywhere; }
        }

        public ManyToOnePart<OTHER> Nullable()
        {
            SetAttribute("not-null", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        public AccessStrategyBuilder<ManyToOnePart<OTHER>> Access
        {
            get { return access; }
        }

        public void PropertyRef(Func<object, object> func)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        public ManyToOnePart<OTHER> Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        #region Explicit IManyToOnePart Implementation
        CascadeExpression<IManyToOnePart> IManyToOnePart.Cascade
        {
            get { return new CascadeExpression<IManyToOnePart>(this); }
        }

        IAccessStrategyBuilder IRelationship.Access
        {
            get { return Access; }
        }

        #endregion
    }
}
