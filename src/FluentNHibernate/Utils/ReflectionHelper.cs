using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.Utils
{
    public static class ReflectionHelper
    {
        public static bool IsMethodExpression<MODEL>(Expression<Func<MODEL, object>> expression)
        {
            return IsMethodExpression<MODEL, object>(expression);
        }

        public static bool IsMethodExpression<MODEL, RETURN>(Expression<Func<MODEL, RETURN>> expression)
        {
            return expression.Body is MethodCallExpression;
        }

        public static bool IsPropertyExpression<MODEL>(Expression<Func<MODEL, object>> expression)
        {
            return getMemberExpression(expression, false) != null;
        }

        public static PropertyInfo GetProperty<MODEL>(Expression<Func<MODEL, object>> expression)
        {
            var isExpressionOfDynamicComponent = expression.ToString().Contains("get_Item");

            if (isExpressionOfDynamicComponent)
                return GetDynamicComponentProperty(expression);
		    
            var memberExpression = getMemberExpression(expression);

            return (PropertyInfo) memberExpression.Member;
        }

        private static PropertyInfo GetDynamicComponentProperty<MODEL, T>(Expression<Func<MODEL, T>> expression)
        {
            Type desiredConversionType = null;
            MethodCallExpression methodCallExpression = null;
            var nextOperand = expression.Body;

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
                
            var constExpression = methodCallExpression.Arguments[0] as ConstantExpression;
                
            return new DummyPropertyInfo((string)constExpression.Value, desiredConversionType);
        }

        public static PropertyInfo GetProperty<MODEL, T>(Expression<Func<MODEL, T>> expression)
        {
            var isExpressionOfDynamicComponent = expression.ToString().Contains("get_Item");

            if (isExpressionOfDynamicComponent)
                return GetDynamicComponentProperty<MODEL, T>(expression);

            MemberExpression memberExpression = getMemberExpression(expression);

            return (PropertyInfo)memberExpression.Member;
        }

        private static MemberExpression getMemberExpression<MODEL, T>(Expression<Func<MODEL, T>> expression)
        {
            return getMemberExpression(expression, true);
        }

        private static MemberExpression getMemberExpression<MODEL, T>(Expression<Func<MODEL, T>> expression, bool enforceCheck)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression) expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (enforceCheck && memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "member");
            }

            return memberExpression;
        }

        public static MethodInfo GetMethod<T>(Expression<Func<T, object>> expression)
        {
            MethodCallExpression methodCall = (MethodCallExpression) expression.Body;
            return methodCall.Method;
        }

        public static MethodInfo GetMethod<T, U>(Expression<Func<T, U>> expression)
        {
            MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
            return methodCall.Method;
        }

        public static MethodInfo GetMethod<T, U, V>(Expression<Func<T, U, V>> expression)
        {
            MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
            return methodCall.Method;
        }

        public static MethodInfo GetMethod(Expression<Func<object>> expression)
        {
            MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
            return methodCall.Method;
        }
    }
}