using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Engine;
using NHibernate.Id;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class IdGeneratorTests : BaseModelFixture
    {
        [Test]
        public void ShouldDefaultToAssignedForStrings()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.String))
                .ModelShouldMatch(x => ((IdMapping)x.Id).Generator.Class.ShouldEqual("assigned"));
        }
        
        [Test]
        public void ShouldDefaultToIdentityForInt()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.Int))
                .ModelShouldMatch(x => ((IdMapping)x.Id).Generator.Class.ShouldEqual("identity"));
        }

        [Test]
        public void ShouldDefaultToGuidCombForGuids()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.Guid))
                .ModelShouldMatch(x => ((IdMapping)x.Id).Generator.Class.ShouldEqual("guid.comb"));
        }

        [Test]
        public void ShouldBeSetAsDefaultWhenNotExplicitlySpecified()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.Guid))
                .ModelShouldMatch(x => ((IdMapping)x.Id).IsSpecified(p => p.Generator).ShouldBeFalse());
        }

        [Test]
        public void ShouldntBeSetAsDefaultWhenExplicitlySpecified()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.Int).GeneratedBy.Assigned())
                .ModelShouldMatch(x => ((IdMapping)x.Id).IsSpecified(p => p.Generator).ShouldBeTrue());
        }

        [Test]
        public void ShouldAllowOverridingOfDefaultInConventions()
        {
            var model = new PersistenceModel();

            var map = new ClassMap<IdentityExamples>();

            map.Id(x => x.Int);

            model.Conventions.Add(new IdConvention());
            model.Add(map);
            var @class = model.BuildMappings()
                .First()
                .Classes.First();

            ((IdMapping)@class.Id).Generator.Class.ShouldEqual("increment");
        }

        [Test]
        public void ShouldAllowCustomGenerators()
        {
            ClassMap<IdentityExamples>()
                .Mapping(m => m.Id(x => x.Int).GeneratedBy.Custom<CustomGenerator>())
                .ModelShouldMatch(x => ((IdMapping)x.Id).Generator.Class.ShouldEqual(typeof(CustomGenerator).AssemblyQualifiedName));
        }

        private class IdentityExamples
        {
            public string String { get; set; }
            public int Int { get; set; }
            public Guid Guid { get; set; }
        }

        private class IdConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.GeneratedBy.Increment();
            }
        }

        private class CustomGenerator : IIdentifierGenerator
        {
            public object Generate(ISessionImplementor session, object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}