using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNHibernate.Testing.Fixtures.AutoMappingAlterations
{
	public abstract class AbstractOveride<T> : IAutoMappingOverride<T>
	{
		public void Override(AutoMapping<T> mapping)
		{
			mapping.BatchSize(10);
		}
	}
}
