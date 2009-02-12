using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;
using Iesi.Collections.Generic;
using Iesi.Collections;
using NHibernate;
using NHibernate.Util;

namespace FluentNHibernate.Testing
{
    public class PersistenceSpecification<T>
    {
        private readonly List<PropertyValue> _allProperties = new List<PropertyValue>();
        private readonly ISession _currentSession;
        private readonly IEqualityComparer _entityEqualityComparer;

        public PersistenceSpecification(ISessionSource source)
            : this(source.CreateSession())
        {
        }

        public PersistenceSpecification(ISessionSource source, IEqualityComparer entityEqualityComparer) 
            : this(source.CreateSession(), entityEqualityComparer)
        {
        }

        public PersistenceSpecification(ISession session) : this(session, null)
        {
        }

        public PersistenceSpecification(ISession session, IEqualityComparer entityEqualityComparer)
        {
            _currentSession = session;
            _entityEqualityComparer = entityEqualityComparer;
        }

        public PersistenceSpecification<T> CheckProperty(Expression<Func<T, object>> expression, object propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new PropertyValue(property, propertyValue, _entityEqualityComparer));

            return this;
        }

        public PersistenceSpecification<T> CheckProperty<ELEMENTTYPE>(Expression<Func<T, Array>> expression, IList<ELEMENTTYPE> propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new ListValue<ELEMENTTYPE>(property, propertyValue, _entityEqualityComparer));

            return this;
        }
        public PersistenceSpecification<T> CheckReference(Expression<Func<T, object>> expression, object propertyValue)
        {
            TransactionalSave(propertyValue);

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new PropertyValue(property, propertyValue, _entityEqualityComparer));

            return this;
        }


        public PersistenceSpecification<T> CheckList<LIST>(Expression<Func<T, object>> expression,
            IList<LIST> propertyValue)
        {
            foreach (LIST item in propertyValue)
            {
                TransactionalSave(item);
            }

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new ListValue<LIST>(property, propertyValue, _entityEqualityComparer));

            return this;
        }

        /// <summary>
        /// Checks a list of components for validity.
        /// </summary>
        /// <typeparam name="LIST">Type of list element</typeparam>
        /// <param name="expression">Property</param>
        /// <param name="propertyValue">Value to save</param>
        public PersistenceSpecification<T> CheckComponentList<LIST>(Expression<Func<T, object>> expression, IList<LIST> propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            _allProperties.Add(new ListValue<LIST>(property, propertyValue, _entityEqualityComparer));

            return this;
        }

        public void VerifyTheMappings()
        {
            // CreateProperties the initial copy
            var first = typeof(T).InstantiateUsingParameterlessConstructor();

            // Set the "suggested" properties, including references
            // to other entities and possibly collections
            _allProperties.ForEach(p => p.SetValue(first));

            // Save the first copy
            TransactionalSave(first);

            object firstId = _currentSession.GetIdentifier(first);

            // Clear and reset the current session
            _currentSession.Flush();
            _currentSession.Clear();

            // "Find" the same entity from the second IRepository
            var second = _currentSession.Get<T>(firstId);

            // Validate that each specified property and value
            // made the round trip
            // It's a bit naive right now because it fails on the first failure
            _allProperties.ForEach(p => p.CheckValue(second));
        }

        private void TransactionalSave(object propertyValue)
        {
            using (var tx = _currentSession.BeginTransaction())
            {
                _currentSession.Save(propertyValue);
                tx.Commit();
            }
        }

        #region Nested type: ListValue

        internal class ListValue<LISTELEMENT> : PropertyValue
        {
            private readonly IList<LISTELEMENT> _expected;

            public ListValue(PropertyInfo property, IList<LISTELEMENT> propertyValue, IEqualityComparer entityEqualityComparer)
                : base(property, propertyValue, entityEqualityComparer)
            {
                _expected = propertyValue;
            }

            internal override void SetValue(object target)
            {
                try
                {
                    object collection;

                    // sorry guys - create an instance of the collection type because we can't rely
                    // on the user to pass in the correct collection type (especially if they're using
                    // an interface). I've tried to create the common ones, but I'm sure this won't be
                    // infallable.
                    if (_property.PropertyType.IsAssignableFrom(typeof(ISet<LISTELEMENT>)))
                        collection = new HashedSet<LISTELEMENT>(_expected);
                    else if (_property.PropertyType.IsAssignableFrom(typeof(ISet)))
                        collection = new HashedSet((ICollection)_expected);
                    else if (_property.PropertyType.IsArray)
                    {
                        collection = Array.CreateInstance(typeof (LISTELEMENT), _expected.Count);
                        Array.Copy((Array)_expected, (Array)collection, _expected.Count);
                    }
                    else
                        collection = new List<LISTELEMENT>(_expected);

                    _property.SetValue(target, collection, null);
                }
                catch (Exception e)
                {
                    string message = "Error while trying to set property " + _property.Name;
                    throw new ApplicationException(message, e);
                }
            }

            internal override void CheckValue(object target)
            {
                var actual = (IEnumerable<LISTELEMENT>)_property.GetValue(target, null);
                assertGenericListMatches<LISTELEMENT>(actual, _expected);
            }

            private void assertGenericListMatches<ITEM>(IEnumerable<ITEM> actualEnumerable, IEnumerable<ITEM> expectedEnumerable)
            {
                if (actualEnumerable == null)
                    throw new ArgumentNullException("actualEnumerable",
                        "Actual and expected are not equal (Actual was null).");
                if (expectedEnumerable == null)
                    throw new ArgumentNullException("expectedEnumerable",
                        "Actual and expected are not equal (expected was null).");

                var actual = actualEnumerable.ToList();
                var expected = expectedEnumerable.ToList();

                if (actual.Count != expected.Count)
                    throw new ApplicationException("Actual count does not equal expected count");

                var equalsFunc = (_entityEqualityComparer != null)
                    ? new Func<object, object, bool>((a, b) => _entityEqualityComparer.Equals(a, b))
                    : new Func<object, object, bool>(Equals);

                for (var i = 0; i < actual.Count; i++)
                {
                    if (equalsFunc(actual[i], expected[i])) continue;

                    var message = string.Format("Expected '{0}' but got '{1}' at position {2}",
                        expected[i],
                        actual[i],
                        i);

                    throw new ApplicationException(message);
                }
            }
        }

        #endregion

        #region Nested type: PropertyValue

        internal class PropertyValue
        {
            protected readonly PropertyInfo _property;
            protected readonly object _propertyValue;
            protected readonly IEqualityComparer _entityEqualityComparer;

            internal PropertyValue(PropertyInfo property, object propertyValue, IEqualityComparer entityEqualityComparer)
            {
                _property = property;
                _propertyValue = propertyValue;
                _entityEqualityComparer = entityEqualityComparer;
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

                bool areEqual;
                if (_entityEqualityComparer != null)
                {
                    areEqual = _entityEqualityComparer.Equals(_propertyValue, actual);
                }
                else
                {
                    areEqual = _propertyValue.Equals(actual);
                }

                if (!areEqual)
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