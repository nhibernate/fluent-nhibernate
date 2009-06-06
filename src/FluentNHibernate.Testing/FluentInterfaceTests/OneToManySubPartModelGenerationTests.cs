using System.Linq;
using FluentNHibernate.MappingModel.Collections;
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
                    x.WithColumn("index-column");
                    x.WithType<int>();
                }))
                .ModelShouldMatch(x =>
                {
                    var list = (ListMapping)x;

                    list.Index.ShouldNotBeNull();
                    list.Index.Columns.Count().ShouldEqual(1);
                    list.Index.Type.ShouldEqual(typeof(int).AssemblyQualifiedName);
                });
        }

        [Test]
        public void MapShouldSetIndex()
        {
            OneToMany(x => x.ListOfChildren)
                .Mapping(m => m.AsMap<int>("index-column"))
                .ModelShouldMatch(x =>
                {
                    var list = (MapMapping)x;

                    list.Index.ShouldNotBeNull();
                    list.Index.Columns.Count().ShouldEqual(1);
                    list.Index.Type.ShouldEqual(typeof(int).AssemblyQualifiedName);
                });
        }
    }
}