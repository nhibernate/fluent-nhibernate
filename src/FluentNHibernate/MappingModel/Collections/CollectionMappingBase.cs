using System.Reflection;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase : MappingBase, ICollectionMapping
    {
        private readonly AttributeStore<ICollectionMapping> attributes;
        public KeyMapping Key { get; set; }
        public ICollectionContentsMapping Contents { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public CollectionMappingBase(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<ICollectionMapping>(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Key != null)
                visitor.Visit(Key);

            if (Contents != null)
                visitor.Visit(Contents);
        }

        AttributeStore<ICollectionMapping> ICollectionMapping.Attributes
        {
            get { return attributes; }
        }

        public bool IsLazy
        {
            get { return attributes.Get(x => x.IsLazy); }
            set { attributes.Set(x => x.IsLazy, value); }
        }

        public bool IsInverse
        {
            get { return attributes.Get(x => x.IsInverse); }
            set { attributes.Set(x => x.IsInverse, value); }
        }

        public string Name
        {
            get { return attributes.Get(x => x.Name); }
            set { attributes.Set(x => x.Name, value); }
        }

        public bool IsNameSpecified
        {
            get { return attributes.IsSpecified(x => x.Name); }
        }

    }

}