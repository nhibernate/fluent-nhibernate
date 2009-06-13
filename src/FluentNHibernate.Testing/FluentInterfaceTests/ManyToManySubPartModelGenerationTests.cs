using System.Linq;
using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class ManyToManySubPartModelGenerationTests : BaseModelFixture
    {
        [Test]
        public void AsTernaryAssocationShouldSetSomething()
        {
            ManyToMany(x => x.BagOfChildren)
                .Mapping(m => m.AsMap("index").AsTernaryAssociation<int>("index-column"))
                .ModelShouldMatch(x =>
                {
                    var index = (IndexManyToManyMapping)((MapMapping)x).Index;

                    index.Columns.Count().ShouldEqual(1);
                    index.Class.ShouldEqual(typeof(int).AssemblyQualifiedName);
                });

        }
    }
}