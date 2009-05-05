namespace FluentNHibernate.Mapping
{
    public class CollectionCascadeExpression<TParentpart> : CascadeExpression<TParentpart>, ICollectionCascadeExpression
        where TParentpart : IHasAttributes
	{
		public CollectionCascadeExpression(TParentpart mappingPart)
			: base(mappingPart)
		{}

		public TParentpart AllDeleteOrphan()
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
