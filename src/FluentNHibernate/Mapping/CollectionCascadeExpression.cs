namespace FluentNHibernate.Mapping
{
    public class CollectionCascadeExpression<PARENTPART> : CascadeExpression<PARENTPART>, ICollectionCascadeExpression
        where PARENTPART : IHasAttributes
	{
		public CollectionCascadeExpression(PARENTPART mappingPart)
			: base(mappingPart)
		{}

		public PARENTPART AllDeleteOrphan()
		{
			MappingPart.SetAttribute("cascade", "all-delete-orphan");
			return MappingPart;
		}

        void ICollectionCascadeExpression.AllDeleteOrphan()
        {
            AllDeleteOrphan();
        }
	}
}
