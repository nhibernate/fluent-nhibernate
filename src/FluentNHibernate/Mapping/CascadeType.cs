namespace FluentNHibernate.Mapping
{
	public struct CascadeType
	{
		internal string Type;
		private CascadeType(string type)
		{
			Type = type;
		}

		public static readonly CascadeType All = new CascadeType("all");
		public static readonly CascadeType None = new CascadeType("none");
		public static readonly CascadeType SaveUpdate = new CascadeType("save-update");
		public static readonly CascadeType Delete = new CascadeType("delete");
	}
}
