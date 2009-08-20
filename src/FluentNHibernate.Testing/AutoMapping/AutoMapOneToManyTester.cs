using System.Linq.Expressions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class AutoMapOneToManyTester
    {
        private AutoMapOneToMany mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new AutoMapOneToMany(new AutoMappingExpressions());
        }

        [Test]
        public void ShouldMapSets()
        {
            ShouldMap(x => x.Set);
        }

        [Test]
        public void ShouldMapLists()
        {
            ShouldMap(x => x.List);
        }

        [Test]
        public void ShouldntMapValueTypes()
        {
            ShouldntMap(x => x.Int);
            ShouldntMap(x => x.String);
            ShouldntMap(x => x.DateTime);
        }

        [Test]
        public void ShouldntMapEntities()
        {
            ShouldntMap(x => x.Entity);
        }

        protected void ShouldMap(Expression<System.Func<PropertyTarget, object>> property)
        {
            mapper.MapsProperty(ReflectionHelper.GetProperty(property)).ShouldBeTrue();
        }

        protected void ShouldntMap(Expression<System.Func<PropertyTarget, object>> property)
        {
            mapper.MapsProperty(ReflectionHelper.GetProperty(property)).ShouldBeFalse();
        }

        protected class PropertyTarget
        {
            public Iesi.Collections.Generic.ISet<PropertyTarget> Set { get; set; }
            public System.Collections.Generic.IList<PropertyTarget> List { get; set; }
            public int Int { get; set; }
            public string String { get; set; }
            public System.DateTime DateTime { get; set; }
            public PropertyTarget Entity { get; set; }
        }
    }
}