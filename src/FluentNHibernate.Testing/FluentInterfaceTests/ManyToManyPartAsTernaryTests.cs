using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManyPartAsTernaryTests : BaseModelFixture
    {
        [Test]
        public void GenericAsTernaryAssocationShouldSetDefaultNames()
        {
            var mapping = MappingFor<ManyToManyTarget>(class_map => class_map.HasManyToMany(x => x.GenericTernaryMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            var index = (IndexMapping)collection.Index;
            index.Columns.Count().ShouldEqual(1);
            index.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
            index.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));

            var relationship = (ManyToManyMapping)collection.Relationship;
            relationship.Columns.Count().ShouldEqual(1);
            relationship.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
            relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
        }

        [Test]
        public void GenericAsTernaryAssociationShouldSetProvidedNames()
        {
            var indexName = "index-name";
            var valueName = "value-name";
            var mapping = MappingFor<ManyToManyTarget>(class_map =>
                class_map.HasManyToMany(x => x.GenericTernaryMapOfChildren)
                    .DictionaryKey(indexName)
                    .ManyToMany(valueName));
            var collection = mapping.Collections.Single() as MapMapping;

            var index = (IndexMapping)collection.Index;
            index.Columns.Count().ShouldEqual(1);
            index.Columns.First().Name.ShouldEqual(indexName);
            index.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));

            var relationship = (ManyToManyMapping)collection.Relationship;
            relationship.Columns.Count().ShouldEqual(1);
            relationship.Columns.First().Name.ShouldEqual(valueName);
            relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
        }

        [Test]
        public void NonGenericAsTernaryAssocationShouldSetDefaultNames()
        {
            var mapping = MappingFor<ManyToManyTarget>(class_map =>
                class_map.HasManyToMany<ChildObject, ChildObject>(x => x.NonGenericTernaryMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;
           
            var index = (IndexMapping)collection.Index;
            index.Columns.Count().ShouldEqual(1);
            index.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
            index.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));

            var relationship = (ManyToManyMapping)collection.Relationship;
            relationship.Columns.Count().ShouldEqual(1);
            relationship.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
            relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
        }

        [Test]
        public void NonGenericAsTernaryAssociationShouldSetProvidedNames()
        {
            var indexName = "index-name";
            var valueName = "value-name";
            var mapping = MappingFor<ManyToManyTarget>(class_map =>
                class_map.HasManyToMany<ChildObject, ChildObject>(x => x.NonGenericTernaryMapOfChildren)
                    .DictionaryKey(indexName)
                    .ManyToMany(valueName));

            var collection = mapping.Collections.Single() as MapMapping;
            var index = (IndexMapping)collection.Index;
            index.Columns.Count().ShouldEqual(1);
            index.Columns.First().Name.ShouldEqual(indexName);
            index.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));

            var relationship = (ManyToManyMapping)collection.Relationship;
            relationship.Columns.Count().ShouldEqual(1);
            relationship.Columns.First().Name.ShouldEqual(valueName);
            relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
        }

        [Test]
        public void EntityMapIsAMapWithAManyToManyIndex()
        {
            var mapping = MappingFor<ManyToManyTarget>(class_map => class_map.HasManyToMany(x => x.GenericTernaryMapOfChildren));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.ShouldBeOfType(typeof(IndexMapping));
        }

        [Test]
        public void AsEntityMapShouldSetProvidedColumnName()
        {
            const string indexName = "index-name";
            const string valueName = "value-name";
            var mapping = MappingFor<ManyToManyTarget>(class_map =>
                class_map.HasManyToMany(x => x.GenericTernaryMapOfChildren)
                    .DictionaryKey(indexName)
                    .ManyToMany(valueName));
            
            var collection = mapping.Collections.Single() as MapMapping;
            
            collection.ShouldNotBeNull();
            collection.Index.ShouldBeOfType(typeof(IndexMapping));
            collection.Index.As<IndexMapping>().Columns.Single().Name.ShouldEqual(indexName);

            var relationship = (ManyToManyMapping)collection.Relationship;
            relationship.Columns.Count().ShouldEqual(1);
            relationship.Columns.First().Name.ShouldEqual(valueName);
        }
    }
}