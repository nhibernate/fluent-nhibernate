using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output.Meta
{
	public class IdMapper:IIdMapper
	{
		#region IIdMapper Members

		public void Bind(IdMapping idMapping, PersistentClass clazz)
		{
			//TODO: use entity type from classmapping next time
			Type entityType = Type.GetType(clazz.ClassName);
			var table = clazz.Table;
			var identifier = new SimpleValue(table);
			identifier.IdentifierGeneratorStrategy = idMapping.Generator.ClassName;
			clazz.Identifier = identifier;
			PropertyInfo result = entityType.GetProperty(idMapping.Name);
			string propName = idMapping.Name;
			AddColumn(identifier, idMapping.Columns);
			CreateIdentifierProperty(clazz, identifier, propName);
			identifier.Table.SetIdentifierValue(identifier);
		}
		protected virtual void AddColumn(SimpleValue id,IEnumerable<ColumnMapping> columns)
		{
			foreach (var column in columns)
			{
				var col = column.AsColumn();
				col.Value = id;

				if (id.Table != null)
					id.Table.AddColumn(col);

				id.AddColumn(col);
			}
		}
		protected virtual void CreateIdentifierProperty(PersistentClass clazz,SimpleValue identifier,string propName)
		{
			identifier.SetTypeUsingReflection(clazz.MappedClass == null ? null : clazz.MappedClass.AssemblyQualifiedName,
														  propName, "property");//TODO: take it from configuration


			var property = new Property(identifier)
			{
				Name = propName,
				PropertyAccessorName = "property",//TODO: use it from configuration
				//Cascade = //TODO: take it from mappings;
				IsUpdateable = true,
				IsInsertable = true,
				IsOptimisticLocked = true,
				Generation = PropertyGeneration.Never
			};

			clazz.IdentifierProperty = property;
		}


		#endregion
	}
}
