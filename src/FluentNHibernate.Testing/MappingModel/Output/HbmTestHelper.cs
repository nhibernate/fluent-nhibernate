using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Reflection;
using FluentNHibernate.Xml;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{

    public class HbmTestHelper<MAPPING_TYPE> where MAPPING_TYPE : new()
    {
        private readonly IList<HbmTest> _tests;

        public HbmTestHelper()
        {
            _tests = new List<HbmTest>();
        }

        public HbmTest Check(Expression<Func<MAPPING_TYPE, object>> sourceProperty, object value)
        {
            var test = new HbmTest(sourceProperty, value);
            _tests.Add(test);
            return test;
        }


        public void VerifyAll(IHbmWriter<MAPPING_TYPE> writer)
        {
            foreach (var test in _tests)
            {
                var mapping = new MAPPING_TYPE();
                test.ApplyToSource(mapping);
                var serializer = new MappingXmlSerializer();
                var xmlDoc = serializer.SerializeHbmFragment(writer.Write(mapping));                
                test.Check(xmlDoc);
            }
        }

        public class HbmTest
        {
            private readonly IDictionary<string, object> _checks;
            private readonly Accessor _sourceProperty;
            private readonly object _sourceValue;

            public HbmTest(Expression<Func<MAPPING_TYPE, object>> sourceProperty, object value)
            {
                _checks = new Dictionary<string, object>();
                _sourceProperty = ReflectionHelper.GetAccessor(sourceProperty);
                _sourceValue = value;
            }

            public HbmTest MapsToAttribute(string attributeName, object value)
            {
                _checks[attributeName] = value;
                return this;
            }

            public HbmTest MapsToAttribute(string attributeName)
            {
                _checks[attributeName] = _sourceValue;
                return this;
            }

            internal void ApplyToSource(MAPPING_TYPE mapping)
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

                    if(!areEqual)
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


    public class HbmTestHelper<MAPPING_TYPE, HBM_TYPE> where MAPPING_TYPE : new() where HBM_TYPE : new()
    {
        private readonly IList<HbmTest> _tests;

        public HbmTestHelper()
        {
            _tests = new List<HbmTest>();
        }

        public HbmTest Check(Expression<Func<MAPPING_TYPE, object>> sourceProperty, object value)
        {
            var test = new HbmTest(sourceProperty, value);
            _tests.Add(test);
            return test;
        }


        public void VerifyAll(IHbmWriter<MAPPING_TYPE> writer)
        {
            foreach (var test in _tests)
            {
                var mapping = new MAPPING_TYPE();
                test.ApplyToSource(mapping);
                var hbm = (HBM_TYPE)writer.Write(mapping);
                test.Check(hbm);
            }
        }

        public class HbmTest
        {
            private readonly IDictionary<Expression<Func<HBM_TYPE, object>>, object> _checks;
            private readonly Accessor _sourceProperty;
            private readonly object _sourceValue;

            public HbmTest(Expression<Func<MAPPING_TYPE, object>> sourceProperty, object value)
            {
                _checks = new Dictionary<Expression<Func<HBM_TYPE, object>>, object>();
                _sourceProperty = ReflectionHelper.GetAccessor(sourceProperty);
                _sourceValue = value;
            }

            public HbmTest MapsTo(Expression<Func<HBM_TYPE, object>> destinationExp, object value)
            {
                _checks[destinationExp] = value;
                return this;
            }

            public HbmTest MapsTo(Expression<Func<HBM_TYPE, object>> destinationExp)
            {
                _checks[destinationExp] = _sourceValue;
                return this;
            }

            public void ApplyToSource(MAPPING_TYPE mapping)
            {
                _sourceProperty.SetValue(mapping, _sourceValue);
            }

            public void Check(HBM_TYPE hbm)
            {
                foreach (var check in _checks)
                {
                    object val = check.Key.Compile()(hbm);
                    Assert.AreEqual(check.Value, val,
                                    "Property {0} was set to {1} and was expected to be written to {2} with value {3}. The value was instead {4}",
                                    _sourceProperty.InnerProperty.ReflectedType.Name + "." + _sourceProperty.Name,
                                    FullyQualifiedValue(_sourceValue), check.Key.ToString(), FullyQualifiedValue(check.Value), FullyQualifiedValue(val)
                        );
                }
            }

            private static string FullyQualifiedValue(object val)
            {
                return string.Format("{0} ({1})", val, val.GetType());
            }

        }
    }
}