using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions
{
    [TestFixture]
    public class ReferencesConventionTests
    {
        [Test]
        public void ShouldBeAbleToSpecifyKeyInConvention()
        {
            var model =
                AutoMap.Source(new StubTypeSource(typeof(Target)))
                    .Conventions.Add<FKConvention>();

            model.CompileMappings();

            model.BuildMappings()
                .First()
                .Classes.First()
                .References.First()
                .Columns.First().Name.ShouldEqual("xxx");
        }

        private class FKConvention : ForeignKeyConvention
        {
            protected override string GetKeyName(PropertyInfo property, Type type)
            {
                return "xxx";
            }
        }
    }
}