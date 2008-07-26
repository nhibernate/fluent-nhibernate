using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Util;
using ShadeTree.Core;

namespace ShadeTree.DomainModel.Query
{


    public class EntityQueryDefinition
    {
        private readonly IDictionary<string, FilterableProperty> _filterProperties;
        private readonly IDictionary<string, FilterableProperty> _quickFilterProperties;

        public EntityQueryDefinition()
        {
            _filterProperties = new Dictionary<string, FilterableProperty>();
            _quickFilterProperties = new Dictionary<string, FilterableProperty>();
        }

        public IEnumerable<FilterableProperty> FilterableProperties
        {
            get { return _filterProperties.Values; }
        }

        public IEnumerable<FilterableProperty> QuickFilterProperties
        {
            get { return _quickFilterProperties.Values; }
        }

        public FilterableProperty GetFilterPropertyForKey(string key)
        {
            return _filterProperties[key];
        }
        
        public void AddQuickFilterProperty(FilterableProperty prop)
        {
            _quickFilterProperties.Add(prop.PropertyName, prop);
            AddFilterProperty(prop);
        }

        public void AddFilterProperty(FilterableProperty prop)
        {
            _filterProperties.Add(prop.PropertyName, prop);
        }
    }

    public class FilterableProperty
    {
        public static FilterableProperty Build<T,U>(Expression<Func<T, U>> expression)
        {
            return new FilterableProperty(){Expression = expression,Property = ReflectionHelper.GetProperty(expression)};
        }


        public Expression Expression{ get; set; }
        public PropertyInfo Property { get; set; }

        public IFilterType[] Filters
        {
            get
            {
                return FilterTypeRegistry.GetFiltersFor(PropertyType).ToArray();
            }
        }

        public string PropertyName
        {
            get { return Property.Name; }
        }

        public Type PropertyType
        {
            get { return Property.PropertyType; }
        }
    }

    public class EntityQueryDefinitionBuilder<ENTITY>
    {
        private readonly EntityQueryDefinition _queryDef = new EntityQueryDefinition();

        public EntityQueryDefinitionBuilder<ENTITY> AllowFilterOn<T>(Expression<Func<ENTITY, T>> expression)
        {
            AddFilterProperty(expression, false);
            return this;
        }

        public EntityQueryDefinitionBuilder<ENTITY> AddQuickFilterFor<T>(Expression<Func<ENTITY, T>> expression)
        {
            AddFilterProperty(expression, true);
            return this;
        }

        private void AddFilterProperty<T>(Expression<Func<ENTITY, T>> expression, bool isQuickFilter)
        {
            Accessor accessor = ReflectionHelper.GetAccessor(expression);

            var filterProp = new FilterableProperty { Property = accessor.InnerProperty, Expression = expression };

            if( isQuickFilter )
            {
                _queryDef.AddQuickFilterProperty(filterProp);
            }
            else
            {
                _queryDef.AddFilterProperty(filterProp);
            }
        }

        public EntityQueryDefinition QueryDefinition { get { return _queryDef; } }
    }

    public class Criteria
    {
        public string property{ get; set;}
        public string op{ get; set;}
        public string value{ get; set; }
    }

    public interface IEntityQueryBuilder
    {
        IEnumerable PrepareQuery(Criteria[] criteria, IRepository repo);
    }

    public class EntityQueryBuilder<ENTITY> : IEntityQueryBuilder
    {
        private readonly IEnumerable<ParameterExpression> _expressionParams = new[] { Expression.Parameter(typeof(ENTITY), "target") };
        private readonly EntityQueryDefinition _queryDef;

        public EntityQueryBuilder(EntityQueryDefinition queryDef)
        {
            _queryDef = queryDef;

        }

        public Expression<Func<ENTITY, bool>> FilterExpression { get; set; }

        public IEnumerable PrepareQuery(Criteria[] criteria, IRepository repo)
        {
            FilterExpression = null;

            foreach( var criterion in criteria )
            {
                FilterableProperty filterProp = _queryDef.GetFilterPropertyForKey(criterion.property);

                string filterTypeKey = criterion.op;

                IFilterType filterType =
                    FilterTypeRegistry.GetFiltersFor(filterProp.PropertyType).Single(p => p.Key == filterTypeKey);

                AddFilter(filterType, filterProp, criterion.value);
            }

            return repo.Query(FilterExpression);
        }

        public void AddFilter(IFilterType filterType, FilterableProperty filterProp, string valueAsString)
        {
            var getMemberValueExpr = Expression.Invoke(filterProp.Expression, _expressionParams.Cast<Expression>());

            var valueExpr = ChangeTypeIfNecessary(filterProp, valueAsString);

            var filterExpr = filterType.GetExpression(getMemberValueExpr, valueExpr);

            var curExpr = (FilterExpression == null)
                ? filterExpr
                : Expression.AndAlso(FilterExpression.Body, filterExpr);

            FilterExpression = Expression.Lambda<Func<ENTITY, bool>>(curExpr, _expressionParams);
        }

        private static Expression ChangeTypeIfNecessary(FilterableProperty filterProp, string valueAsString)
        {
            if (filterProp.PropertyType == typeof(string))
            {
                return Expression.Constant(valueAsString);
            }
            
            var changeTypeMethod = typeof (Convert).GetMethod("ChangeType",
                                                              new[] {typeof (object), typeof (Type)});

            var valueExpr = Expression.Call(changeTypeMethod, Expression.Constant(valueAsString),
                                            Expression.Constant(filterProp.PropertyType));

            return Expression.Convert(valueExpr, filterProp.PropertyType);
        }
    }
}