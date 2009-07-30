using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManySubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AsTernaryAssocationShouldSetDefaultNames()
        {
            ManyToMany(x => x.TernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation())
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;
                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)((MapMapping)x).Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(typeof(ChildObject).Name + "_id");
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void AsTernaryAssociationShouldSetProvidedNames()
        {
            var indexName = "index-name";
            var valueName = "value-name";
            ManyToMany(x => x.TernaryMapOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation(indexName, valueName))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;

                    index.Columns.Count().ShouldEqual(1);
                    index.Columns.First().Name.ShouldEqual(indexName);
                    index.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));

                    var relationship = (ManyToManyMapping)((MapMapping)x).Relationship;
                    relationship.Columns.Count().ShouldEqual(1);
                    relationship.Columns.First().Name.ShouldEqual(valueName);
                    relationship.Class.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }
    }
}