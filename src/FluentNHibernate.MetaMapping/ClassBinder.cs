using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Util;
using FluentNHibernate.MetaMapping.Helpers;
namespace FluentNHibernate.MetaMapping
{
	public class ClassBinder
	{
		protected readonly Mappings mappings;
		protected readonly Dialect dialect;

		public ClassBinder(Mappings mappings, Dialect dialect)
		{
			this.mappings = mappings;
			this.dialect = dialect;
		}


		protected ClassBinder(ClassBinder parent)
		{
			mappings = parent.mappings;
		}

		protected ClassBinder(Mappings mappings)
		{
			this.mappings = mappings;
		}

		protected virtual void BindPersistentClassCommonValues(ClassMapping classMapping, PersistentClass model)
		{
			//TODO: Introduce all those
			model.DiscriminatorValue = model.EntityName;
			model.DynamicUpdate = false; ;
			model.DynamicInsert = false;
			string qualifiedName = model.MappedClass == null ? model.EntityName : model.MappedClass.AssemblyQualifiedName;
			mappings.AddImport(qualifiedName, model.EntityName);
			if (mappings.IsAutoImport && model.EntityName.IndexOf('.') > 0)
				mappings.AddImport(qualifiedName, StringHelper.Unqualify(model.EntityName));

			model.BatchSize = 0;
			model.SelectBeforeUpdate = false;
			model.OptimisticLockMode = NHibernate.Engine.Versioning.OptimisticLock.Version;
			model.IsAbstract = false;
			model.IsLazy = true;
		}
		protected virtual void BindClass(ClassMapping classMapping, PersistentClass clazz)
		{
			Type type = classMapping.Type;
			string entityName = type.FullName;
			clazz.EntityName = entityName;
			clazz.ClassName = type.AssemblyQualifiedName;
			BindPocoRepresentation(classMapping, clazz);
			BindPersistentClassCommonValues(classMapping, clazz);

		}
		protected virtual void BindPocoRepresentation(ClassMapping classMapping, PersistentClass clazz)
		{
			string className = classMapping.Type.AssemblyQualifiedName;
			clazz.ClassName = className;
			clazz.ProxyInterfaceName = className;
		}
		protected virtual void BindProperties(PersistentClass model, ClassMapping mapping)
		{
			foreach (var propertyMapping in mapping.Properties)
			{
				var value = new SimpleValue(model.Table);
				BindSimpleValue(value, propertyMapping);
				BindProperty(propertyMapping, value, model);
				BindColumn(value, new ColumnMapping
				{
					Name = propertyMapping.Name,
					PropertyInfo = propertyMapping.PropertyInfo,
                    Length=propertyMapping.Length,
					IsNotNullable = propertyMapping.IsNotNullable
				});

			}
			foreach (var propertyMapping in mapping.Collections)
			{
				//TODO: Implement collection mapping
			}
		}
		protected virtual void BindSimpleValue(SimpleValue simpleValue, PropertyMapping propertyMapping)
		{
			simpleValue.TypeName = propertyMapping.PropertyInfo.PropertyType.FullName;
		}
		protected virtual void BindColumn(SimpleValue model, ColumnMapping columnMapping)
		{
			Table table = model.Table;
			var column = CreateColumn(model, columnMapping);
			if (table != null)
				table.AddColumn(column);
			model.AddColumn(column);
		}
		protected virtual void BindProperty(PropertyMapping propertyMapping, IValue value, PersistentClass model)
		{
			var property = CreateProperty(model, value, propertyMapping.PropertyInfo);
			model.AddProperty(property);
		}
		protected virtual Property CreateProperty(PersistentClass model, IValue value, PropertyInfo propInfo)
		{
			Property property = new Property
			{
				Name = propInfo.Name,
				PropertyAccessorName = mappings.DefaultAccess,
				Cascade = mappings.DefaultCascade,
				IsUpdateable = true,
				IsInsertable = true,
				IsOptimisticLocked = true,
				Generation = PropertyGeneration.Never,
				Value = value
			};
			return property;
		}
		protected virtual Column CreateColumn(IValue value, ColumnMapping columnMapping)
		{
			var columnName = columnMapping.Name;
			if (columnName.IsNullOrEmpty())
				columnName = this.mappings.NamingStrategy.ColumnName(columnMapping.PropertyInfo.Name);
			var column = new Column(columnName)
			{
				Value = value,
				IsNullable = (!columnMapping.IsNotNullable),
                Length=columnMapping.Length,
                CheckConstraint = columnMapping.Check,
				IsUnique=columnMapping.IsUnique,
                SqlType=columnMapping.SqlType,
			};
			return column;
		}
	}
}
