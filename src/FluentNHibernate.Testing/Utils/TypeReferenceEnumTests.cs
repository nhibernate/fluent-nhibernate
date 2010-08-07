using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Testing.Utils
{
    [TestFixture]
    public class TypeReferenceEnumTests
    {
        [Test]
        public void IsEnumOnTypeReferenceToGenericEnumMapperShouldBeTrue()
        {
            var enumTypeReference = new TypeReference(typeof(GenericEnumMapper<TestEnum>));
            enumTypeReference.IsEnum.ShouldBeTrue();
        }

        private enum TestEnum
        {
            Value1            
        }

        
    }
}