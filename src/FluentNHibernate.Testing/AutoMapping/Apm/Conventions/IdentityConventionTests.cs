using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.Automapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.AutoMapping.Apm.Conventions
{
    [TestFixture]
    public class IdentityConventionTests
    {
        [Test]
        public void ShouldBeAbleToSpecifyKeyInConvention()
        {
            var model =
                AutoMap.Source(new StubTypeSource(typeof(IdTarget)))
                    .Conventions.Add<IdConvention>();

            model.CompileMappings();

            var classMapping = model.BuildMappings()
                .First()
                .Classes.First();

            ((IdMapping)classMapping.Id).Columns.First().Name.ShouldEqual("xxx");
        }

        private class IdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column("xxx");
            }
        }
    }

    internal class IdTarget
    {
        public int Id { get; set; }
    }
}