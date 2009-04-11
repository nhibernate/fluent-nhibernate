using FluentNHibernate.MappingModel.Collections;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Collections
{
    [TestFixture]
    public class OneToManyMappingTester
    {
        [Test]
        public void ExceptionOnNotFound_should_default_to_true()
        {
            var oneToMany = new OneToManyMapping();
            oneToMany.Attributes.IsSpecified(x => x.ExceptionOnNotFound).ShouldBeFalse();
            oneToMany.ExceptionOnNotFound.ShouldBeTrue();
        }
    }
}