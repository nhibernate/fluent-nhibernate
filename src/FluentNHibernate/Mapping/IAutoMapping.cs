namespace FluentNHibernate.Mapping
{
	public interface IAutoMapping : IMapping
	{
		void GenerateAutoMappings();
		void GenerateAutoMappings(string idPropertyName);
	}
}
