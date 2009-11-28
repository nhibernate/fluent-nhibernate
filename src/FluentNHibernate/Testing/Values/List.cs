using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Testing.Values
{
    public class List<T, TListElement> : Property<T, IEnumerable<TListElement>>
    {
        private readonly IEnumerable<TListElement> _expected;
        private Action<T, PropertyInfo, IEnumerable<TListElement>> _valueSetter;

        public List(PropertyInfo property, IEnumerable<TListElement> value)
            : base(property, value)
        {
            _expected = value;
        }

        public override Action<T, PropertyInfo, IEnumerable<TListElement>> ValueSetter
        {
            get
            {
                if (_valueSetter != null)
                {
                    return _valueSetter;
                }

                return (target, propertyInfo, value) =>
                {
                    object collection;

                    // sorry guys - create an instance of the collection type because we can't rely
                    // on the user to pass in the correct collection type (especially if they're using
                    // an interface). I've tried to create the common ones, but I'm sure this won't be
                    // infallible.
                    if (propertyInfo.PropertyType.IsAssignableFrom(typeof(ISet<TListElement>)))
                    {
                        collection = new HashedSet<TListElement>(Expected.ToList());
                    }
                    else if (propertyInfo.PropertyType.IsAssignableFrom(typeof(ISet)))
                    {
                        collection = new HashedSet((ICollection)Expected);
                    }
                    else if (propertyInfo.PropertyType.IsArray)
                    {
                        collection = Array.CreateInstance(typeof(TListElement), Expected.Count());
                        Array.Copy((Array)Expected, (Array)collection, Expected.Count());
                    }
                    else
                    {
                        collection = new List<TListElement>(Expected);
                    }

                    propertyInfo.SetValue(target, collection, null);
                };
            }
            set { _valueSetter = value; }
        }

        protected IEnumerable<TListElement> Expected
        {
            get { return _expected; }
        }

        public override void CheckValue(object target)
        {
            var actual = PropertyInfo.GetValue(target, null) as IEnumerable;
            AssertGenericListMatches(actual, Expected);
        }

        private void AssertGenericListMatches(IEnumerable actualEnumerable, IEnumerable<TListElement> expectedEnumerable)
        {
            if (actualEnumerable == null)
            {
                throw new ArgumentNullException("actualEnumerable",
                    "Actual and expected are not equal (actual was null).");
            }
            if (expectedEnumerable == null)
            {
                throw new ArgumentNullException("expectedEnumerable",
                    "Actual and expected are not equal (expected was null).");
            }

            List<object> actualList = new List<object>();
            foreach (var item in actualEnumerable)
            {
                actualList.Add(item);
            }

            var expectedList = expectedEnumerable.ToList();

            if (actualList.Count != expectedList.Count)
            {
                throw new ApplicationException("Actual count does not equal expected count");
            }

            var equalsFunc = (EntityEqualityComparer != null)
                ? new Func<object, object, bool>((a, b) => EntityEqualityComparer.Equals(a, b))
                : new Func<object, object, bool>(Equals);

            for (var i = 0; i < actualList.Count; i++)
            {
                if (equalsFunc(actualList[i], expectedList[i]))
                {
                    continue;
                }

                var message = String.Format("Expected '{0}' but got '{1}' at position {2}",
                    expectedList[i],
                    actualList[i],
                    i);

                throw new ApplicationException(message);
            }
        }
    }
}