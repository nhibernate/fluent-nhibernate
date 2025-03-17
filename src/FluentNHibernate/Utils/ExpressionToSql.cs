using System;
using System.Linq.Expressions;

namespace FluentNHibernate.Utils;

/// <summary>
/// Converts an expression to a best guess SQL string
/// </summary>
public class ExpressionToSql
{
    /// <summary>
    /// Converts a Func expression to a best guess SQL string
    /// </summary>
    public static string Convert<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression)
            return Convert(expression, (MemberExpression)expression.Body);
        if (expression.Body is MethodCallExpression)
            return Convert((MethodCallExpression)expression.Body);
        if (expression.Body is UnaryExpression)
            return Convert(expression, (UnaryExpression)expression.Body);
        if (expression.Body is ConstantExpression)
            return Convert((ConstantExpression)expression.Body);

        throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    /// <summary>
    /// Converts a boolean Func expression to a best guess SQL string
    /// </summary>
    public static string Convert<T>(Expression<Func<T, bool>> expression)
    {
        if (expression.Body is BinaryExpression)
            return Convert<T>((BinaryExpression)expression.Body);
        if (expression.Body is MemberExpression memberExpression && memberExpression.Type == typeof(bool))
            return Convert(CreateExpression<T>(memberExpression)) + " = " + Convert(true);
        if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Type == typeof(bool) && unaryExpression.NodeType == ExpressionType.Not)
            return Convert(CreateExpression<T>(unaryExpression.Operand)) + " = " + Convert(false);

        throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    static string Convert<T>(Expression<Func<T, object>> expression, MemberExpression body)
    {
        // TODO: should really do something about conventions and overridden names here
        var member = body.Member;

        if (member.DeclaringType.IsAssignableFrom(typeof(T)))
            return member.Name;
                
        // try get value of lambda, hoping it's just a direct value return or local reference
        var compiledExpression = expression.Compile();
        var value = compiledExpression(default(T)); // give it null/default because a value will not need anything

        return Convert(value);
    }

    /// <summary>
    /// Gets the value of a method call.
    /// </summary>
    /// <param name="body">Method call expression</param>
    static string Convert(MethodCallExpression body)
    {
        return Convert(body.Method.Invoke(body.Object, null));
    }

    static string Convert<T>(Expression<Func<T, object>> expression, UnaryExpression body)
    {

        if (body.Operand is ConstantExpression constant)
            return Convert(constant);

        if (body.Operand is MemberExpression member)
            return Convert(expression, member);

        if (body.Operand is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
            return Convert(expression, unaryExpression);

        throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    static string Convert(ConstantExpression expression)
    {
        if (expression.Type.IsEnum)
            return Convert((int)expression.Value);

        return Convert(expression.Value);
    }

    static Expression<Func<T, object>> CreateExpression<T>(Expression body)
    {
        var expression = body;
        var parameter = Expression.Parameter(typeof(T), "x");

        if (expression.Type.IsValueType)
            expression = Expression.Convert(expression, typeof(object));

        return (Expression<Func<T, object>>)Expression.Lambda(typeof(Func<T, object>), expression, parameter);
    }

    static string Convert<T>(BinaryExpression expression)
    {
        var left = Convert(CreateExpression<T>(expression.Left));
        var right = Convert(CreateExpression<T>(expression.Right));
        string op;

        switch (expression.NodeType)
        {
            default:
            case ExpressionType.Equal:
                op = "=";
                break;
            case ExpressionType.GreaterThan:
                op = ">";
                break;
            case ExpressionType.GreaterThanOrEqual:
                op = ">=";
                break;
            case ExpressionType.LessThan:
                op = "<";
                break;
            case ExpressionType.LessThanOrEqual:
                op = "<=";
                break;
            case ExpressionType.NotEqual:
                op = "!=";
                break;
        }

        return left + " " + op + " " + right;
    }

    static string Convert(object value)
    {
        if (value is string)
            return "'" + value + "'";
        if (value is bool)
        {
            return (bool)value ? "1" : "0";
        }
            
        return value.ToString();
    }
}
