using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentNHibernate
{
	public class Cache<TKey, TValue> : IEnumerable<TValue> where TValue : class
	{
		private readonly Dictionary<TKey, TValue> values = new Dictionary<TKey, TValue>();
		private readonly Func<TKey, TValue> onMissing = delegate(TKey key)
		                                               	{
		                                               		string message = string.Format("Key '{0}' could not be found", key);
		                                               		throw new KeyNotFoundException(message);
		                                               	};

		private Func<TValue, TKey> getKey = delegate { throw new NotImplementedException(); };

		public Cache()
		{
		}

		public Cache(Dictionary<TKey, TValue> values, Func<TKey, TValue> onMissing)
			: this(onMissing)
		{
			this.values = values;
		}

		public Cache(Func<TKey, TValue> onMissing)
		{
			this.onMissing = onMissing;
		}

		public Func<TValue, TKey> GetKey
		{
			get { return getKey; }
			set { getKey = value; }
		}

		public int Count
		{
			get
			{
				return values.Count;
			}
		}

		public TValue First
		{
			get
			{
				foreach (KeyValuePair<TKey, TValue> pair in values)
				{
					return pair.Value;
				}

				return null;
			}
		}

		public TValue Store(TKey key, TValue value)
		{
			if (values.ContainsKey(key))
			{
				values[key] = value;
			}
			else
			{
				values.Add(key, value);
			}

			return value;
		}

		public void Fill(TKey key, TValue value)
		{
			if (values.ContainsKey(key))
			{
				return;
			}

			values.Add(key, value);
		}

		public TValue Get(TKey key)
		{
			if (!values.ContainsKey(key))
			{
				TValue value = onMissing(key);
				values.Add(key, value);
			}

			return values[key];
		}

		public void Each(Action<TValue> action)
		{
			foreach (KeyValuePair<TKey, TValue> pair in values)
			{
				action(pair.Value);
			}
		}

		public void ForEachPair(Action<TKey, TValue> action)
		{
			foreach (var pair in values)
			{
				action(pair.Key, pair.Value);
			}
		}

		public bool Has(TKey key)
		{
			return values.ContainsKey(key);
		}

		public bool Exists(Predicate<TValue> predicate)
		{
			bool returnValue = false;

			Each(delegate(TValue value)
			     	{
			     		returnValue |= predicate(value);
			     	});

			return returnValue;
		}

		public TValue Find(Predicate<TValue> predicate)
		{
			foreach (KeyValuePair<TKey, TValue> pair in values)
			{
				if (predicate(pair.Value))
				{
					return pair.Value;
				}
			}

			return null;
		}

		public TValue[] GetAll()
		{
			TValue[] returnValue = new TValue[Count];
			values.Values.CopyTo(returnValue, 0);

			return returnValue;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TValue>)this).GetEnumerator();
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return values.Values.GetEnumerator();
		}

		public void Remove(TKey key)
		{
			if (values.ContainsKey(key))
			{
				values.Remove(key);
			}
		}

		public void ClearAll()
		{
			values.Clear();
		}
	}
}
