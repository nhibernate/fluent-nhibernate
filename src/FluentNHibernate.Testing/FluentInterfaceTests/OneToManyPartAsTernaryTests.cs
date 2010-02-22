using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManyPartAsTernaryTests : BaseModelFixture
    {
        [Test]
        public void AsTernaryAssocationShouldCreateIndexManyToMany()
        {
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsMap("irrelevant-value").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    IIndexMapping index = ((MapMapping)x).Index;
                    index.ShouldBeOfType(typeof(IndexManyToManyMapping));
                });
        }

        [Test]
        public void AsTernaryAssocationShouldSetIndexManyToManyClass()
        {
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsMap("irrelevant-value").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;
                    index.Class.ShouldEqual(new TypeReference(typeof(SomeEntity)));
                });
        }

        [Test]
        public void AsTernaryAssocationShouldSetDefaultColumnName()
        {
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsMap("irrelevant-value").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;
                    index.Columns.Single().Name.ShouldEqual(typeof(SomeEntity).Name + "_id");
                });
        }

        [Test]
        public void AsTernaryAssociationShouldSetProvidedColumnName()
        {
            const string indexName = "index-name";
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsMap("irrelevant-value").AsTernaryAssociation(indexName))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;
                    index.Columns.Single().Name.ShouldEqual(indexName);
                });
        }

        [Test]
        public void EntityMapIsAMapWithAManyToManyIndex()
        {
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsEntityMap())
                .ModelShouldMatch(x =>
                {
                    x.ShouldBeOfType(typeof(MapMapping));
                    IIndexMapping index = ((MapMapping)x).Index;
                    index.ShouldBeOfType(typeof(IndexManyToManyMapping));
                });
        }

        [Test]
        public void AsEntityMapShouldSetProvidedColumnName()
        {
            const string indexName = "index-name";

            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsEntityMap(indexName))
                .ModelShouldMatch(x =>
                {
                    x.ShouldBeOfType(typeof(MapMapping));
                    IIndexMapping index = ((MapMapping)x).Index;
                    index.ShouldBeOfType(typeof(IndexManyToManyMapping));
                    index.Columns.Single().Name.ShouldEqual(indexName);
                });
        }
    }
}