using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output.Meta
{
	public static class MappingHelpers
	{
		public static Column AsColumn(this ColumnMapping mapping)
		{
			Column col=new Column();
			col.Name = mapping.Name;
			col.Unique = mapping.IsUnique;
			col.Length = mapping.Length;
			col.SqlType = mapping.SqlType;
			col.IsNullable = !mapping.IsNotNullable;
			col.CheckConstraint = mapping.Check;
			return col;
		}
		public static T As<T>(this object t)
		{
			return (T)t;
		}
	}
}
