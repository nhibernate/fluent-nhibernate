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
            ManyToMany(x => x.GenericTernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)x.Index;
                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)x.Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void GenericAsTernaryAssociationShouldSetProvidedNames()
        {
            var indexName = "index-name";
            var valueName = "value-name";
            ManyToMany(x => x.GenericTernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation(indexName, valueName))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)x.Index;

                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(indexName);
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)x.Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(valueName);
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void NonGenericAsTernaryAssocationShouldSetDefaultNames()
        {
            ManyToMany(x => x.NonGenericTernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation(typeof(ChildObject), typeof(ChildObject)))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)x.Index;
                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)x.Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void NonGenericAsTernaryAssociationShouldSetProvidedNames()
        {
            var indexName = "index-name";
            var valueName = "value-name";
            ManyToMany(x => x.NonGenericTernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation(typeof(ChildObject), indexName, typeof(ChildObject), valueName))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)x.Index;

                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(indexName);
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)x.Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(valueName);
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void EntityMapIsAMapWithAManyToManyIndex()
        {
            ManyToMany(x => x.GenericTernaryMapOfChildren)
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
            const string valueName = "value-name";
            ManyToMany(x => x.GenericTernaryMapOfChildren)
                .Mapping(m => m.AsEntityMap(indexName, valueName))                
                .ModelShouldMatch(x =>
                {
                    x.Collection.ShouldEqual(Collection.Map);
                    x.Index.ShouldBeOfType(typeof(IndexManyToManyMapping));
                    x.Index.Columns.Single().Name.ShouldEqual(indexName);

                    var relationship = (ManyToManyMapping)x.Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(valueName);
                });                
        }
    }
}