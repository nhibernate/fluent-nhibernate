using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;
using System.Linq.Expressions;

namespace FluentNHibernate.Testing.AutoMap
{
    [TestFixture]
    public class PrivateAutoMappingTester
    {
        private class ExampleClass
        {
            private string _privateProperty { get; set; }

            public class PrivateProperties
            {
                public static Expression<Func<ExampleClass, object>> Property = x => x._privateProperty;
            }
        }

        private class ExampleParent
        {
            private IList<ExampleClass> _privateChildren { get; set; }

            public class PrivateProperties
            {
                public static Expression<Func<ExampleParent, object>> Children = x => x._privateChildren;
            }
        }

        [Test]
        public void WillMapPrivatePropertyMatchingTheConvention()
        {
            var conventions = new Conventions();
            conventions.FindMappablePrivateProperties = p => p.Name.StartsWith("_");
            var autoMapper = new PrivateAutoMapper(conventions);
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());
            
            Assert.Contains(ReflectionHelper.GetProperty(ExampleClass.PrivateProperties.Property), (ICollection) map.PropertiesMapped);
            
        }

        [Test]
        public void DoNotMapPrivatePropertiesThatDoNotMatchConvention()
        {
            var conventions = new Conventions();
            conventions.FindMappablePrivateProperties = p => p.Name.StartsWith("asdf");
            var autoMapper = new PrivateAutoMapper(conventions);
            var map = autoMapper.Map<ExampleClass>(new List<AutoMapType>());
            map.PropertiesMapped.ShouldBeEmpty();
        }

        [Test]
        public void AutoPersistenceModelCanUsePrivateAutoMapper()
        {
            var conventions = new Conventions();
            conventions.FindMappablePrivateProperties = p => p.Name.StartsWith("_");
            
            var model = new AutoPersistenceModel(new PrivateAutoMapper(conventions));
            model.AutoMap<ExampleClass>();

            IClassMap map = model.FindMapping<ExampleClass>();
            var autoMap = (AutoMap<ExampleClass>) map;
            Assert.Contains(ReflectionHelper.GetProperty(ExampleClass.PrivateProperties.Property), (ICollection)autoMap.PropertiesMapped);
        }

        [Test]
        public void CanMapPrivateCollection()
        {
            var conventions = new Conventions();
            conventions.FindMappablePrivateProperties = p => p.Name.StartsWith("_");
            var autoMapper = new PrivateAutoMapper(conventions);
            var map = autoMapper.Map<ExampleParent>(new List<AutoMapType>());

            Assert.Contains(ReflectionHelper.GetProperty(ExampleParent.PrivateProperties.Children), (ICollection)map.PropertiesMapped);
        }

    }

    
}
