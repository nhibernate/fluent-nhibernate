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
            OneToMany(x => x.BagOfChildren)
                .Mapping(m => m.Component(c => c.Map(x => x.Name)))
                .ModelShouldMatch(x => x.CompositeElement.ShouldNotBeNull());
        }

        [Test]
        public void ListShouldSetIndex()
        {
            OneToMany(x => x.ListOfChildren)
                .Mapping(m => m.AsList(x =>
                {
                    x.Column("index-column");
                    x.Type<int>();
                }))
                .ModelShouldMatch(x =>
                {
                    var list = (ListMapping)x;

                    list.Index.ShouldNotBeNull();
                    list.Index.Columns.Count().ShouldEqual(1);
                    ((IndexMapping)list.Index).Type.ShouldEqual(new TypeReference(typeof(int)));
                });
        }

        [Test]
        public void MapShouldSetIndex()
        {
            OneToMany(x => x.ListOfChildren)
                .Mapping(m => m.AsMap<int>("index-column"))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexMapping)((MapMapping)x).Index;

                    index.ShouldNotBeNull();
                    index.Columns.Count().ShouldEqual(1);
                    index.Type.ShouldEqual(new TypeReference(typeof(int)));
                });
        }

        [Test]
        public void ShouldSetElement()
        {
            OneToMany(x => x.ListOfChildren)
                .Mapping(m => m.Element("element"))
                .ModelShouldMatch(x =>
                {
                    x.Element.ShouldNotBeNull();
                    x.Element.Columns.Count().ShouldEqual(1);
                    x.Element.Type.ShouldEqual(new TypeReference(typeof(ChildObject)));
                });
        }

        [Test]
        public void ElementMappingShouldntHaveOneToMany()
        {
            OneToMany(x => x.ListOfChildren)
                .Mapping(m => m.Element("element"))
                .ModelShouldMatch(x => x.Relationship.ShouldBeNull());
        }
    }
}