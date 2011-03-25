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
                .ModelShouldMatch(x => x.Index.ShouldBeOfType(typeof(IndexManyToManyMapping)));
        }

        [Test]
        public void AsTernaryAssocationShouldSetIndexManyToManyClass()
        {
            OneToMany(x => x.EntityMapOfChildren)
                .Mapping(m => m.AsMap("irrelevant-value").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)x.Index;
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
                    var index = (IndexManyToManyMapping)x.Index;
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
                    var index = (IndexManyToManyMapping)x.Index;
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
                    x.Collection.ShouldEqual(Collection.Map);
                    x.Index.ShouldBeOfType(typeof(IndexManyToManyMapping));
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
                    x.Collection.ShouldEqual(Collection.Map);
                    x.Index.ShouldBeOfType(typeof(IndexManyToManyMapping));
                    x.Index.Columns.Single().Name.ShouldEqual(indexName);
                });
        }
    }
}