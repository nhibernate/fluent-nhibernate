using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing
{
    [TestFixture]
    public class ExpressionToSqlTests
    {
        [Test]
        public void ConvertPropertyToPropertyName()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name);

            sql.ShouldEqual("Name");
        }

        [Test]
        public void ConvertIntToValue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => 1);

            sql.ShouldEqual("1");
        }

        [Test]
        public void ConvertStringToValue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => "1");

            sql.ShouldEqual("'1'");
        }

        [Test]
        public void ConvertEqualsOfTwoProperties()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name == x.Name);

            sql.ShouldEqual("Name = Name");
        }

        [Test]
        public void ConvertEqualsPropertyAndInt()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position == 1);

            sql.ShouldEqual("Position = 1");
        }

        [Test]
        public void ConvertEqualsPropertyAndString()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name == "1");

            sql.ShouldEqual("Name = '1'");
        }

        [Test]
        public void ConvertGreater()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position > 1);

            sql.ShouldEqual("Position > 1");
        }

        [Test]
        public void ConvertLess()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position < 1);

            sql.ShouldEqual("Position < 1");
        }

        [Test]
        public void ConvertGreaterEquals()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position >= 1);

            sql.ShouldEqual("Position >= 1");
        }

        [Test]
        public void ConvertLessEquals()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position <= 1);

            sql.ShouldEqual("Position <= 1");
        }

        [Test]
        public void ConvertNot()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position != 1);

            sql.ShouldEqual("Position != 1");
        }

        [Test]
        public void ConvertLocalVariable()
        {
            var local = "someValue";
            var sql = ExpressionToSql.Convert<ChildObject>(x => local);

            sql.ShouldEqual("'someValue'");
        }

        private const string someValue = "someValue";

        [Test]
        public void ConvertConst()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => someValue);

            sql.ShouldEqual("'someValue'");
        }

        private class StaticExample
        {
            public static string Value = "someValue";
            public static string Method()
            {
                return "someValue";
            }
        }

        [Test]
        public void ConvertStaticMemberReference()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => StaticExample.Value);

            sql.ShouldEqual("'someValue'");
        }

        [Test]
        public void ConvertStaticMethodCall()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => StaticExample.Method());

            sql.ShouldEqual("'someValue'");
        }

        private enum Something
        {
            Else = 10
        }

        [Test]
        public void ConvertEnumMemberReferenceMethodCall()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Something.Else.ToString());

            sql.ShouldEqual("'Else'");
        }

        [Test]
        public void ConvertEnumMemberReference()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Something.Else);

            sql.ShouldEqual("10");
        }
    }
}