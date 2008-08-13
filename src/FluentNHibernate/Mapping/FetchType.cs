namespace FluentNHibernate.Mapping
{
	public struct FetchType
	{
		internal string Type;
		public FetchType(string type)
		{
			Type = type;
		}

		public static readonly FetchType Join = new FetchType("join");
		public static readonly FetchType Select = new FetchType("select");
	}
}
