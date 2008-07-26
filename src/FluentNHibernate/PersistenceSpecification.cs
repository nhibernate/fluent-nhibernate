using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ShadeTree.Core;
using StructureMap;

namespace ShadeTree.DomainModel
{
    public class PersistenceSpecification<T> where T : Entity, new()
    {
        private readonly List<PropertyValue> _allProperties = new List<PropertyValue>();
        private readonly IRepository _repository;

        public PersistenceSpecification()
        {
            _repository = createRepository();
        }

        private IRepository createRepository()
        {
            ISessionSource source = ObjectFactory.GetInstance<ISessionSource>();
            return new Repository(source.CreateSession());
        }

        public PersistenceSpecification<T> CheckProperty(Expression<System.Func<T, object>> expression, object propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new PropertyValue(property, propertyValue));

            return this;
        }

        public PersistenceSpecification<T> CheckReference(Expression<System.Func<T, object>> expression, object propertyValue)
        {
            _repository.Save(propertyValue);

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new PropertyValue(property, propertyValue));

            return this;
        }


        public PersistenceSpecification<T> CheckList<LIST>(Expression<System.Func<T, object>> expression,
                                                           IList<LIST> propertyValue)
        {
            foreach (LIST item in propertyValue)
            {
                _repository.Save(item);
            }

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new ListValue<LIST>(property, propertyValue));

            return this;
        }

        public void VerifyTheMappings()
        {
            // Create the initial copy
            var first = new T();

            // Set the "suggested" properties, including references
            // to other entities and possibly collections
            _allProperties.ForEach(p => p.SetValue(first));

            // Save the first copy
            _repository.Save(first);

            // Get a completely different IRepository
            var secondRepository = createRepository();

            // "Find" the same entity from the second IRepository
            var second = secondRepository.Find<T>(first.Id);

            // Validate that each specified property and value
            // made the round trip
            // It's a bit naive right now because it fails on the first failure
            _allProperties.ForEach(p => p.CheckValue(second));
        }

        #region Nested type: ListValue

        internal class ListValue<T> : PropertyValue
        {
            private readonly IList<T> _expected;

            public ListValue(PropertyInfo property, IList<T> propertyValue)
                : base(property, propertyValue)
            {
                _expected = propertyValue;
            }

            internal override void CheckValue(object target)
            {
                var actual = (IList<T>)_property.GetValue(target, null);
                assertGenericListMatches(actual, _expected);
            }

            private static void assertGenericListMatches<T>(IList<T> actual, IList<T> expected)
            {
                ArrayList list = new ArrayList(actual.ToArray());

                if (expected.Count != list.Count)
                {
                    throw new ApplicationException("The counts between actual and expected do not match");
                }


                for (int i = 0; i < expected.Count; i++)
                {
                    object expectedValue = expected[i];
                    var actualValue = actual[i];
                    if (!expectedValue.Equals(actualValue))
                    {
                        string message = 
                            string.Format(
                                "Expected '{0}' but got '{1}' at position {2}", 
                                expectedValue,
                                actualValue, i);

                        throw new ApplicationException(message);
                    }
                }
            }
        }

        #endregion

        #region Nested type: PropertyValue

        internal class PropertyValue
        {
            protected readonly PropertyInfo _property;
            protected readonly object _propertyValue;

            internal PropertyValue(PropertyInfo property, object propertyValue)
            {
                _property = property;
                _propertyValue = propertyValue;
            }

            internal virtual void SetValue(object target)
            {
                try
                {
                    _property.SetValue(target, _propertyValue, null);
                }
                catch (Exception e)
                {
                    string message = "Error while trying to set property " + _property.Name;
                    throw new ApplicationException(message, e);
                }
            }

            internal virtual void CheckValue(object target)
            {
                object actual = _property.GetValue(target, null);
                if (!_propertyValue.Equals(actual))
                {
                    string message =
                        string.Format(
                            "Expected '{0}' but got '{1}' for Property '{2}'",
                            _propertyValue,
                            actual, _property.Name);

                    throw new ApplicationException(message);
                }
            }
        }

        #endregion
    }
}
