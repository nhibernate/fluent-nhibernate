using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Collections
{
    public abstract class CollectionMappingBase : MappingBase, ICollectionMapping
    {
        private readonly CollectionAttributes _attributes;

        public CollectionMappingBase(AttributeStore underlyingStore)
        {
            _attributes = new CollectionAttributes(underlyingStore);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            if (Key != null)
                visitor.Visit(Key);

            if (Contents != null)
                visitor.Visit(Contents);
        }

        public PropertyInfo PropertyInfo { get; set; }

        CollectionAttributes ICollectionMapping.Attributes
        {
            get { return _attributes; }
        }

        public bool IsLazy
        {
            get { return _attributes.IsLazy; }
            set { _attributes.IsLazy = value; }
        }

        public bool IsInverse
        {
            get { return _attributes.IsInverse; }
            set { _attributes.IsInverse = value; }
        }

        public string Name
        {
            get { return _attributes.Name; }
            set { _attributes.Name = value;}
        }

        public KeyMapping Key { get; set; }
        public ICollectionContentsMapping Contents { get; set; }



    }

}