using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;
using System;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ClassMapSubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void HasManyShouldAddToCollectionsCollectionOnModel()
        {
            ClassMap<OneToManyTarget>()
                .Mapping(m => m.HasMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void HasManyToManyShouldAddToCollectionsCollectionOnModel()
        {
            ClassMap<OneToManyTarget>()
                .Mapping(m => m.HasManyToMany(x => x.BagOfChildren))
                .ModelShouldMatch(x => x.Collections.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesShouldAddToReferencesCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.References(x => x.Reference))
                .ModelShouldMatch(x => x.References.Count().ShouldEqual(1));
        }

        [Test]
        public void ReferencesWithExplicitTypeShouldUseSpecifiedType()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.References<PropertyReferenceTargetProxy>(x => x.Reference))
                .ModelShouldMatch(x => x.References.First().Class.GetUnderlyingSystemType().ShouldEqual(typeof(PropertyReferenceTargetProxy)));
        }

        [Test]
        public void ReferencesAnyShouldAddToAnyCollectionOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.ReferencesAny(x => x.Reference)
                    .IdentityType<int>()
                    .EntityIdentifierColumn("col1")
                    .EntityTypeColumn("col2"))
                .ModelShouldMatch(x => x.Anys.Count().ShouldEqual(1));
        }

        [Test]
        public void IdShouldSetIdPropertyOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Id(x => x.Id))
                .ModelShouldMatch(x => x.Id.ShouldBeOfType<IdMapping>());
        }

        [Test]
        public void IdWithoutPropertyShouldSetIdPropertyOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Id<int>("id"))
                .ModelShouldMatch(m =>
                {
                    var id = m.Id as IdMapping;

                    id.ShouldNotBeNull();
                    id.Type.ShouldEqual(new TypeReference(typeof(int)));
                    id.Columns.ShouldContain(x => x.Name == "id");
                });
        }

        [Test]
        public void CompositeIdShouldSetIdPropertyOnModel()
        {
            ClassMap<PropertyTarget>()
                .Mapping(m => m.CompositeId()
                    .KeyProperty(x => x.Id))
                .ModelShouldMatch(x => x.Id.ShouldBeOfType<CompositeIdMapping>());
        }

        [Test]
        public void TuplizerShouldSetTuplizerOnModel()
        {
            Type tuplizerType = typeof(NHibernate.Tuple.Entity.PocoEntityTuplizer);
            ClassMap<PropertyTarget>()
                .Mapping(m => m.Tuplizer(TuplizerMode.Poco, tuplizerType))
                .ModelShouldMatch(x =>
                {
                    x.Tuplizer.ShouldNotBeNull();
                    x.Tuplizer.Mode.ShouldEqual(TuplizerMode.Poco);
                    x.Tuplizer.Type.ShouldEqual(new TypeReference(tuplizerType));
                });
        }

    }
}