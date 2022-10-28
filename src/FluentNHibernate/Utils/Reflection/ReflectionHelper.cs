using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace FluentNHibernate.Utils.Reflection
{
    public static class ReflectionHelper
    {
        public static Type AutomappingTypeForEntityType(Type entityType)
        {
            return typeof(AutoMapping<>).MakeGenericType(entityType);
        }

        public static Member GetMember<TModel, TReturn>(Expression<Func<TModel, TReturn>> expression)
        {
            return GetMember(expression.Body);
        }

        public static Member GetMember<TModel>(Expression<Func<TModel, object>> expression)
        {
            return GetMember(expression.Body);
        }

        public static Accessor GetAccessor<MODEL>(Expression<Func<MODEL, object>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression.Body);

            return getAccessor(memberExpression);
        }

        public static Accessor GetAccessor<MODEL, T>(Expression<Func<MODEL, T>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression.Body);

            return getAccessor(memberExpression);
        }

        private static bool IsIndexedPropertyAccess(Expression expression)
        {
            return IsMethodExpression(expression) && expression.ToString().Contains("get_Item");
        }

        private static bool IsMethodExpression(Expression expression)
        {
            return expression is MethodCallExpression || (expression is UnaryExpression && IsMethodExpression((expression as UnaryExpression).Operand));
        }

        private static Member GetMember(Expression expression)
        {
            if (IsIndexedPropertyAccess(expression))
                return GetDynamicComponentProperty(expression).ToMember();
            if (IsMethodExpression(expression))
                return ((MethodCallExpression)expression).Method.ToMember();

            var memberExpression = GetMemberExpression(expression);

            return memberExpression.Member.ToMember();
        }

        private static PropertyInfo GetDynamicComponentProperty(Expression expression)
        {
            Type desiredConversionType = null;
            MethodCallExpression methodCallExpression = null;
            var nextOperand = expression;

            while (nextOperand != null)
            {
                if (nextOperand.NodeType == ExpressionType.Call)
                {
                    methodCallExpression = nextOperand as MethodCallExpression;
                    desiredConversionType = desiredConversionType ?? methodCallExpression.Method.ReturnType;
                    break;
                }

                if (nextOperand.NodeType != ExpressionType.Convert)
                    throw new ArgumentException("Expression not supported", "expression");
	            
                var unaryExpression = (UnaryExpression)nextOperand;
                desiredConversionType = unaryExpression.Type;
                nextOperand = unaryExpression.Operand;
            }
            var expr = DynamicComponentPropertyVisitor.Visit(methodCallExpression.Arguments[0]);
            var value = (expr as ConstantExpression)?.Value;
            
            return new DummyPropertyInfo((string)value, desiredConversionType);
        }
        private static readonly Visitor DynamicComponentPropertyVisitor = new Visitor();
        private class Visitor:ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression memberExpression)
            {
                var expression = Visit(memberExpression.Expression);
                object container = expression is ConstantExpression constantExpression ? constantExpression.Value : null;
                var member = memberExpression.Member;
                switch (member)
                {
                    case FieldInfo fieldInfo: 
                        return Expression.Constant(fieldInfo.GetValue(container));
                    case PropertyInfo propertyInfo: 
                        return Expression.Constant(propertyInfo.GetValue(container, null));
                    default: 
                        return base.VisitMember(memberExpression);
                }
            }
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            return GetMemberExpression(expression, true);
        }

        private static MemberExpression GetMemberExpression(Expression expression, bool enforceCheck)
        {
            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression) expression;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            if (enforceCheck && memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }

            return memberExpression;
        }

        private static Accessor getAccessor(MemberExpression memberExpression)
        {
            var list = new List<Member>();

            while (memberExpression != null)
            {
                list.Add(memberExpression.Member.ToMember());
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            if (list.Count == 1)
            {
                return new SingleMember(list[0]);
            }

            list.Reverse();
            return new PropertyChain(list.ToArray());
        }
    }
}