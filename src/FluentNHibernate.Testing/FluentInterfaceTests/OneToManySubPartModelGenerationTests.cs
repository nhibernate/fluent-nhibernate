using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class OneToManySubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void ComponentShouldSetCompositeElement()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.BagOfChildren)
                    .Component(c => c.Map(x => x.Name)));

            mapping.Collections.Single()
                .CompositeElement.ShouldNotBeNull();
        }

        [Test]
        public void ListShouldSetIndex()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.ListOfChildren)
                    .AsList(ls =>
                    {
                        ls.Column("index-column");
                        ls.Type<int>();
                    }));

            var collection = mapping.Collections.Single() as ListMapping;

            collection.ShouldNotBeNull();
            collection.Index.ShouldNotBeNull();
            collection.Index.Type.ShouldEqual(new TypeReference(typeof(int)));
            collection.Index.As<IndexMapping>().Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void MapShouldSetIndex()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.MapOfValues));
            var collection = mapping.Collections.Single() as MapMapping;

            collection.ShouldNotBeNull();
            collection.Index.ShouldBeOfType<IndexMapping>();
            collection.Index.Type.ShouldEqual(new TypeReference(typeof(string)));
            collection.Index.As<IndexMapping>().Columns.Count().ShouldEqual(1);
        }

        [Test]
        public void ShouldSetElement()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                    class_map.HasMany(x => x.ListOfChildren)
                        .Element("element"));
            var collection = mapping.Collections.Single();

            collection.Element.ShouldNotBeNull();
            collection.Element.Columns.Count().ShouldEqual(1);
            collection.Element.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));
        }

        [Test]
        public void ElementMappingShouldntHaveOneToMany()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.ListOfChildren)
                    .Element("element"));

            mapping.Collections.Single()
                .Relationship.ShouldBeNull();
        }

        [Test]
        public void ShouldPerformKeyColumnMapping()
        {
            var mapping = MappingFor<OneToManyTarget>(class_map =>
                class_map.HasMany(x => x.ListOfChildren)
                    .Key(ke => ke.Columns.Add("col1", c => c.Length(50).Not.Nullable())));
            var column = mapping.Collections.Single().Key.Columns.Single();

            column.Name.ShouldEqual("col1");
            column.Length.ShouldEqual(50);
            column.NotNull.ShouldBeTrue();
        }
    }
}