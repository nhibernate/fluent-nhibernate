using FluentNHibernate.Mapping;
using FluentNHibernate.Testing.DomainModel.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class StoredProcedurePartGenerationTests : BaseModelFixture
    {
        [Test]
        public void CheckTypeShouldBeAbleToSetToNone()
        {
            StoredProcedure()
                .Mapping(m => m.Check.None())
                .ModelShouldMatch(x => x.Check.ShouldEqual("none"));
        }    
        
        [Test]
        public void CheckTypeShouldBeAbleToSetToRowCount()
        {
            StoredProcedure()
                .Mapping(m => m.Check.RowCount())
                .ModelShouldMatch(x => x.Check.ShouldEqual("rowcount"));
        }
        
    }
}
