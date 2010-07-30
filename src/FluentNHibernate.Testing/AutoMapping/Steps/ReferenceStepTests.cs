using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Testing.Automapping;
using FluentNHibernate.Utils.Reflection;
using Iesi.Collections.Generic;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Steps
{
    [TestFixture]
    public class ReferenceStepTests
    {
        [Test]
        public void ShouldntMapSets()
        {
            ShouldntMap(x => x.Set);
        }

        [Test]
        public void ShouldntMapLists()
        {
            ShouldntMap(x => x.List);
        }

        [Test]
        public void ShouldntMapValueTypes()
        {
            ShouldntMap(x => x.Int);
            ShouldntMap(x => x.String);
            ShouldntMap(x => x.DateTime);
        }

        [Test]
        public void ShouldMapEntities()
        {
            ShouldMap(x => x.Entity);
        }

        private ReferenceStep mapper;

        [SetUp]
        public void CreateMapper()
        {
            mapper = new ReferenceStep(new DefaultAutomappingConfiguration());
        }

        protected void ShouldMap(Expression<Func<PropertyTarget, object>> property)
        {
            mapper.ShouldMap(ReflectionHelper.GetMember(property)).ShouldBeTrue();
        }

        protected void ShouldntMap(Expression<Func<PropertyTarget, object>> property)
        {
            mapper.ShouldMap(ReflectionHelper.GetMember(property)).ShouldBeFalse();
        }

        protected class PropertyTarget
        {
            public ISet<PropertyTarget> Set { get; set; }
            public IList<PropertyTarget> List { get; set; }
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public PropertyTarget Entity { get; set; }
        }
    }
}