using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.MappingModel;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Testing
{
    public class XmlWriterTestHelper<TMappingType> where TMappingType : new()
    {
        private readonly IList<XmlTest> _tests;

        public XmlWriterTestHelper()
        {
            _tests = new List<XmlTest>();
        }

        public XmlTest Check(Expression<Func<TMappingType, object>> sourceProperty, object value)
        {
            var test = new XmlTest(sourceProperty, value);
            _tests.Add(test);
            return test;
        }


        public void VerifyAll(IXmlWriter<TMappingType> writer)
        {
            foreach (var test in _tests)
            {
                var mapping = new TMappingType();
                test.ApplyToSource(mapping);

                var serializer = new MappingXmlSerializer();
                var xmlDoc = writer.Write(mapping);

                test.Check(xmlDoc);
            }
        }

        public class XmlTest
        {
            private readonly IDictionary<string, object> _checks;
            private readonly Accessor _sourceProperty;
            private readonly object _sourceValue;

            public XmlTest(Expression<Func<TMappingType, object>> sourceProperty, object value)
            {
                _checks = new Dictionary<string, object>();
                _sourceProperty = ReflectionHelper.GetAccessor(sourceProperty);
                _sourceValue = value;
            }

            public XmlTest MapsToAttribute(string attributeName, object value)
            {
                _checks[attributeName] = value;
                return this;
            }

            public XmlTest MapsToAttribute(string attributeName)
            {
                _checks[attributeName] = _sourceValue;
                return this;
            }

            internal void ApplyToSource(TMappingType mapping)
            {
                _sourceProperty.SetValue(mapping, _sourceValue);
            }

            internal void Check(XmlDocument document)
            {
                var rootElement = document.DocumentElement;
                foreach (var check in _checks)
                {
                    string attributeValue = rootElement.GetAttribute(check.Key);
                    bool areEqual = string.Equals(attributeValue, check.Value.ToString(),
                                                  StringComparison.InvariantCultureIgnoreCase);

                    if (!areEqual)
                        document.OutputXmlToConsole();

                    Assert.That(areEqual,
                                    "Property '{0}' was set to '{1}' and was expected to be written to attribute '{2}' with value '{3}'. The value was instead '{4}'",
                                    _sourceProperty.InnerProperty.ReflectedType.Name + "." + _sourceProperty.Name,
                                    _sourceValue, check.Key, check.Value, attributeValue
                        );
                    //string.Equals()
                    //rootElement.AttributeShouldEqual(check.Key, check.Value.ToString());
                    //rootElement.Attributes[check.Key].Value.ShouldBeEqualIgnoringCase(check.Value.ToString());

                }
            }
        }
    }
}
