using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEnumerable=System.Collections.IEnumerable;

namespace FluentNHibernate
{
	public class Cache<KEY, VALUE> : IEnumerable<VALUE> where VALUE : class
	{
		private readonly Dictionary<KEY, VALUE> _values = new Dictionary<KEY, VALUE>();
		private readonly Func<KEY, VALUE> _onMissing = delegate(KEY key)
		                                               	{
		                                               		string message = string.Format("Key '{0}' could not be found", key);
		                                               		throw new KeyNotFoundException(message);
		                                               	};

		private Func<VALUE, KEY> _getKey = delegate { throw new NotImplementedException(); };

		public Cache()
		{
		}

		public Cache(Dictionary<KEY, VALUE> values, Func<KEY, VALUE> onMissing)
			: this(onMissing)
		{
			_values = values;
		}

		public Cache(Func<KEY, VALUE> onMissing)
		{
			_onMissing = onMissing;
		}

		public Func<VALUE, KEY> GetKey
		{
			get { return _getKey; }
			set { _getKey = value; }
		}

		public int Count
		{
			get
			{
				return _values.Count;
			}
		}

		public VALUE First
		{
			get
			{
				foreach (KeyValuePair<KEY, VALUE> pair in _values)
				{
					return pair.Value;
				}

				return null;
			}
		}

		public VALUE Store(KEY key, VALUE value)
		{
			if (_values.ContainsKey(key))
			{
				_values[key] = value;
			}
			else
			{
				_values.Add(key, value);
			}

			return value;
		}

		public void Fill(KEY key, VALUE value)
		{
			if (_values.ContainsKey(key))
			{
				return;
			}

			_values.Add(key, value);
		}

		public VALUE Get(KEY key)
		{
			if (!_values.ContainsKey(key))
			{
				VALUE value = _onMissing(key);
				_values.Add(key, value);
			}

			return _values[key];
		}

		public void Each(Action<VALUE> action)
		{
			foreach (KeyValuePair<KEY, VALUE> pair in _values)
			{
				action(pair.Value);
			}
		}

		public void ForEachPair(Action<KEY, VALUE> action)
		{
			foreach (var pair in _values)
			{
				action(pair.Key, pair.Value);
			}
		}

		public bool Has(KEY key)
		{
			return _values.ContainsKey(key);
		}

		public bool Exists(Predicate<VALUE> predicate)
		{
			bool returnValue = false;

			Each(delegate(VALUE value)
			     	{
			     		returnValue |= predicate(value);
			     	});

			return returnValue;
		}

		public VALUE Find(Predicate<VALUE> predicate)
		{
			foreach (KeyValuePair<KEY, VALUE> pair in _values)
			{
				if (predicate(pair.Value))
				{
					return pair.Value;
				}
			}

			return null;
		}

		public VALUE[] GetAll()
		{
			VALUE[] returnValue = new VALUE[Count];
			_values.Values.CopyTo(returnValue, 0);

			return returnValue;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<VALUE>)this).GetEnumerator();
		}

		public IEnumerator<VALUE> GetEnumerator()
		{
			return _values.Values.GetEnumerator();
		}

		public void Remove(KEY key)
		{
			if (_values.ContainsKey(key))
			{
				_values.Remove(key);
			}
		}

		public void ClearAll()
		{
			_values.Clear();
		}
	}
}
