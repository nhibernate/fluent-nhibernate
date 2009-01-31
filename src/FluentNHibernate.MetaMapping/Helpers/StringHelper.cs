using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MetaMapping.Helpers
{
	public static class StrHelper
	{
		public static bool IsNullOrEmpty(this string thiz)
		{
			return string.IsNullOrEmpty(thiz);
		}
		public static string ValueOrDefault(this string thiz,string @default)
		{
			return thiz.IsNullOrEmpty() ? @default : thiz;
		}
	}
}
