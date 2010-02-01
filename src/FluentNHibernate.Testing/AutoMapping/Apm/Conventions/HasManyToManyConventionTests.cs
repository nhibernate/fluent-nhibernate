using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions
{
    [TestFixture]
    public class HasManyToManyConventionTests
    {
        [Test]
        public void ShouldSetTableName()
        {
            var model =
                AutoMap.Source(new StubTypeSource(new[] { typeof(One), typeof(Two) }));

            var classMapping = model.BuildMappings()
                .SelectMany(x => x.Classes)
                .First(x => x.Type == typeof(One));
             
            classMapping.Collections.First()
                .TableName.ShouldEqual("M2MRelation1ToM2MRelation1");
        }
    }

    public class One
    {
        public int Id { get; set; }
        public IList<Two> M2MRelation1 { get; set; }
        public IList<Two> M2MRelation2 { get; set; }
    }

    public class Two
    {
        public int Id { get; set; }
        public IList<One> M2MRelation1 { get; set; }
        public IList<One> M2MRelation2 { get; set; }
    }
}