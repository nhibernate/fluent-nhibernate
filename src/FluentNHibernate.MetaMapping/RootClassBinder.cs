using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.MetaMapping.Helpers;
using NHibernate.Cfg;
using NHibernate.Cfg.XmlHbmBinding;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace FluentNHibernate.MetaMapping
{
	public class RootClassBinder:ClassBinder
	{
		public RootClassBinder(Mappings mappings, Dialect dialect)
			: base(mappings, dialect)
		{

		}


		public virtual void Bind(ClassMapping classMapping)
		{
			RootClass rootClass = new RootClass();
			BindClass(classMapping, rootClass);
			string schema = mappings.SchemaName;
			string catalog = mappings.CatalogName;
			string tableName = classMapping.Tablename.ValueOrDefault(
				mappings.NamingStrategy.TableName(classMapping.Type.Name));

			Table table = mappings
				.AddTable(schema, catalog, tableName, null,
						  rootClass.IsAbstract.GetValueOrDefault());//Introduce SchemaAction
			((ITableOwner)rootClass).Table = table;

			rootClass.IsMutable = true;
			rootClass.IsExplicitPolymorphism = false;
			new ClassIdBinder(this).BindId(classMapping.Id as IdMapping, rootClass, table);
			rootClass.CreatePrimaryKey(dialect);
			base.BindProperties(rootClass, classMapping);
			mappings.AddClass(rootClass);
		}
	}
}
