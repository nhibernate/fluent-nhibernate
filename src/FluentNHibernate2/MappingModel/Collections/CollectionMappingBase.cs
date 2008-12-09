using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase<T> : MappingBase<T>, ICollectionMapping where T : class, new()
    {
        private readonly CollectionAttributes _attributes;
        protected KeyMapping _key;
        protected ICollectionContentsMapping _contents;

        public CollectionMappingBase()
        {
            _attributes = new CollectionAttributes(this);
        }

        public CollectionAttributes Attributes
        {
            get { return _attributes; }
        }

        public abstract string Name { get; set; }
        public abstract KeyMapping Key { get; set; }
        public abstract ICollectionContentsMapping Contents { get; set; }
    }

}