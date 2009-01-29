using System;
using System.Linq.Expressions;

namespace FluentNHibernate
{
    /// <summary>
    /// Converts an expression to a best guess SQL string
    /// </summary>
    public class ExpressionToSql
    {
        /// <summary>
        /// Converts a Func expression to a buest guess SQL string
        /// </summary>
        public static string Convert<T>(Expression<Func<T, object>> expression)
        {
            return Convert(expression.Body);
        }

        /// <summary>
        /// Converts a boolean Func expression to a buest guess SQL string
        /// </summary>
        public static string Convert<T>(Expression<Func<T, bool>> expression)
        {
            if (expression.Body is BinaryExpression)
                return Convert((BinaryExpression)expression.Body);

            throw new NotImplementedException();
        }

        private static string Convert(Expression expression)
        {
            if (expression is MemberExpression)
                return Convert((MemberExpression)expression);
            if (expression is UnaryExpression)
                return Convert((UnaryExpression)expression);
            if (expression is ConstantExpression)
                return Convert((ConstantExpression)expression);

            throw new NotImplementedException();
        }

        private static string Convert(MemberExpression expression)
        {
            // TODO: should really do something about conventions and overridden names here
            return expression.Member.Name;
        }

        private static string Convert(UnaryExpression expression)
        {
            var constant = expression.Operand as ConstantExpression;

            if (constant != null)
                return Convert(constant);

            throw new NotImplementedException();
        }

        private static string Convert(ConstantExpression expression)
        {
            return Convert(expression.Value);
        }

        private static string Convert(BinaryExpression expression)
        {
            var left = Convert(expression.Left);
            var right = Convert(expression.Right);
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

        private static string Convert(object value)
        {
            if (value is string)
                return "'" + value + "'";
            
            return value.ToString();
        }
    }
}