using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ShadeTree.Core;

namespace FluentNHibernate.Query
{
    public interface IFilterType
    {
        string Key { get; }
        Expression GetExpression(Expression memberAccessExpression, Expression valueExpression);
    }

    public class FilterTypeRegisterExpression
    {
        private readonly IFilterType _filter;

        public FilterTypeRegisterExpression(IFilterType filter)
        {
            _filter = filter;
        }

        public FilterTypeRegisterExpression ForType<T>()
        {
            FilterTypeRegistry.RegisterFilterForType(_filter, typeof (T));
            return this;
        }

        public FilterTypeRegisterExpression ForTypes(params Type[] filterableTypes)
        {
            foreach (Type filterableType in filterableTypes)
            {
                FilterTypeRegistry.RegisterFilterForType(_filter, filterableType);
            }

            return this;
        }
    }

    public static class FilterTypeRegistry
    {
        private static readonly Cache<Type, IList<IFilterType>> _filterCache =
            new Cache<Type, IList<IFilterType>>(key => new List<IFilterType>());

        static FilterTypeRegistry()
        {
            ResetAll();
        }

        public static FilterTypeRegisterExpression RegisterFilter(IFilterType filter)
        {
            return new FilterTypeRegisterExpression(filter);
        }

        public static void RegisterFilterForType(IFilterType filterType, Type filterableType)
        {
            _filterCache.Get(filterableType).Add(filterType);
        }

        public static IEnumerable<IFilterType> GetFiltersFor<T>()
        {
            return GetFiltersFor(typeof (T));
        }

        public static IEnumerable<IFilterType> GetFiltersFor(Type filterableType)
        {
            return _filterCache.Get(filterableType);
        }

        public static void ClearAll()
        {
            _filterCache.ClearAll();
        }

        public static void ResetAll()
        {
            ClearAll();

            var standardTypes = new[]
                                    {
                                        typeof (char),
                                        typeof (byte),
                                        typeof (bool),
                                        typeof (short),
                                        typeof (int),
                                        typeof (long),
                                        typeof (DateTime),
                                        typeof (sbyte),
                                        typeof (ushort),
                                        typeof (uint),
                                        typeof (ulong),
                                        typeof (char?),
                                        typeof (byte?),
                                        typeof (bool),
                                        typeof (short?),
                                        typeof (int?),
                                        typeof (long?),
                                        typeof (DateTime?),
                                        typeof (sbyte?),
                                        typeof (ushort?),
                                        typeof (uint?),
                                        typeof (ulong?)
                                    };

            RegisterFilter(new BinaryFilterType {Key = "EQUAL", FilterExpressionType = ExpressionType.Equal})
                .ForTypes(standardTypes)
                .ForType<string>();

            RegisterFilter(new BinaryFilterType {Key = "NOTEQUAL", FilterExpressionType = ExpressionType.Equal})
                .ForTypes(standardTypes)
                .ForType<string>();

            RegisterFilter(new BinaryFilterType
                               {Key = "LESSTHAN", FilterExpressionType = ExpressionType.LessThan})
                .ForTypes(standardTypes);

            RegisterFilter(new BinaryFilterType
                               {Key = "LESSTHANOREQUAL", FilterExpressionType = ExpressionType.LessThanOrEqual})
                .ForTypes(standardTypes);

            RegisterFilter(new BinaryFilterType
                               {Key = "GREATERTHAN", FilterExpressionType = ExpressionType.GreaterThan})
                .ForTypes(standardTypes);

            RegisterFilter(new BinaryFilterType
                               {Key = "GREATERTHANOREQUAL", FilterExpressionType = ExpressionType.GreaterThanOrEqual})
                .ForTypes(standardTypes);

            RegisterFilter(new StringFilterType {Key = "STARTSWITH", StringMethod = s => s.StartsWith("")})
                .ForType<string>();

            RegisterFilter(new StringFilterType {Key = "ENDSWITH", StringMethod = s => s.EndsWith("")})
                .ForType<string>();

            RegisterFilter(new StringFilterType {Key = "CONTAINS", StringMethod = s => s.Contains("")})
                .ForType<string>();
        }
    }

    public class BinaryFilterType : IFilterType
    {
        public ExpressionType FilterExpressionType { get; set; }

        #region IFilterType Members

        public string Key { get; set; }

        public Expression GetExpression(Expression memberAccessExpression, Expression valueExpression)
        {
            return Expression.MakeBinary(FilterExpressionType, memberAccessExpression, valueExpression);
        }

        #endregion
    }

    public class StringFilterType : IFilterType
    {
        private MethodInfo _stringMethodInfo;
        public Expression<Func<string, bool>> StringMethod { get; set; }

        #region IFilterType Members

        public string Key { get; set; }

        public virtual Expression GetExpression(Expression memberAccessExpression, Expression valueExpression)
        {
            if (_stringMethodInfo == null)
            {
                _stringMethodInfo = ReflectionHelper.GetMethod(StringMethod);
            }

            return Expression.Call(memberAccessExpression, _stringMethodInfo, valueExpression);
        }

        #endregion
    }
}