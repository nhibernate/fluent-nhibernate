using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Utils;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate;

namespace FluentNHibernate.Testing
{
    public class PersistenceSpecification<T>
    {
        private readonly List<PropertyValue> allProperties = new List<PropertyValue>();
        private readonly ISession currentSession;
        private readonly IEqualityComparer entityEqualityComparer;
        private readonly bool hasExistingSession;

        public PersistenceSpecification(ISessionSource source)
            : this(source.CreateSession())
        {
        }

        public PersistenceSpecification(ISessionSource source, IEqualityComparer entityEqualityComparer)
            : this(source.CreateSession(), entityEqualityComparer)
        {
        }

        public PersistenceSpecification(ISession session)
            : this(session, null)
        {
        }

        public PersistenceSpecification(ISession session, IEqualityComparer entityEqualityComparer)
        {
            currentSession = session;
            hasExistingSession = currentSession.Transaction != null && currentSession.Transaction.IsActive;
            this.entityEqualityComparer = entityEqualityComparer;
        }

        public PersistenceSpecification<T> CheckProperty(Expression<Func<T, object>> expression, object propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            allProperties.Add(new PropertyValue(property, propertyValue, entityEqualityComparer));

            return this;
        }

        public PersistenceSpecification<T> CheckProperty<TElement>(Expression<Func<T, Array>> expression, IList<TElement> propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            allProperties.Add(new ListValue<TElement>(property, propertyValue, entityEqualityComparer));

            return this;
        }
        public PersistenceSpecification<T> CheckReference(Expression<Func<T, object>> expression, object propertyValue)
        {
            TransactionalSave(propertyValue);

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            allProperties.Add(new PropertyValue(property, propertyValue, entityEqualityComparer));

            return this;
        }


        public PersistenceSpecification<T> CheckList<TList>(Expression<Func<T, object>> expression,
            IList<TList> propertyValue)
        {
            foreach (TList item in propertyValue)
            {
                TransactionalSave(item);
            }

            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            allProperties.Add(new ListValue<TList>(property, propertyValue, entityEqualityComparer));

            return this;
        }

        /// <summary>
        /// Checks a list of components for validity.
        /// </summary>
        /// <typeparam name="TList">Type of list element</typeparam>
        /// <param name="expression">Property</param>
        /// <param name="propertyValue">Value to save</param>
        public PersistenceSpecification<T> CheckComponentList<TList>(Expression<Func<T, object>> expression, IList<TList> propertyValue)
        {
            PropertyInfo property = ReflectionHelper.GetProperty(expression);
            allProperties.Add(new ListValue<TList>(property, propertyValue, entityEqualityComparer));

            return this;
        }

        public void VerifyTheMappings()
        {
            // CreateProperties the initial copy
            var first = typeof(T).InstantiateUsingParameterlessConstructor();

            // Set the "suggested" properties, including references
            // to other entities and possibly collections
            allProperties.ForEach(p => p.SetValue(first));

            // Save the first copy
            TransactionalSave(first);

            object firstId = currentSession.GetIdentifier(first);

            // Clear and reset the current session
            currentSession.Flush();
            currentSession.Clear();

            // "Find" the same entity from the second IRepository
            var second = currentSession.Get<T>(firstId);

            // Validate that each specified property and value
            // made the round trip
            // It's a bit naive right now because it fails on the first failure
            allProperties.ForEach(p => p.CheckValue(second));
        }

        private void TransactionalSave(object propertyValue)
        {
            if (hasExistingSession)
            {
                currentSession.Save(propertyValue);
            }
            else
            {
                using (var tx = currentSession.BeginTransaction())
                {
                    currentSession.Save(propertyValue);
                    tx.Commit();
                }
            }
        }

        #region Nested type: ListValue

        internal class ListValue<TListelement> : PropertyValue
        {
            private readonly IList<TListelement> expected;

            public ListValue(PropertyInfo property, IList<TListelement> propertyValue, IEqualityComparer entityEqualityComparer)
                : base(property, propertyValue, entityEqualityComparer)
            {
                expected = propertyValue;
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
                    if (property.PropertyType.IsAssignableFrom(typeof(ISet<TListelement>)))
                        collection = new HashedSet<TListelement>(expected);
                    else if (property.PropertyType.IsAssignableFrom(typeof(ISet)))
                        collection = new HashedSet((ICollection)expected);
                    else if (property.PropertyType.IsArray)
                    {
                        collection = Array.CreateInstance(typeof(TListelement), expected.Count);
                        Array.Copy((Array)expected, (Array)collection, expected.Count);
                    }
                    else
                        collection = new List<TListelement>(expected);

                    property.SetValue(target, collection, null);
                }
                catch (Exception e)
                {
                    string message = "Error while trying to set property " + property.Name;
                    throw new ApplicationException(message, e);
                }
            }

            internal override void CheckValue(object target)
            {
                var actual = (IEnumerable<TListelement>)property.GetValue(target, null);
                AssertGenericListMatches(actual, expected);
            }

            private void AssertGenericListMatches<TItem>(IEnumerable<TItem> actualEnumerable, IEnumerable<TItem> expectedEnumerable)
            {
                if (actualEnumerable == null)
                    throw new ArgumentNullException("actualEnumerable",
                        "Actual and expected are not equal (Actual was null).");
                if (expectedEnumerable == null)
                    throw new ArgumentNullException("expectedEnumerable",
                        "Actual and expected are not equal (expected was null).");

                var actualList = actualEnumerable.ToList();
                var expectedList = expectedEnumerable.ToList();

                if (actualList.Count != expectedList.Count)
                    throw new ApplicationException("Actual count does not equal expected count");

                var equalsFunc = (entityEqualityComparer != null)
                    ? new Func<object, object, bool>((a, b) => entityEqualityComparer.Equals(a, b))
                    : new Func<object, object, bool>(Equals);

                for (var i = 0; i < actualList.Count; i++)
                {
                    if (equalsFunc(actualList[i], expectedList[i])) continue;

                    var message = string.Format("Expected '{0}' but got '{1}' at position {2}",
                        expectedList[i],
                        actualList[i],
                        i);

                    throw new ApplicationException(message);
                }
            }
        }

        #endregion

        #region Nested type: PropertyValue

        internal class PropertyValue
        {
            protected readonly PropertyInfo property;
            protected readonly object propertyValue;
            protected readonly IEqualityComparer entityEqualityComparer;

            internal PropertyValue(PropertyInfo property, object propertyValue, IEqualityComparer entityEqualityComparer)
            {
                this.property = property;
                this.propertyValue = propertyValue;
                this.entityEqualityComparer = entityEqualityComparer;
            }

            internal virtual void SetValue(object target)
            {
                try
                {
                    property.SetValue(target, propertyValue, null);
                }
                catch (Exception e)
                {
                    string message = "Error while trying to set property " + property.Name;
                    throw new ApplicationException(message, e);
                }
            }

            internal virtual void CheckValue(object target)
            {
                object actual = property.GetValue(target, null);

                bool areEqual;
                if (entityEqualityComparer != null)
                {
                    areEqual = entityEqualityComparer.Equals(propertyValue, actual);
                }
                else
                {
                    areEqual = propertyValue.Equals(actual);
                }

                if (!areEqual)
                {
                    string message =
                        string.Format(
                            "Expected '{0}' but got '{1}' for Property '{2}'",
                            propertyValue,
                            actual, property.Name);

                    throw new ApplicationException(message);
                }
            }
        }

        #endregion
    }
}