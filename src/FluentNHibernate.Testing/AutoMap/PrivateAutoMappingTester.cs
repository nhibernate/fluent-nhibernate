using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using NUnit.Framework;
using System.Linq.Expressions;

namespace FluentNHibernate.Testing.Automapping
{
    [TestFixture]
    public class PrivateAutoMappingTester
    {
        private AutoPersistenceModel model;

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
            Model<ExampleClass>(p => p.Name.StartsWith("_"));

            Test<ExampleClass>(mapping =>
                mapping.Properties.ShouldContain(x => x.PropertyInfo == ReflectionHelper.GetProperty(ExampleClass.PrivateProperties.Property)));
        }

        [Test]
        public void DoNotMapPrivatePropertiesThatDoNotMatchConvention()
        {
            Model<ExampleClass>(p => p.Name.StartsWith("asdf"));

            Test<ExampleClass>(mapping =>
                mapping.Properties.ShouldBeEmpty());
        }

        [Test]
        public void CanMapPrivateCollection()
        {
            Model<ExampleParent>(p => p.Name.StartsWith("_"));

            Test<ExampleParent>(mapping =>
                mapping.Collections.ShouldContain(x => x.MemberInfo == ReflectionHelper.GetProperty(ExampleParent.PrivateProperties.Children)));
        }

        private void Model<T>(Func<PropertyInfo, bool> convention)
        {
            model = new PrivateAutoPersistenceModel()
                .Setup(conventions => conventions.FindMappablePrivateProperties = convention);

            model.AutoMap<T>();
        }

        private void Test<T>(Action<ClassMapping> mapping)
        {
            var map = model.FindMapping<T>();

            mapping(map.GetClassMapping());
        }
    }
}
