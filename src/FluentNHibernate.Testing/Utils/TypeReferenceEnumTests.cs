using System;
using FluentNHibernate.MappingModel;
using NUnit.Framework;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace FluentNHibernate.Testing.Utils
{
    [TestFixture]
    public class TypeReferenceEnumTests
    {
        [Test]
        public void IsEnumOnTypeReferenceToEnumStringTypeShouldBeTrue()
        {
            var enumTypeReference = new TypeReference(typeof(EnumStringType<TestEnum>));
            enumTypeReference.IsEnum.ShouldBeTrue();
        }
        
        [Test, Obsolete]
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