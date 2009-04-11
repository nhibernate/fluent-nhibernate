namespace FluentNHibernate.Mapping
{
	public class CascadeExpression<PARENTPART> 
		where PARENTPART : IHasAttributes
	{
		protected PARENTPART MappingPart { get; set; }

		public CascadeExpression(PARENTPART mappingPart)
		{
			MappingPart = mappingPart;
		}

		public PARENTPART All()
		{
			MappingPart.SetAttribute("cascade", "all");
			return MappingPart;
		}

		public PARENTPART None()
		{
			MappingPart.SetAttribute("cascade", "none");
			return MappingPart;
		}

		public PARENTPART SaveUpdate()
		{
			MappingPart.SetAttribute("cascade", "save-update");
			return MappingPart;
		}

		public PARENTPART Delete()
		{
			MappingPart.SetAttribute("cascade", "delete");
			return MappingPart;
		}
	}
}
