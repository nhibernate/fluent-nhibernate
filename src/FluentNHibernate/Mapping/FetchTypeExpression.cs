namespace FluentNHibernate.Mapping
{
	public class FetchTypeExpression<TParentpart> 
		where TParentpart : IHasAttributes
	{
		private readonly Cache<string, string> properties;
		protected TParentpart MappingPart { get; set; }

		public FetchTypeExpression(TParentpart mappingPart, Cache<string, string> properties)
		{
			MappingPart = mappingPart;
			this.properties = properties;
		}

		public TParentpart Join()
		{
			properties.Store("fetch", "join");
			return MappingPart;
		}

		public TParentpart Select()
		{
			properties.Store("fetch", "select");
			return MappingPart;
		}
	}
}
