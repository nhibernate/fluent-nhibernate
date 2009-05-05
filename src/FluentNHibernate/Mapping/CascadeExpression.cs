namespace FluentNHibernate.Mapping
{
    public class CascadeExpression<TParentPart> : ICascadeExpression
        where TParentPart : IHasAttributes
	{
		protected TParentPart MappingPart { get; set; }

		public CascadeExpression(TParentPart mappingPart)
		{
			MappingPart = mappingPart;
		}

		public TParentPart All()
		{
			MappingPart.SetAttribute("cascade", "all");
			return MappingPart;
		}

        void ICascadeExpression.All()
        {
            All();
        }

		public TParentPart None()
		{
			MappingPart.SetAttribute("cascade", "none");
			return MappingPart;
		}

        void ICascadeExpression.None()
        {
            None();
        }

		public TParentPart SaveUpdate()
		{
			MappingPart.SetAttribute("cascade", "save-update");
			return MappingPart;
		}

        void ICascadeExpression.SaveUpdate()
        {
            SaveUpdate();
        }

		public TParentPart Delete()
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
