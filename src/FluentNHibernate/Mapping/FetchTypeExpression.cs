namespace FluentNHibernate.Mapping
{
	public class FetchTypeExpression<PARENTPART> 
		where PARENTPART : IMappingPart
	{
		private readonly Cache<string, string> _properties;
		protected PARENTPART MappingPart { get; set; }

		public FetchTypeExpression(PARENTPART mappingPart, Cache<string, string> properties)
		{
			MappingPart = mappingPart;
			_properties = properties;
		}

		public PARENTPART Join()
		{
			_properties.Store("fetch", "join");
			return MappingPart;
		}

		public PARENTPART Select()
		{
			_properties.Store("fetch", "select");
			return MappingPart;
		}
	}
}
