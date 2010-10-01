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
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasMany(x => x.EntityMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.ShouldBeOfType<IndexMapping>();
        }

        [Test]
        public void AsTernaryAssocationShouldSetIndexManyToManyClass()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasMany(x => x.EntityMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.Type.ShouldEqual(new TypeReference(typeof(SomeEntity)));
        }

        [Test]
        public void AsTernaryAssocationShouldSetDefaultColumnName()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasMany(x => x.EntityMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.As<IndexMapping>()
                .Columns.Single().Name.ShouldEqual(typeof(SomeEntity).Name + "_id");
        }

        [Test]
        public void AsTernaryAssociationShouldSetProvidedColumnName()
        {
            const string indexName = "index-name";
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.EntityMapOfChildren)
                    .DictionaryKey(indexName));

            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.As<IndexMapping>()
                .Columns.Single().Name.ShouldEqual(indexName);
        }

        [Test]
        public void EntityMapIsAMapWithAManyToManyIndex()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map => class_map.HasManyToMany(x => x.EntityMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.ShouldBeOfType<IndexMapping>();
        }

        [Test]
        public void AsEntityMapShouldSetProvidedColumnName()
        {
            const string indexName = "index-name";
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.EntityMapOfChildren)
                    .DictionaryKey(indexName));

            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.As<IndexMapping>()
                .Columns.Single().Name.ShouldEqual(indexName);
        }
    }
}