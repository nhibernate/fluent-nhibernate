using FluentNHibernate.Testing.DomainModel.Mapping;
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
    }
}