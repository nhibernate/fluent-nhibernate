namespace FluentNHibernate.Mapping
{
    public class CascadeExpression<PARENTPART> : ICascadeExpression
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

        void ICascadeExpression.All()
        {
            All();
        }

		public PARENTPART None()
		{
			MappingPart.SetAttribute("cascade", "none");
			return MappingPart;
		}

        void ICascadeExpression.None()
        {
            None();
        }

		public PARENTPART SaveUpdate()
		{
			MappingPart.SetAttribute("cascade", "save-update");
			return MappingPart;
		}

        void ICascadeExpression.SaveUpdate()
        {
            SaveUpdate();
        }

		public PARENTPART Delete()
		{
			MappingPart.SetAttribute("cascade", "delete");
			return MappingPart;
		}

        void ICascadeExpression.Delete()
        {
            Delete();
        }
	}
}
