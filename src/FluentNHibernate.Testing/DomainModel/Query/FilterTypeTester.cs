using System;
using System.Linq.Expressions;
using NUnit.Framework;
using FluentNHibernate.Query;

namespace FluentNHibernate.Testing.DomainModel.Query
{
    [TestFixture]
    public class StringFilterTypeTester : FilterTypeSpec<StringFilterType, FilterTestEntity>
    {
        [Test]
        public void StringFilterType_should_generate_an_expression_that_calls_the_designated_string_method_on_the_value()
        {
            _filter.StringMethod = s => s.StartsWith("");
            ForMember(t=>t.Name).AndValue("Foo").TheResultExpression.ShouldEqual("Invoke(t => t.Name,target).StartsWith(\"Foo\")");
        }
    }

    public class BinaryFilterTypeTester : FilterTypeSpec<BinaryFilterType, FilterTestEntity>
    {
        [Test]
        public void BinaryFilterType_should_generate_an_expression_that_calls_the_correct_operator_on_the_value()
        {
            _filter.FilterExpressionType = ExpressionType.GreaterThan;
            ForMember(t => t.Age).AndValue(99).TheResultExpression.ShouldEqual("(Invoke(t => t.Age,target) > 99)");
        }
    }

    public class FilterTypeSpec<FILTERTYPE, ENTITYTYPE> where FILTERTYPE : IFilterType, new()
    {
        protected FILTERTYPE _filter = new FILTERTYPE();
        private ConstantExpression _valueExpr;
        private InvocationExpression _memberAccessExpr;

       public FilterTypeSpec<FILTERTYPE, ENTITYTYPE> ForMember<VALUE>(Expression<Func<ENTITYTYPE, VALUE>> memberExpression)
        {
            _memberAccessExpr = Expression.Invoke(memberExpression, Expression.Parameter(typeof(ENTITYTYPE), "target"));
            return this;
        }

        public FilterTypeSpec<FILTERTYPE, ENTITYTYPE> AndValue<VALUE>(VALUE value)
        {
            _valueExpr = Expression.Constant(value);
            return this;
        }

        public string TheResultExpression
        {
            get
            {
                return _filter.GetExpression(_memberAccessExpr, _valueExpr).ToString();
            }
        }
    }

    public class FilterTestEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}