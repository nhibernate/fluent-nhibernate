using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
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
    }
    [TestFixture]
    public class IdMutablePropertyModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AccessShouldSetModelAccessPropertyToValue()
        {
            Id()
                .Mapping(m => m.Access.Field())
                .ModelShouldMatch(x => x.Access.ShouldEqual("field"));
        }

        [Test]
        public void ColumnNameShouldAddToModelColumnsCollection()
        {
            Id()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.Count().ShouldEqual(1));
        }

        [Test]
        public void ColumnNameShouldSetModelColumnName()
        {
            Id()
                .Mapping(m => m.Column("col"))
                .ModelShouldMatch(x => x.Columns.First().Name.ShouldEqual("col"));
        }

        [Test]
        public void ShouldSetModelNamePropertyToPropertyName()
        {
            Id()
                .Mapping(m => {})
                .ModelShouldMatch(x => x.Name.ShouldEqual("IntId"));
        }

        [Test]
        public void ShouldSetModelTypePropertyToPropertyType()
        {
            Id()
                .Mapping(m => { })
                .ModelShouldMatch(x => x.Type.ShouldEqual(new TypeReference(typeof(int))));
        }

        [Test]
        public void UnsavedValueShouldSetModelTypePropertyToPropertyType()
        {
            Id()
                .Mapping(m => m.UnsavedValue(10))
                .ModelShouldMatch(x => x.UnsavedValue.ShouldEqual("10"));
        }


        [Test]
        public void GeneratedByShouldSetModelGeneratorProperty()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned())
                .ModelShouldMatch(x => x.Generator.ShouldNotBeNull());
        }

        [Test]
        public void GeneratedByShouldSetModelGeneratorPropertyToValue()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned())
                .ModelShouldMatch(x => x.Generator.Class.ShouldEqual("assigned"));
        }

        [Test]
        public void GeneratedByWithParamsShouldSetModelGeneratorParams()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned(p =>
                    p.AddParam("name", "value")
                     .AddParam("another", "another-value")))
                .ModelShouldMatch(x => x.Generator.Params.Count().ShouldEqual(2));
        }

        [Test]
        public void GeneratedByWithParamsShouldSetModelGeneratorParamsValues()
        {
            Id()
                .Mapping(m => m.GeneratedBy.Assigned(p =>
                    p.AddParam("name", "value")
                     .AddParam("another", "another-value")))
                .ModelShouldMatch(x =>
                {
                    var first = x.Generator.Params.First();

                    first.Key.ShouldEqual("name");
                    first.Value.ShouldEqual("value");

                    var second = x.Generator.Params.ElementAt(1);

                    second.Key.ShouldEqual("another");
                    second.Value.ShouldEqual("another-value");
                });
        }

        public void LengthShouldSetModelLengthPropertyToValue()
        {
            Id()
                .Mapping(m => m.Length(8))
                .ModelShouldMatch(x => x.Length.ShouldEqual(8));
        }
    }
}