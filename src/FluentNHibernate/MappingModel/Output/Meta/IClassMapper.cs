using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentNHibernate.MappingModel.Output.Meta
{
	public interface IClassMapper
	{
		void Bind(ClassMapping mapping);
	}
}
