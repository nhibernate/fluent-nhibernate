using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Mapping;

namespace FluentNHibernate.MetaMapping
{
	public class ClassIdBinder : ClassBinder
	{

		public ClassIdBinder(ClassBinder parent)
			: base(parent)
		{

		}

		public void BindId(IdMapping idMapping, PersistentClass clazz, Table table)
		{
			SimpleValue id = new SimpleValue(table);
			id.TypeName = idMapping.PropertyInfo.PropertyType.FullName;
			var property = CreateProperty(clazz, id, idMapping.PropertyInfo);
			clazz.IdentifierProperty = property;
			id.Table = clazz.Table;
			clazz.Identifier = id;
			foreach (var column in idMapping.Columns)
			{
				BindColumn(id, column);
			}
			BindGenerator(idMapping, id);
			id.Table.SetIdentifierValue(id);
		}
		protected virtual void BindGenerator(IdMapping idMapping, SimpleValue value)
		{
			value.IdentifierGeneratorStrategy = idMapping.Generator.ClassName;
		}
	}
}
