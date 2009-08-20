using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions
{
    [TestFixture]
    public class HasManyConventionTests
    {
        [Test]
        public void ShouldBeAbleToSpecifyKeyColumnNameInConvention()
        {
            var model =
                AutoMap.Source(new StubTypeSource(typeof(Target)))
                    .Conventions.Add<HasManyConvention>();

            model.CompileMappings();

            model.BuildMappings()
                .First()
                .Classes.First()
                .Collections.First()
                .Key.Columns.First().Name.ShouldEqual("xxx");
            
        }

        private class HasManyConvention : IHasManyConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.Key.Column("xxx");
            }
        }
    }

    internal class Target
    {
        public IList<Child> Children { get; set; }
    }

    internal class Child
    { }
}
