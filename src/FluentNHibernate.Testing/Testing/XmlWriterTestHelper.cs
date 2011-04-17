using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.MappingModel;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing
{
    public class XmlWriterTestHelper<TMappingType>
        where TMappingType : IMapping
    {
        readonly IList<XmlTest> tests;
        Func<TMappingType> constructor;

        public XmlWriterTestHelper()
        {
            tests = new List<XmlTest>();
        }

        public XmlTest Check(Expression<Func<TMappingType, object>> sourceProperty, object value)
        {
            var test = new XmlTest(sourceProperty, value);
            tests.Add(test);
            return test;
        }

        public void CreateInstance(Func<TMappingType> construct)
        {
            constructor = construct;
        }

        public void VerifyAll(IXmlWriter<TMappingType> writer)
        {
            foreach (var test in tests)
            {
                TMappingType mapping;

                if (constructor == null)
                    mapping = (TMappingType)typeof(TMappingType).InstantiateUsingParameterlessConstructor();
                else
                    mapping = constructor();

                test.ApplyToSource(mapping);

                var serializer = new MappingXmlSerializer();
                var xmlDoc = writer.Write(mapping);

                test.Check(xmlDoc);
            }
        }

        public class XmlTest
        {
            readonly IDictionary<string, object> checks;
            readonly Accessor sourceProperty;
            readonly object sourceValue;
            readonly Member member;

            public XmlTest(Expression<Func<TMappingType, object>> sourceProperty, object value)
            {
                checks = new Dictionary<string, object>();
                member = sourceProperty.ToMember();
                this.sourceProperty = ReflectionHelper.GetAccessor(sourceProperty);
                sourceValue = value;
            }

            public XmlTest MapsToAttribute(string attributeName, object value)
            {
                checks[attributeName] = value;
                return this;
            }

            public XmlTest MapsToAttribute(string attributeName)
            {
                checks[attributeName] = sourceValue;
                return this;
            }

            internal void ApplyToSource(TMappingType mapping)
            {
                mapping.Set(member.Name, Layer.Defaults, sourceValue);
            }

            internal void Check(XmlDocument document)
            {
                var rootElement = document.DocumentElement;
                foreach (var check in checks)
                {
                    string attributeValue = rootElement.GetAttribute(check.Key);
                    bool areEqual = string.Equals(attributeValue, check.Value.ToString(),
                                                  StringComparison.InvariantCultureIgnoreCase);

                    if (!areEqual)
                        document.OutputXmlToConsole();

                    Assert.That(areEqual,
                                    "Property '{0}' was set to '{1}' and was expected to be written to attribute '{2}' with value '{3}'. The value was instead '{4}'",
                                    sourceProperty.InnerMember.MemberInfo.ReflectedType.Name + "." + sourceProperty.Name,
                                    sourceValue, check.Key, check.Value, attributeValue
                        );
                    //string.Equals()
                    //rootElement.AttributeShouldEqual(check.Key, check.Value.ToString());
                    //rootElement.Attributes[check.Key].Value.ShouldBeEqualIgnoringCase(check.Value.ToString());

                }
            }
        }
    }
}
