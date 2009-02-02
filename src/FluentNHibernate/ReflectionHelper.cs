using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate
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

	    private static PropertyInfo GetDynamicComponentProperty<MODEL>(Expression<Func<MODEL, object>> expression)
	    {
	        Type desiredConversionType = null;
	        MethodCallExpression methodCallExpression = null;
	        var nextOperand = expression.Body;

	        while (nextOperand != null)
	        {
	            if (nextOperand.NodeType == ExpressionType.Call)
	            {
	                methodCallExpression = nextOperand as MethodCallExpression;
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

		public static Accessor GetAccessor<MODEL>(Expression<Func<MODEL, object>> expression)
		{
			MemberExpression memberExpression = getMemberExpression(expression);

			return getAccessor(memberExpression);
		}

		private static Accessor getAccessor(MemberExpression memberExpression)
		{
			var list = new List<PropertyInfo>();

			while (memberExpression != null)
			{
				list.Add((PropertyInfo) memberExpression.Member);
				memberExpression = memberExpression.Expression as MemberExpression;
			}

			if (list.Count == 1)
			{
				return new SingleProperty(list[0]);
			}

			list.Reverse();
			return new PropertyChain(list.ToArray());
		}

		public static Accessor GetAccessor<MODEL, T>(Expression<Func<MODEL, T>> expression)
		{
			MemberExpression memberExpression = getMemberExpression(expression);

			return getAccessor(memberExpression);
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

    public static class InvocationHelper
    {
        public static object InvokeGenericMethodWithDynamicTypeArguments<T>(T target, Expression<Func<T, object>> expression, object[] methodArguments, params Type[] typeArguments)
        {
            var methodInfo = ReflectionHelper.GetMethod(expression);
            if (methodInfo.GetGenericArguments().Length != typeArguments.Length)
                throw new ArgumentException(
                    string.Format("The method '{0}' has {1} type argument(s) but {2} type argument(s) were passed. The amounts must be equal.",
                    methodInfo.Name,
                    methodInfo.GetGenericArguments().Length,
                    typeArguments.Length));

            return methodInfo
                .GetGenericMethodDefinition()
                .MakeGenericMethod(typeArguments)
                .Invoke(target, methodArguments);
        }
    }

    
}
