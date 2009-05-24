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

    public class ManyToOnePart<TOther> : IManyToOnePart, IAccessStrategy<ManyToOnePart<TOther>>
    {
		private readonly Cache<string, string> properties = new Cache<string, string>();
        public PropertyInfo Property { get; private set; }
        public Type EntityType { get; private set; }
        private string columnName;
        private readonly AccessStrategyBuilder<ManyToOnePart<TOther>> access;
        private readonly FetchTypeExpression<ManyToOnePart<TOther>> fetch;
        private readonly IList<string> columns = new List<string>();
        private bool nextBool = true;

        public ManyToOnePart(Type entity, PropertyInfo property) 
        {
            EntityType = entity;
            access = new AccessStrategyBuilder<ManyToOnePart<TOther>>(this, value => SetAttribute("access", value));
            fetch = new FetchTypeExpression<ManyToOnePart<TOther>>(this, value => SetAttribute("fetch", value));

            Property = property;
        }

		public FetchTypeExpression<ManyToOnePart<TOther>> FetchType
		{
			get { return fetch; }
		}

        public NotFoundExpression<ManyToOnePart<TOther>> NotFound
        {
            get
            {
                return new NotFoundExpression<ManyToOnePart<TOther>>( this, properties );
            }
        }

        INotFoundExpression IManyToOnePart.NotFound
        {
            get { return NotFound; }
        }

        public ManyToOnePart<TOther> PropertyRef(Expression<Func<TOther, object>> propRefExpression)
        {
            var prop = ReflectionHelper.GetProperty(propRefExpression);
            properties.Store("property-ref", prop.Name);

            return this;
        }

        public ManyToOnePart<TOther> Unique()
        {
            properties.Store("unique", nextBool.ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specifies the name of a multi-column unique constraint.
        /// </summary>
        /// <param name="keyName">Name of constraint</param>
        public ManyToOnePart<TOther> UniqueKey(string keyName)
        {
            properties.Store("unique-key", keyName);
            return this;
        }

        public ManyToOnePart<TOther> LazyLoad()
        {
            properties.Store("lazy", nextBool ? "proxy" : "false");
            nextBool = true;
            return this;
        }
		
		public ManyToOnePart<TOther> WithForeignKey()
		{
			return WithForeignKey(string.Format("FK_{0}To{1}", Property.DeclaringType.Name, Property.Name));
		}
		
		public ManyToOnePart<TOther> WithForeignKey(string foreignKeyName)
		{
			properties.Store("foreign-key", foreignKeyName);
			return this;
		}

        public ManyToOnePart<TOther> WithColumns(params string[] columns)
        {
            foreach (var column in columns)
            {
                this.columns.Add(column);
            }

            return this;
        }

        public ManyToOnePart<TOther> WithColumns(params Expression<Func<TOther, object>>[] columns)
        {
            foreach (var expression in columns)
            {
                var property = ReflectionHelper.GetProperty(expression);

                WithColumns(property.Name);
            }

            return this;
        }
		
		public CascadeExpression<ManyToOnePart<TOther>> Cascade
		{
			get
			{
				return new CascadeExpression<ManyToOnePart<TOther>>(this);
			}
		}

        public void Write(XmlElement classElement, IMappingVisitor visitor)
        {
        	properties.Store("name", Property.Name);

            if (columns.Count == 0)
                properties.Store("column", columnName);

            var manyToOneElement = classElement.AddElement("many-to-one").WithProperties(properties);

            if (columns.Count > 0)
            {
                foreach (var column in columns)
                {
                    manyToOneElement.AddElement("column")
                        .WithAtt("name", column);
                }
            }

        }

        public void SetAttribute(string name, string value)
        {
			properties.Store(name, value);
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

        public ManyToOnePart<TOther> ColumnName(string name)
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

        public ManyToOnePart<TOther> Nullable()
        {
            SetAttribute("not-null", (!nextBool).ToString().ToLowerInvariant());
            nextBool = true;
            return this;
        }

        public AccessStrategyBuilder<ManyToOnePart<TOther>> Access
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
        public ManyToOnePart<TOther> Not
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
