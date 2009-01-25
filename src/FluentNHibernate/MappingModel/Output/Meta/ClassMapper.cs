using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output.Meta
{
	public class ClassMapper:IClassMapper
	{
		public ClassMapper(Mappings mappings,Dialect dialect,IIdMapper idMapper)
		{
			this.mappings = mappings;
			this.dialect = dialect;
			this.idMapper = idMapper;
		}

		private readonly IIdMapper idMapper;
		private readonly Mappings mappings;
		private readonly Dialect dialect;//I think we should get rid of this



		#region IClassMapper Members

		public void Bind(ClassMapping mapping)
		{
			Type mappedType = Type.GetType(mapping.Name);
			var rootClass = new RootClass();
			rootClass.IsLazy = true;
			rootClass.EntityName = mappedType.FullName;
			rootClass.ClassName = mappedType.AssemblyQualifiedName;
			rootClass.ProxyInterfaceName = mappedType.AssemblyQualifiedName;
			string tableName = mappedType.Name;//TODO: need to have real table name
			Table table = mappings.AddTable(null, null, tableName, null, rootClass.IsAbstract.GetValueOrDefault());
			((ITableOwner)rootClass).Table = table;
			rootClass.IsMutable = true;
			idMapper.Bind(mapping.Id.As<IdMapping>(),rootClass);

			this.mappings.AddClass(rootClass);
		}

		#endregion
	}
}
