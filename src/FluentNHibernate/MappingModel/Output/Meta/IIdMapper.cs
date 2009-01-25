using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Mapping;

namespace FluentNHibernate.MappingModel.Output.Meta
{
	public interface IIdMapper
	{
		void Bind(IdMapping idMapping, PersistentClass clazz);
	}
}
